using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.EventSystems;

public class UIRelicDirector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image[] imgRelic;
    [SerializeField]
    private SpriteAtlas sa;
    private int relicCnt;
    [SerializeField]
    private RectTransform[] rects;
    [SerializeField]
    private GameObject[] relicDescPopups;
    [SerializeField]
    private Text[] txtName;
    [SerializeField]
    private Text[] txtDesc;

    private bool[] isPopupActive;
        
    public void Init() 
    {
        EventDispatcher.Instance.AddListener<string, int>(EventDispatcher.EventName.UIRelicDirectorTakeRelic, this.TakeRelic);

        this.isPopupActive = new bool[relicDescPopups.Length];
    }

    // 이벤트를 붙여 얻은 relic을 Ui에 추가합니다.
    public int TakeRelic(string relicName)
    {
        bool isRelicNotFull = true;
        if (this.relicCnt >= 3) isRelicNotFull = false;
        if (isRelicNotFull)
        {
            this.imgRelic[this.relicCnt].gameObject.SetActive(true);
            this.imgRelic[this.relicCnt].sprite = this.sa.GetSprite(relicName);

            var relicData = DataManager.Instance.GetRelicDataFromPrefabName(relicName);

            this.txtName[this.relicCnt].text = relicData.name;
            this.txtDesc[this.relicCnt].text = relicData.disc;
            this.rects[this.relicCnt].gameObject.SetActive(true);
        }
        
        this.relicCnt++;

        int relicNum = this.relicCnt;
        return relicNum;
    }

    // 터치하면 정보를 표시합니다.
    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < this.rects.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(this.rects[i], eventData.position))
                this.isPopupActive[i] = true;
            else
                this.isPopupActive[i] = false;
        }

        for (int i = 0; i < this.relicDescPopups.Length; i++)
        {
            if (this.isPopupActive[i] && this.rects[i].gameObject.activeSelf)
                this.relicDescPopups[i].SetActive(true);
            else
                this.relicDescPopups[i].SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        for(int i = 0; i < this.relicDescPopups.Length; i++)
        {
            this.relicDescPopups[i].SetActive(false);
        }        
    }    
}
