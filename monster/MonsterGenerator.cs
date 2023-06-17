using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject[] generatePoints;
    public List<GameObject> portalPointList; // 포탈 리스트
    private List<GameObject> monsterPrefabList;

    [System.NonSerialized]
    public Queue<GameObject> monsterPool;
    private Dictionary<int, Queue<GameObject>> monsterPools;
    private List<GameObject> monsterList;
    private int numberOfMonsters;
    private int monsterMultiplyer = 4;
    private float spawnInterval = 0.1f;
    public float generateInterval = 5f;
    private int maxWave = 6;
    private int difficultIdx;
    private int startDifficultyIdx;

    private int mostFarSpawnPoint;
    private int secondFarSpawnPoint;
    private int thirdFarSpawnPoint;

    private bool isAwaked;
    public bool isFinished;
    Coroutine generateRoutine;
    List<int> monsterIDList;

    private bool isRoomCleared;

    private ChestArrowController chestArrowController;

    public void Init()
    {
        this.monsterList = new List<GameObject>();
        this.player = this.transform.Find("PlayerShell(Clone)").gameObject;
        this.startDifficultyIdx = SetDifficultyID(InfoManager.instance.dungeonInfo.currentStepInfo); //인자 -> 룸 번호(난이도)
        string roomName = this.gameObject.name.Replace("(Clone)", "").Trim();
        this.monsterPrefabList = new List<GameObject>();
        // 몬스터 제너레이터와 Data를 연동합니다.
        var roomType = DataManager.Instance.GetRoomDataFromName(roomName).EnemyType;
        this.monsterIDList = DataManager.Instance.GetMonsetIDList(this.SetGroupID(InfoManager.instance.dungeonInfo.CurrentStageInfo, roomType)); //인자로 스테이지와 몬스터 타입

        for (int i = 0; i < monsterIDList.Count; i++)
        {
            int monsterID = monsterIDList[i];
            var prefabName = string.Format("Prefabs/Monsters/{0}", DataManager.Instance.GetMonsterData(monsterID).prefabPath);
            this.monsterPrefabList.Add(Resources.Load<GameObject>(prefabName));
        }

        this.monsterPools = new Dictionary<int, Queue<GameObject>>();
        foreach (var id in monsterIDList)
        {
            monsterPools[id] = new Queue<GameObject>();
        }

   
        this.chestArrowController = this.transform.GetComponentInChildren<ChestArrowController>();
        this.chestArrowController.Init();

        this.mostFarSpawnPoint = Random.Range(0, this.generatePoints.Length);
        this.secondFarSpawnPoint = Random.Range(0, this.generatePoints.Length);
        this.generateRoutine = StartCoroutine(this.GenerateMonstersRoutine());
        this.isAwaked = true;
    }

    void Update()
    {
        if (this.isAwaked)
        {
            this.mostFarSpawnPoint = 0;
            this.secondFarSpawnPoint = 0;
            float maxDist = 0;
            float secondMaxDist = 0;
            float thirdMaxDist = 0;

            for (int i = 0; i < this.generatePoints.Length; i++)
            {
                float distance = Vector2.Distance(this.player.transform.position, this.generatePoints[i].transform.position);
                if (distance > maxDist)
                {
                    thirdMaxDist = secondMaxDist;
                    this.thirdFarSpawnPoint = this.secondFarSpawnPoint;

                    secondMaxDist = maxDist;
                    this.secondFarSpawnPoint = this.mostFarSpawnPoint;

                    maxDist = distance;
                    this.mostFarSpawnPoint = i;
                }
                else if (distance > secondMaxDist)
                {
                    thirdMaxDist = secondMaxDist;
                    this.thirdFarSpawnPoint = this.secondFarSpawnPoint;

                    secondMaxDist = distance;
                    this.secondFarSpawnPoint = i;
                }
                else if (distance > thirdMaxDist)
                {
                    thirdMaxDist = distance;
                    this.thirdFarSpawnPoint = i;
                }
            }

            if (AllMonstersInactive())
            {
                if (this.generateRoutine != null) StopCoroutine(this.generateRoutine);
                this.generateRoutine = StartCoroutine(this.GenerateMonstersRoutine());

                if (this.maxWave <= 0 && !this.isFinished && AllMonstersInactive())
                {
                    this.RoomClearInitializing();
                }
            }
        }

    }

    private void RoomClearInitializing()
    {
        this.isFinished = true;
        this.portalPointList.ForEach((x) => x.SetActive(true));
        this.GetAllFieldCoins();
        EventDispatcher.Instance.Dispatch<Transform>(EventDispatcher.EventName.UIPortalArrowControllerInitializingArrows,
            this.transform);
        EventDispatcher.Instance.Dispatch<Transform, bool>(EventDispatcher.EventName.ChestItemGeneratorMakeChest,
            this.transform);
        EventDispatcher.Instance.Dispatch<UIAnnounceDirector.eAnnounceType>(EventDispatcher.EventName.UIAnnounceDirectorStartAnnounce,
            UIAnnounceDirector.eAnnounceType.ROOM);
        this.chestArrowController.StartArrowAnimation();
        InfoManager.instance.ChangeDungeonStepInfo();

        this.isRoomCleared = true;
        foreach (var pools in this.monsterPools)
        {
            foreach (var monster in pools.Value)
            {
                var poison = monster.GetComponentInChildren<VfxPoison>();
                if (poison != null) poison.transform.SetParent(null);
                Destroy(monster);
            }
            pools.Value.Clear();
        }
    }

    private void GetAllFieldCoins()
    {
        var coins = GameObject.FindGameObjectsWithTag("FieldCoin").ToList();
        coins.ForEach(x =>{x.GetComponent<DropItem>().ClickedItemCheck();});                   
    }
        
    private bool AllMonstersInactive()
    {
        if (!this.isRoomCleared)
        {
            foreach (GameObject monster in monsterList)
            {
                if (monster.activeSelf)
                {
                    return false;
                }
            }
        }

        return true;
    }

    IEnumerator GenerateMonstersRoutine()
    {
        while (this.maxWave >= 0)
        {            
            var difficulty = DataManager.Instance.GetDifficultyData(this.difficultIdx + this.startDifficultyIdx);
            this.numberOfMonsters = difficulty.monsterAmount * this.monsterMultiplyer;
            int monsterID = 0;
            int spawnNum = 0;

            for (int i = 0; i < this.numberOfMonsters; i++)
            {
                int monsterIdx = i % this.monsterIDList.Count;
                monsterID = this.monsterIDList[monsterIdx];
                GameObject monsterGo;

                if (this.monsterPools[monsterID].Count > 0)
                {
                    monsterGo = this.monsterPools[monsterID].Dequeue();
                    if (monsterGo.activeSelf) monsterGo = Instantiate(this.monsterPrefabList[monsterIdx]);                    
                }
                else
                {
                    monsterGo = Instantiate(this.monsterPrefabList[monsterIdx]);
                }

                monsterGo.GetComponent<Monster>().Init(this, difficulty.monsterHP, difficulty.monsterDefense);
                monsterGo.GetComponent<Monster>().id = monsterID;
                this.monsterList.Add(monsterGo);
                monsterGo.gameObject.name = monsterGo.gameObject.name + spawnNum.ToString();

                int randomSpawn = Random.Range(0, 3);
                int spawnPointNum;
                // 플레이어와 먼 3 포인트에서 몬스터를 생성합니다.
                if (randomSpawn == 0)
                {
                    spawnPointNum = this.mostFarSpawnPoint;
                }
                else if (randomSpawn == 1)
                {
                    spawnPointNum = this.secondFarSpawnPoint;
                }
                else
                {
                    spawnPointNum = this.thirdFarSpawnPoint;
                }

                monsterGo.transform.position = this.generatePoints[spawnPointNum].transform.position;
                monsterGo.SetActive(true);
                // 몬스터의 sortingorder 정리
                monsterGo.GetComponent<SpriteRenderer>().sortingOrder = spawnNum;
                spawnNum++;
                yield return new WaitForSeconds(this.spawnInterval);
            }
            this.difficultIdx++;
            this.maxWave--;
            yield return new WaitForSeconds(this.generateInterval);
        }
    }

    private int SetDifficultyID(int roomNum)
    {
        int id = 13000;

        if (roomNum >= 1 && roomNum <= 15)
        {
            id += (roomNum - 1) * 12;
        }

        return id;
    }

    private int SetGroupID(int stageNum, string monsterType)
    {
        int id = 15000 + (stageNum - 1) * 6;

        switch (monsterType)
        {
            case "melee":
                break;
            case "ranged":
                id += 1;
                break;
            case "meleeRanged":
                id += 2;
                break;
            case "meleeExplosive":
                id += 3;
                break;
            case "rangedExplosive":
                id += 4;
                break;
            case "meleeRangedExplosive":
                id += 5;
                break;
        }

        return id;
    }

    public void ReturnMonsterToPool(int monsterID, GameObject monster)
    {
        this.monsterPools[monsterID].Enqueue(monster);
    }
}
