using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerShell : MonoBehaviour
{
    //열거형식으로 애니메이션을 제어하기 위함
    private enum eLookState
    {
        Down, Left, Up, Right
    }

    public Rigidbody2D rBody2d;
    //빈 오브젝트(Shell)의 자식오브젝트로 Sprite를 관리합니다.
    public GameObject player;

    //이동 관련
    [System.NonSerialized]
    public float playerMoveSpeed;
    private JoystickMove joystickMove;
    private Vector2 moveDir;
    private eLookState lookState;
    [System.NonSerialized]
    public Vector2 tmpDir;

    //대시 관련
    private Button btnDash;
    [System.NonSerialized]
    public bool isDash;
    private float dashTime;
    private float maxaDashTime = 0.15f;
    public Ghost ghost;
    public GameObject dashDustGO;

    public GunShell gunShell;

    public Animator anim;
    public SpriteRenderer playerSprite;
    public GameObject[] eyes;

    [System.NonSerialized]
    public float maxFireRate;
    private float fireRate;
    private Vector2 skillPoint;

    //스킬관련
    private JoystickSkillDir[] joystickSkillDir;
    private GunShell.eSkillType eSkillType;
    public GameObject[] indicators;
    public GameObject bg;

    public RelicBuilder relicBuilder;
    private int relicCnt;

    public GameObject vfxLevelUp;

    [System.NonSerialized]
    public bool isDead;

    [SerializeField]
    private AudioSource etcSource;

    void Start()
    {
        //Debug.Log(InfoManager.instance.gameInfo.roundCnt);

        this.isDead = false;

        this.joystickMove = GameObject.FindObjectOfType<JoystickMove>();
        this.btnDash = GameObject.Find("BtnDash").GetComponent<Button>();
        //플레이어의 초기 방향을 오른쪽으로 지정합니다.
        this.anim.SetInteger("LookState", 3);
        this.lookState = eLookState.Right;
        this.playerMoveSpeed = (0.05f * InfoManager.instance.charactoristicInfo.moveSpeedCharacteristic + 0.5f) * 500;

        //UI를 직접 변수로 저장하여 dash를 제어합니다.
        this.btnDash.onClick.AddListener(() =>
        {
            this.isDash = true;
            var go = Instantiate(this.dashDustGO);
            go.transform.position = this.transform.position;
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.PlayerDash, this.etcSource);
        });

        this.gunShell.Init();

        //스킬관련 UI 컴포넌트 연결
        this.joystickSkillDir = new JoystickSkillDir[3];
        this.joystickSkillDir[0] = GameObject.Find("JoystickSkillDirFirst").GetComponent<JoystickSkillDir>();
        this.joystickSkillDir[1] = GameObject.Find("JoystickSkillDirSecond").GetComponent<JoystickSkillDir>();
        this.joystickSkillDir[2] = GameObject.Find("JoystickSkillDirThird").GetComponent<JoystickSkillDir>();

        this.relicBuilder.Init();

        //스킬 사용
        //참조형식 변수(스킬)의 Action 대리자의 무명메서드 값을 할당합니다.        
        for (int i = 0; i < this.joystickSkillDir.Length; i++)
        {
            int idx = i;
            float bulletSpeed = 700f;
            float maxExistTime = 0.5f;
            int bulletDamage = InfoManager.instance.statInfo.BattleRate * 2;

            this.joystickSkillDir[idx].shootSkill += () =>
            {
                var go = Instantiate(this.gunShell.skillGo[idx]);
                
                go.transform.position = this.skillPoint;
                var angle = Mathf.Atan2(this.joystickSkillDir[idx].Vertical, this.joystickSkillDir[idx].Horizontal) * Mathf.Rad2Deg;
                var lookRotation = Quaternion.Euler(angle * Vector3.forward);

                //스킬이 탄환일 때
                if (go.GetComponent<Bullet>() != null) 
                {
                    go.transform.rotation = lookRotation;
                    go.GetComponent<Bullet>().Init(bulletSpeed, maxExistTime, bulletDamage, this.gunShell);
                    go.transform.position = this.gunShell.bulletPoint.transform.position;
                }  

                go.transform.rotation = lookRotation;

                //스킬이 액티브 스킬일 때
               if (go.GetComponent<PlayerActiveSkill>() != null) 
                {
                    go.GetComponent<PlayerActiveSkill>().Init(this.gunShell);
                    if (go.GetComponent<PlayerActiveSkill>().isCircle) go.transform.rotation = Quaternion.Euler(Vector3.zero);
                } 
            };

            //스킬이 버튼(즉발) 일 때
            this.gunShell.btnSkills[idx].onClick.AddListener(() =>
            {
                var go = Instantiate(this.gunShell.skillGo[idx]);
                if (go.GetComponent<Bullet>() != null) go.GetComponent<Bullet>().Init(bulletSpeed, maxExistTime, bulletDamage);
                if (go.GetComponent<PlayerActiveSkill>() != null) go.GetComponent<PlayerActiveSkill>().Init(this.gunShell);
            });
        }

        EventDispatcher.Instance.AddListener<string, int>(EventDispatcher.EventName.PlayerShellTakeRelic, this.TakeRelic);
    }

    private void FixedUpdate()
    {
        // 대시 중일 때 움직일 수 없습니다.
        if (!this.isDash) 
        {
            this.playerMoveSpeed = (0.05f * InfoManager.instance.charactoristicInfo.moveSpeedCharacteristic + 0.5f) * 500;
            if(this.joystickMove.Input == Vector3.zero) this.moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            else this.moveDir = new Vector2(this.joystickMove.Horizontal, this.joystickMove.Vertical);
            Vector2 movePos = this.moveDir.normalized * this.playerMoveSpeed * Time.deltaTime;
            this.rBody2d.velocity = movePos;
        }

        // -50 ~ -130, -130 ~ 130, 130 ~ 60, 60 ~ -50
        //총과 캐릭터의 애니메이션을 제어하고 있습니다.
        var angle = this.gunShell.Angle;
        if (angle < -50f && angle > -130f) this.lookState = eLookState.Down;
        else if (angle < -130f || angle > 130f) this.lookState = eLookState.Left;
        else if (angle < 130 && angle > 60) this.lookState = eLookState.Up;
        else if (angle < 60f && angle > -50) this.lookState = eLookState.Right;

        if ((int)this.lookState != this.anim.GetInteger("LookState"))
        {
            if (this.lookState == eLookState.Down) this.anim.SetInteger("LookState", 0);
            else if (this.lookState == eLookState.Left) this.anim.SetInteger("LookState", 1);
            else if (this.lookState == eLookState.Up) this.anim.SetInteger("LookState", 2);
            else if (this.lookState == eLookState.Right) this.anim.SetInteger("LookState", 3);  
        }

        if (this.moveDir != Vector2.zero) 
        {
            this.anim.SetBool("MoveState", true);
            this.tmpDir = this.moveDir;
        } 
        else this.anim.SetBool("MoveState", false);

        this.anim.SetBool("ShootState", this.gunShell.IsShoot);

        //해당 조건문 아래 기능이 추가될 때를 대비하여 중괄호를 표시하였습니다.

        //공격속도 구현
        if(this.fireRate <= this.maxFireRate + 1)
        {
            this.fireRate += Time.fixedDeltaTime;
        }
        if (this.gunShell.IsShoot && this.fireRate >= this.maxFireRate && !this.isDead)
        {
            this.fireRate = 0;
            this.gunShell.Shoot();
            this.gunShell.OneShotGunShotSFX();
        }

        if (this.isDash)
        {
            this.PlayerDash();
        }

        if (this.gunShell.IsFirstSkillOperated)
        {
            this.SetFirstSkill();
        }
        if (this.gunShell.IsSecondSkillOperated)
        {
            this.SetSecondSkill();
        }
        if (this.gunShell.IsThirdSkillOperated)
        {
            this.SetThirdSkill();
        }

        if (this.gunShell.IsAllSkillStoped())
        {
            for(int i = 0; i < this.indicators.Length; i++)
            {
                this.indicators[i].SetActive(false);
                this.indicators[i].transform.localPosition = Vector2.zero;
                this.bg.SetActive(false);
                this.joystickSkillDir[i].isButton = false;
            }
        }

        // Relic test
        //if (Input.GetKeyDown("1"))
        //{
        //    string goName = "LaserLine";
        //    EventDispatcher.Instance.Dispatch<string, int>(EventDispatcher.EventName.PlayerShellTakeRelic, goName, out relicNum);
        //    EventDispatcher.Instance.Dispatch<string, int>(EventDispatcher.EventName.UIRelicDirectorTakeRelic, goName, out relicNum);
        //    Debug.Log("Laser");
        //}

        //if (Input.GetKeyDown("2"))
        //{
        //    string goName = "DashAttack";
        //    EventDispatcher.Instance.Dispatch<string, int>(EventDispatcher.EventName.PlayerShellTakeRelic, goName, out relicNum);
        //    EventDispatcher.Instance.Dispatch<string, int>(EventDispatcher.EventName.UIRelicDirectorTakeRelic, goName, out relicNum);
        //    Debug.Log("DashAttack");
        //}

        //if (Input.GetKeyDown("3"))
        //{
        //    string goName = "PoisonBullet";
        //    EventDispatcher.Instance.Dispatch<string, int>(EventDispatcher.EventName.PlayerShellTakeRelic, goName, out relicNum);
        //    EventDispatcher.Instance.Dispatch<string, int>(EventDispatcher.EventName.UIRelicDirectorTakeRelic, goName, out relicNum);
        //    Debug.Log("PoisonBullet");
        //}

        //if (Input.GetKeyDown("4"))
        //{
        //    string goName = "GrenadeLauncher";
        //    EventDispatcher.Instance.Dispatch<string, int>(EventDispatcher.EventName.PlayerShellTakeRelic, goName, out relicNum);
        //    EventDispatcher.Instance.Dispatch<string, int>(EventDispatcher.EventName.UIRelicDirectorTakeRelic, goName, out relicNum);
        //    Debug.Log("GrenadeLauncher");
        //}

        //if (Input.GetKeyDown("5"))
        //{
        //    string goName = "DefensiveBullet";
        //    EventDispatcher.Instance.Dispatch<string, int>(EventDispatcher.EventName.PlayerShellTakeRelic, goName, out relicNum);
        //    EventDispatcher.Instance.Dispatch<string, int>(EventDispatcher.EventName.UIRelicDirectorTakeRelic, goName, out relicNum);
        //    Debug.Log("DefensiveBullet");
        //}
        //if (Input.GetKeyDown("6"))
        //{
        //    InfoManager.instance.statInfo.fireRateStat += 10;
        //}
    }

    //private int relicNum;

    public void PlayerDash()
    {
        this.ghost.makeGhost = true;
        this.isDash = true;        

        this.dashTime += Time.deltaTime;
        if (this.tmpDir == Vector2.zero) this.tmpDir = Vector2.right;
        this.rBody2d.velocity = this.tmpDir.normalized * 2000 * Time.deltaTime;
        if (this.dashTime >= this.maxaDashTime)
        {
            this.dashTime = 0;
            this.isDash = false;
            this.ghost.makeGhost = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(this.gameObject.transform.position + new Vector3(0, -0.2f, 0), new Vector2(0.7f, 1.0f));
    }

    public int TakeRelic(string relicName)
    {        
        this.relicCnt++;
        int rellicNum = this.relicCnt;
        bool isRelicNotFull = true;
        if (this.relicCnt > 3) isRelicNotFull = false;
        if (isRelicNotFull) 
        {
            if (relicName == "LaserLine") this.relicBuilder.parts[0].gameObject.SetActive(true);
            if (relicName == "DashAttack") this.relicBuilder.parts[1].gameObject.SetActive(true);
            if (relicName == "PoisonBullet") this.relicBuilder.parts[2].gameObject.SetActive(true);
            if (relicName == "GrenadeLauncher") this.relicBuilder.parts[3].gameObject.SetActive(true);
            if (relicName == "DefensiveBullet") this.relicBuilder.parts[4].gameObject.SetActive(true);
            this.relicBuilder.Init();
        }

        return rellicNum;
    }
}
