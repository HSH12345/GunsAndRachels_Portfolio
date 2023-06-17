using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class UIStatsUpgrade : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Text[] txtBattleRate;
    public Text[] txtCurrentEthers;
    public Text txtDemandEther;
    public Text[] txtCurrentAbilities;
    public Text[] txtIncreasedAbilities;

    public Button[] btnDowngrades;
    public RectTransform[] rectDowngrades;
    public Button[] btnUpgrades;
    public RectTransform[] rectUpgrades;

    //강화 확인
    public Button btnSelect;
    public GameObject uiPopupSelect;

    public Button[] btnCloses;

    //능력치 초기화
    public Button btnReset;
    public GameObject uiPopupReset;

    private int battleRate;
    private int IncresedBattleRate;
    private int currentEther;
    //공격력, 공격속도, 사거리, 이동속도, 넉백
    private int[] startStats = new int[4];
    private int[] currentStats = new int[4];

    [System.NonSerialized]
    public int totalCost;
    private int cost;

    private bool[] isUDowngrading = new bool[4];
    private bool[] isUpgrading = new bool[4];
    private bool isOnTouch;
    private float touchDelay;
    private float delta;

    [SerializeField]
    private AudioSource sfxSource;

    public void Init()
    {
        //초기 값 설정
        this.isOnTouch = false;
        this.touchDelay = 1f;
        this.totalCost = 0;
        this.txtDemandEther.text = this.totalCost.ToString();
        this.battleRate = InfoManager.instance.statInfo.BattleRateOrigin;
        this.IncresedBattleRate = InfoManager.instance.statInfo.BattleRateOrigin;
        this.txtBattleRate[0].text = this.battleRate.ToString();
        this.txtBattleRate[1].text = this.battleRate.ToString();
        this.currentEther = InfoManager.instance.possessionAmountInfo.etherAmount;
        this.txtCurrentEthers[0].text = this.currentEther.ToString();
        this.cost = InfoManager.instance.statInfo.BattleRateOrigin;

        this.startStats[0] = InfoManager.instance.statInfo.powerStatOrigin;
        this.startStats[1] = InfoManager.instance.statInfo.fireRateStatOrigin;
        this.startStats[2] = InfoManager.instance.statInfo.criticalHitChanceOrigin;
        this.startStats[3] = InfoManager.instance.statInfo.criticalHitAmountOrigin;

        for (int i = 0; i < this.startStats.Length; i++)
        {
            this.currentStats[i] = this.startStats[i];
        }

        this.txtCurrentAbilities[0].text = InfoManager.instance.statInfo.powerStatOrigin.ToString();
        this.txtCurrentAbilities[1].text = InfoManager.instance.statInfo.fireRateStatOrigin.ToString();
        this.txtCurrentAbilities[2].text = InfoManager.instance.statInfo.criticalHitChanceOrigin.ToString();
        this.txtCurrentAbilities[3].text = InfoManager.instance.statInfo.criticalHitAmountOrigin.ToString();

        this.txtIncreasedAbilities[0].text = InfoManager.instance.statInfo.powerStatOrigin.ToString();
        this.txtIncreasedAbilities[1].text = InfoManager.instance.statInfo.fireRateStatOrigin.ToString();
        this.txtIncreasedAbilities[2].text = InfoManager.instance.statInfo.criticalHitChanceOrigin.ToString();
        this.txtIncreasedAbilities[3].text = InfoManager.instance.statInfo.criticalHitAmountOrigin.ToString();

        //실제 UI로직
        Color green = Color.green;
        Color red = Color.red;
        Color white = Color.white;

        //초기 색 지정
        this.txtDemandEther.color = white;
        this.txtCurrentEthers[1].color = white;
        this.txtBattleRate[1].color = white;
        this.txtBattleRate[1].color = white;

        for (int i = 0; i < this.txtIncreasedAbilities.Length; i++)
        {
            this.txtIncreasedAbilities[i].color = white;
        }

        this.btnSelect.onClick.RemoveAllListeners();
        this.btnSelect.onClick.AddListener(() =>
        {
            for(int i = 0; i < this.currentStats.Length; i++)
            {
                this.currentStats[i] -= this.startStats[i];
                //Debug.LogFormat("idx{0} = {1}", i, this.currentStats[i]);
            }
            this.uiPopupSelect.GetComponent<UIPopUpStatsUpgradeSelect>().Init(this.currentStats, this.totalCost);
            this.uiPopupSelect.SetActive(true);
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
        });

        for (int i = 0; i < this.btnCloses.Length; i++)
        {
            int idx = i;

            this.btnCloses[idx].onClick.RemoveAllListeners();
            this.btnCloses[idx].onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
                this.gameObject.SetActive(false);
            });
        }

        this.btnReset.onClick.RemoveAllListeners();
        this.btnReset.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
            this.uiPopupReset.SetActive(true);
        });
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < this.rectDowngrades.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(this.rectDowngrades[i], eventData.position))
            {
                this.isUDowngrading[i] = true;
                this.DowngradeStat(i);
                AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
            }
        }

        for (int i = 0; i < this.rectUpgrades.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(this.rectUpgrades[i], eventData.position))
            {
                this.isUpgrading[i] = true;
                this.UpgradeStat(i);
                AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        for (int i = 0; i < this.rectDowngrades.Length; i++)
        {
            this.isUpgrading[i] = false;
            this.isUDowngrading[i] = false;
        }

        this.touchDelay = 1f;
        this.isOnTouch = false;
    }

    // rect에 터치된 상태를 감지하여 UI를 조작합니다.
    private void FixedUpdate()
    {
        for (int i = 0; i < this.isUDowngrading.Length; i++)
        {
            if (this.isOnTouch && (this.isUDowngrading[i] || this.isUpgrading[i]))
            {
                this.delta += Time.deltaTime;
                if (this.delta >= this.touchDelay)
                {
                    if (this.isUDowngrading[i])
                    {
                        this.DowngradeStat(i);
                    }
                    else if (this.isUpgrading[i])
                    {
                        this.UpgradeStat(i);
                    }
                    AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);

                    if (this.touchDelay >= 0.08f) this.touchDelay /= 2f;
                    else this.touchDelay = 0.08f;

                    this.delta = 0;
                }
            }
        }

        if (!this.isOnTouch) this.isOnTouch = true;
    }

    public void DowngradeStat(int idx)
    {
        if (this.startStats[idx] < this.currentStats[idx])
        {
            this.cost--;
            this.currentStats[idx]--;
            this.txtIncreasedAbilities[idx].text = this.currentStats[idx].ToString();
            this.IncresedBattleRate = this.currentStats.Sum();
            this.txtBattleRate[1].text = this.IncresedBattleRate.ToString();
            this.totalCost -= this.cost;
            this.txtDemandEther.text = this.totalCost.ToString();
            this.currentEther += this.cost;
            this.txtCurrentEthers[1].text = this.currentEther.ToString();

            this.SetColor(idx);
        }
    }

    public void UpgradeStat(int idx)
    {
        if (this.currentEther - this.cost >= 0)
        {
            this.currentStats[idx]++;
            this.txtIncreasedAbilities[idx].text = this.currentStats[idx].ToString();
            this.IncresedBattleRate = this.currentStats.Sum();
            this.txtBattleRate[1].text = this.IncresedBattleRate.ToString();
            this.totalCost += this.cost;
            this.txtDemandEther.text = this.totalCost.ToString();
            this.currentEther -= this.cost;
            this.txtCurrentEthers[1].text = this.currentEther.ToString();
            this.cost++;

            this.SetColor(idx);
        }
    }

    public void SetColor(int idx)
    {
        Color green = Color.green;
        Color red = Color.red;
        Color white = Color.white;

        this.txtDemandEther.color = white;
        this.txtCurrentEthers[1].color = white;
        this.txtBattleRate[1].color = white;
        this.txtBattleRate[1].color = white;

        if (this.currentStats[idx] > this.startStats[idx])
        {
            this.txtIncreasedAbilities[idx].color = green;
        }
        else
        {
            this.txtIncreasedAbilities[idx].color = white;
        }

        if (this.IncresedBattleRate > this.battleRate)
        {
            this.txtBattleRate[1].color = green;
        }
        else
        {
            this.txtBattleRate[1].color = white;
        }

        if (this.totalCost != 0)
        {
            this.txtDemandEther.color = red;
            this.txtCurrentEthers[1].color = red;
        }
        else
        {
            this.txtDemandEther.color = white;
            this.txtCurrentEthers[1].color = white;
        }
    }

    private void OnEnable()
    {
        AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Open, this.sfxSource);
    }

    private void OnDisable()
    {
        AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, this.sfxSource);
    }
}
