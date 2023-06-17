using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKoboldPriest : AINormalRange
{
    private int attackCnt;
    [SerializeField]
    private int maxAttackCnt;

    public override void Init(Transform player)
    {
        base.Init(player);
        this.attackCnt = 0;
    }

    // 애니메이션 clip의 Event로 제어합니다.
    protected void InstantateMoreBullet()
    {
        StartCoroutine(this.InstantateBulletRoutine());
    }

    IEnumerator InstantateBulletRoutine()
    {
        while (true)
        {
            this.InstantiateBullet();
            this.attackCnt++;            
            yield return new WaitForSeconds(0.05f); 
            if(this.attackCnt >= this.maxAttackCnt) 
            {
                this.attackCnt = 0;
                break;
            }
        }
    }

    protected override void InstantiateBullet()
    {
        GameObject go;
        float angle = Mathf.Atan2(this.dirPlayer.y, this.dirPlayer.x) * Mathf.Rad2Deg;
        var lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        var pool = MonsterBulletPooler.instance.koboldPriestBulletPool;

        if (pool.Count > 0)
        {
            go = pool.Dequeue();
            if (go.activeSelf) go = Instantiate(this.bulletGo);            
        }
        else
        {
            go = Instantiate(this.bulletGo);
        }

        go.transform.position = this.bulletPoint.transform.position;
        go.SetActive(true);
        go.transform.rotation = lookRotation;
        go.GetComponent<MonsterBullet>().Init(pool);
    }
}
