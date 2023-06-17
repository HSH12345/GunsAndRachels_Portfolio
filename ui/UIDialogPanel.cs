using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 데이터를 제외한 기능 구현하였습니다.
public class UIDialogPanel : MonoBehaviour
{
    public enum eDialogType
    {
        NONE = -1,
        TUTORIALBASIC,
        TUTORIALSTAT,
        TUTORIALSHOP,
        TUTORIALRESULT,
        TUTORIALKNIGHTDIPOSIT,
        TUTORIALROGUEDIPOSIT,
        TUTORIALDICE,
    }

    [SerializeField]
    private Text txtNPCName;

    public Text txtDialog;
    public Button btnDim;
    public Animator anim;
    public GameObject endArrowGo;

    [SerializeField]
    private Image portrait;
    private int idx;

    [SerializeField]
    private float charPerSecond;
    private string targetDialog;

    private int diaologIdx;
    private bool isTalking;

    // 던전 입장 시 대화
    private string dialogA = "이봐 레이첼,새 무기를 받아가도록해";
    private string dialogB = "건승을 빈다 레이첼";

    private string gun;

    private List<DialogData> dialogDataList;
    private List<string> dialogList;
    private List<string> npcNameList;
    private List<string> SpriteNameList;

    public event Action OnDialogEnd;

    private AudioSource audioSource;

    private string[] gunArr = { "AssultRifle", "ShotGun", "SubmachineGun", "SniperRifle" };

    public void Init(){

        this.dialogDataList = new List<DialogData>();
        this.dialogList = new List<string>();
        this.npcNameList = new List<string>();
        this.SpriteNameList = new List<string>();
        this.audioSource = this.GetComponentInParent<AudioSource>();    

        EventDispatcher.Instance.AddListener<Action>(EventDispatcher.EventName.UIDialogPanelRandomWeaponDialog,
            this.RandomWeaponDialog);
        EventDispatcher.Instance.AddListener<eDialogType>(EventDispatcher.EventName.UIDialogPanelStartDialog, 
            this.StartDialog);

        this.gameObject.SetActive(false);    
    }

