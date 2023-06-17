using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupCoinDetail : MonoBehaviour
{
    public Button[] btnCloses;
    public GameObject[] CoinDetails;
    private List<System.Tuple<int, int>> depositList;

    [SerializeField]
    private AudioSource sfxSource;

    public void Init()
    {
        // ¾Õ : ¼º°ø±Ý¾× , µÚ : ÃÑ µðÆÄÁþ ±Ý¾×
        if (InfoManager.instance.MakeDipositTupleList() != null)
        {
            this.depositList = InfoManager.instance.MakeDipositTupleList();
        }
        else this.depositList.Add(new System.Tuple<int, int>(0, 0));

        for (int i = 0; i < this.btnCloses.Length; i++)
        {            
            int idx = i;
            this.btnCloses[idx].onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Click, this.sfxSource);
                this.gameObject.SetActive(false);
            });
        }

        for(int i = 0; i < this.depositList.Count; i++)
        {
            this.CoinDetails[i].SetActive(true);
            this.CoinDetails[i].transform.Find("txtCoinDetail").GetComponent<Text>().text = 
                string.Format("{0}½ºÅ×ÀÌÁö : {1}/{2} È¹µæ", i + 1, this.depositList[i].Item1, this.depositList[i].Item2);
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
