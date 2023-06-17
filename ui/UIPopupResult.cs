using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupResult : MonoBehaviour
{
    public Text txtLastStage;
    public Text txEarnEther;
    public Text txtDepositCoin;
    public Text txtRoundCnt;
    public UIPopupCoinDetail uiPopupCoinDetail;

    public Button[] btnCloses;
    public Button btnCheckCoinDetail;

    [SerializeField]
    private AudioSource sfxSource;

    public void Init()
    {
        var stageName = "";
        var stageInfo = InfoManager.instance.dungeonInfo.CurrentStageInfo;
        switch (stageInfo)
        {
            case 1: stageName = "½£";
                break;
            case 2:
                stageName = "¹¦Áö";
                break;
            case 3:
                stageName = "»ç¿ø";
                break;
            case 4:
                stageName = "¾Ç¸¶ÀÇ ½ÉÀå";
                break;
            default: stageName = "½£";
                break;
        }

        this.txtLastStage.text = stageName;
        if (InfoManager.instance.dungeonInfo.isClearStage4) this.txtLastStage.text = string.Format("{0}¹øÂ° À±È¸ ¼º°ø", InfoManager.instance.gameInfo.roundCnt -1);
        this.txtRoundCnt.text = string.Format("{0}¹øÂ° À±È¸({1}È¸Â÷) ÁøÇà ÁßÀÔ´Ï´Ù.", InfoManager.instance.gameInfo.roundCnt - 1, InfoManager.instance.gameInfo.roundCnt);

        int sum = 0;
        var tupleList = InfoManager.instance?.MakeDipositTupleList();
        if (tupleList != null && tupleList.Count > 0)
        {
            foreach (var tuple in tupleList)
            {
                sum += tuple.Item2;
            }
        }

        this.txEarnEther.text = InfoManager.instance.possessionAmountInfo.totalDungeonEther.ToString();
        this.txtDepositCoin.text = sum.ToString();

        this.btnCheckCoinDetail.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
            this.uiPopupCoinDetail.Init();
            this.uiPopupCoinDetail.gameObject.SetActive(true);
        });

        for(int i = 0; i < this.btnCloses.Length; i++)
        {
            int idx = i;
            this.btnCloses[idx].onClick.RemoveAllListeners();

            this.btnCloses[idx].onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
                this.gameObject.SetActive(false);
            });
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