    /// <summary>
    /// 무기 랜덤 변경 대화 (기사단장과의 대화) 
    /// </summary>
    private void RandomWeaponDialog(Action onPortalAnim)
    {
        this.gameObject.SetActive(true);


        //다이얼 로그 관련 필드 초기화
        this.btnDim.onClick.RemoveAllListeners();
        this.idx = 0;
        this.isTalking = false;

        //다이얼로그 데이터 준비
        this.dialogDataList.Clear();
        this.dialogList.Clear();
        this.npcNameList.Clear();
        this.SpriteNameList.Clear();

        this.dialogList.Add(this.dialogA);
        this.txtNPCName.text = "기사단장";

        //랜덤 총기 선택
        var random = new System.Random();
        var ran = random.Next(0, 4);
        this.gun = this.gunArr[ran];
        if (this.gun == "AssultRifle")
        {
            this.dialogList.Add("여기 \"AK47\" 를 가져가.");
            this.dialogList.Add("돌격소총계열의 무기로 아주 믿음직한 친구지!");
        }
        else if (this.gun == "SniperRifle")
        {
            this.dialogList.Add("여기 \"AWP\" 를 가져가.");
            this.dialogList.Add("저격총은 한발 한발의 위력이 아주 매서운 무기지!");
        }
        else if (this.gun == "ShotGun")
        {
            this.dialogList.Add("여기 \"SPAS-12\" 를 가져가.");
            this.dialogList.Add("아! 샷건! 아주 좋은 대화의 수단이지!!");
        }
        else if (this.gun == "SubmachineGun") 
        {
            this.dialogList.Add("여기 \"UZI\" 를 가져가.");
            this.dialogList.Add("기관단총은 연사력 하나만으로 아주 예술인 무기이지!");      
        }

        this.dialogList.Add(this.dialogB);

        //아틀라스 매니저를 사용하여 대화중인 NPC로 이미지 변경
        this.portrait.sprite = AtlasManager.instance.GetAtlasByName("UINPCPortraitIcon").GetSprite("EliteKnight_Idle_1");
        this.portrait.transform.localScale = new Vector3(50, 50, 50);
        var pos = this.portrait.transform.localPosition;
        this.portrait.transform.localPosition = new Vector3(pos.x, pos.y + 350, pos.z);

        this.SetDialog(this.dialogList[this.idx]);
        this.anim.Play("UIDialogPanel_Portrait", -1, 0);
        this.btnDim.onClick.AddListener(() =>
        {
            if (!this.isTalking)
            {
                Debug.Log(this.audioSource);
                AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.audioSource);
                this.anim.Play("UIDialogPanel_Portrait", -1, 0);
                this.idx++;
                if (this.idx >= this.dialogList.Count)
                {
                    this.idx = 0;
                    EventDispatcher.Instance.Dispatch<string>(EventDispatcher.EventName.DungeonSceneMainTakeGun,this.gun);
                    onPortalAnim();
                    this.gameObject.SetActive(false);
                }               
            }           
            this.SetDialog(this.dialogList[this.idx]);
        });
    }

    /// <summary>
    /// NPC다이얼로그를 타입에 따라 대화를 진행합니다.
    /// </summary>
    /// <param name="type">다이얼로그 타입</param>
    private void StartDialog(eDialogType type)
    {
        this.gameObject.SetActive(true);
   
        this.SetDialogList(type);
        this.SetDialog(this.dialogList[this.idx]);
        this.SetPortraitAndName(this.npcNameList[this.idx],this.SpriteNameList[this.idx]);

        this.anim.Play("UIDialogPanel_Portrait", -1, 0);

        this.btnDim.onClick.AddListener(() =>
        {
            if (!this.isTalking)
            {
                this.anim.Play("UIDialogPanel_Portrait", -1, 0);

                this.idx++;
                if (this.idx >= this.dialogList.Count)
                {
                    var pos = this.portrait.transform.localPosition;
                    if (type == eDialogType.TUTORIALDICE) this.portrait.transform.localPosition = new Vector3(pos.x, pos.y - 200, pos.z);
                    else this.portrait.transform.localPosition = new Vector3(pos.x, pos.y - 350, pos.z);
                    this.idx = 0;
                    this.OnDialogEnd?.Invoke();
                    this.OnDialogEnd = null;
                    this.gameObject.SetActive(false);
                }

            }
            this.SetPortraitAndName(this.npcNameList[this.idx], this.SpriteNameList[this.idx]);
            this.SetDialog(this.dialogList[this.idx]);
        });
    }

    private void SetDialogList(eDialogType type)
    {
        //다이얼 로그 관련 필드 초기화
        this.btnDim.onClick.RemoveAllListeners();
        this.idx = 0;
        this.isTalking = false;
        var pos = this.portrait.transform.localPosition;
        if(type == eDialogType.TUTORIALDICE) this.portrait.transform.localPosition = new Vector3(pos.x, pos.y + 200, pos.z);
        else this.portrait.transform.localPosition = new Vector3(pos.x, pos.y + 350, pos.z);

        //다이얼로그 데이터 준비
        this.dialogDataList.Clear();
        this.dialogList.Clear();
        this.npcNameList.Clear();
        this.SpriteNameList.Clear();
        //다이얼 로그 관련 리스트 할당
        this.dialogDataList = DataManager.Instance.GetDialog(type);
        this.dialogDataList.ForEach((x) => { this.dialogList.Add(x.dialogKOR); });
        this.dialogDataList.ForEach((x) => { this.SpriteNameList.Add(x.npcSpriteName); });
        this.dialogDataList.ForEach((x) => { this.npcNameList.Add(x.npcName); });
    }

    public void SetDialog(string dialog)
    {
        if (this.isTalking) 
        {
            this.txtDialog.text = this.targetDialog;
            CancelInvoke();            
            this.EndEffect();
        }
        else
        {
            this.targetDialog = dialog;
            this.StartEffect();
        }
    }

    private void SetPortraitAndName(string NPCName, string NPCPortraitName)
    {
        this.txtNPCName.text = NPCName;
        if (NPCPortraitName == "")
        {
            this.portrait.sprite = AtlasManager.instance.GetAtlasByName("UINPCPortraitIcon").GetSprite("nullImage");
        }
        else
        {
            this.portrait.sprite = AtlasManager.instance.GetAtlasByName("UINPCPortraitIcon").GetSprite(NPCPortraitName);
            this.portrait.transform.localScale = new Vector3(50, 50, 50);         
        }
    }

    //애니메이션 제어 3단계
    private void StartEffect()
    {
        this.txtDialog.text = "";
        this.diaologIdx = 0;
        this.endArrowGo.SetActive(false);
        this.isTalking = true;

        //1글자가 나오는 속도
        Invoke("OnEffect", 1 / this.charPerSecond);
    }

    private void OnEffect()
    {
        if (this.txtDialog.text == this.targetDialog)
        {
            this.EndEffect();
            return;
        }
        
        this.txtDialog.text += this.targetDialog[this.diaologIdx];
        this.diaologIdx++;
        //재귀적 사용
        Invoke("OnEffect", 1 / this.charPerSecond);
    }

    private void EndEffect()
    {
        this.endArrowGo.SetActive(true);
        this.isTalking = false;
    }
}
