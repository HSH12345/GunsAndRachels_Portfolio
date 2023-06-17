using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpStatsReset : MonoBehaviour
{
    public Button[] btnCloses;
    public Button btnSelect;
    public GameObject uiMessage;

    [SerializeField]
    private AudioSource sfxSource;

    void Start()
    {
        this.btnSelect.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
            bool isEnough = InfoManager.instance.DecreaseEther(100);
            if (isEnough)
            {
                int sum = 0;
                for(int i = 40; i < InfoManager.instance.statInfo.BattleRateOrigin; i++)
                {
                    sum += i;
                }
                InfoManager.instance.IncreaseEther(sum);
                InfoManager.instance.ResetStats();
                this.gameObject.SetActive(false);
                FindObjectOfType<UIStatsUpgrade>().gameObject.SetActive(false);
            }
            else
            {
                var go = Instantiate(this.uiMessage, this.transform);
                string message = "에테르가 부족합니다.";
                go.GetComponent<UIPopupMessage>().Init(message);
            }
        });

        for (int i = 0; i < this.btnCloses.Length; i++)
        {
            int idx = i;
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
