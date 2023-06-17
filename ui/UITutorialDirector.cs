using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorialDirector : MonoBehaviour
{
    public Button[] btnOverviewHighers;
    public GameObject[] overviewLowerGOs;
    public Button[] btnControlls;
    public GameObject[] descControllsGOs;
    public Button[] btnSystems;
    public GameObject[] descSystemsGOs;
    public Button[] btnSanctuary;
    public GameObject[] descSanctuaryGOs;
    public Button[] btnDungeon;
    public GameObject[] descDungeonGOs;
    public GameObject[] descFirstGOs;

    private GameObject currentOverviewLower;
    private GameObject currentDescGO;

    public GameObject uiTutorialGO;
    public Button[] btnCloses;

    [SerializeField]
    private AudioSource sfxSource;

    //public void Start()
    //{
    //    this.Init();
    //}

    public void Init()
    {
        this.currentOverviewLower = this.overviewLowerGOs[0];
        this.currentDescGO = this.descControllsGOs[0];

        //overviewLower 표기
        for (int i = 0; i < this.btnOverviewHighers.Length; i++)
        {
            int idx = i;
            this.btnOverviewHighers[idx].onClick.AddListener(() =>
            {
                this.ShowOverviewLower(idx);
                AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
            });
        }

        //각각 desc 표기
        this.AddClickListener(this.btnControlls, this.descControllsGOs);
        this.AddClickListener(this.btnSystems, this.descSystemsGOs);
        this.AddClickListener(this.btnSanctuary, this.descSanctuaryGOs);
        this.AddClickListener(this.btnDungeon, this.descDungeonGOs);

        //close
        for (int i = 0; i < this.btnCloses.Length; i++)
        {
            int idx = i;
            this.btnCloses[idx].onClick.AddListener(() =>
            {
                this.uiTutorialGO.SetActive(false);
                AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
            });
        }
    }

    private void ShowOverviewLower(int idx)
    {
        this.currentOverviewLower.SetActive(false);
        this.currentOverviewLower = overviewLowerGOs[idx];
        this.currentOverviewLower.SetActive(true);
        this.currentOverviewLower.transform.Find("contents").GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        this.currentDescGO.SetActive(false);
        this.currentDescGO = this.descFirstGOs[idx];
        this.currentDescGO.SetActive(true);
        this.currentDescGO.transform.Find("textScrollView/contents").GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    private void AddClickListener(Button[] buttons, GameObject[] descriptions)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int idx = i;
            buttons[idx].onClick.AddListener(() => this.ShowDescription(descriptions[idx]));
        }
    }

    private void ShowDescription(GameObject description)
    {
        this.currentDescGO.SetActive(false);
        this.currentDescGO = description;
        this.currentDescGO.SetActive(true);
        this.currentDescGO.transform.Find("textScrollView/contents").GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
    }

    public void Show()
    {
        this.uiTutorialGO.SetActive(true);
    }
}