using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGunCharacteristicChoice : MonoBehaviour
{
    //이속, 넉백, 탄환개수, 대시회복, 관통 순
    public GameObject[] characteristicsGo;
    public Button[] btnCharacteristics;
    private int[] characteristics;
    [SerializeField]
    private GameObject pauseGO;

    public void UnShow()
    {
        if (!this.pauseGO.activeSelf)
        {
            Time.timeScale = 1;
        }

        this.gameObject.SetActive(false);
    }

    public void Show()
    {

        var gunCharacteristics = InfoManager.instance.charactoristicInfo;
        Time.timeScale = 0;
        for(int i = 0; i < this.characteristicsGo.Length; i++)
        {
            this.characteristicsGo[i].SetActive(false);
        }

        this.gameObject.SetActive(true);
        this.characteristics = new int[5];

        //총기특성, 레벨 관련 Info 필요
        this.characteristics[0] = gunCharacteristics.moveSpeedCharacteristic;
        this.characteristics[1] = gunCharacteristics.kncokBackCharacteristic;
        this.characteristics[2] = gunCharacteristics.bulletAmountCharacteristic;
        this.characteristics[3] = gunCharacteristics.dashRecoverCharacteristic;
        this.characteristics[4] = gunCharacteristics.penetrateCharacteristic;
        this.ActiveRandomCharacteristics(3);

        for (int i = 0; i < this.btnCharacteristics.Length; i++)
        {
            int idx = i;
            this.btnCharacteristics[idx].onClick.RemoveAllListeners();

            this.btnCharacteristics[idx].onClick.AddListener(() =>
            {
                switch (idx)
                {
                    case 0:
                        gunCharacteristics.moveSpeedCharacteristic++;
                        gunCharacteristics.originMoveSpeedCharacteristic++;
                        break;
                    case 1:
                        gunCharacteristics.kncokBackCharacteristic++;
                        gunCharacteristics.originKncokBackCharacteristic++;
                        break;
                    case 2:
                        gunCharacteristics.bulletAmountCharacteristic++;
                        gunCharacteristics.originBulletAmountCharacteristic++;
                        break;
                    case 3:
                        gunCharacteristics.dashRecoverCharacteristic++;
                        gunCharacteristics.originDashRecoverCharacteristic++;
                        break;
                    case 4:
                        gunCharacteristics.penetrateCharacteristic++;
                        gunCharacteristics.originPenetrateCharacteristic++;
                        break;
                }
                //this.uiPopupCharacteristicSelect.Init(idx);
                this.UnShow();
            });
        }
    }

    public void ActiveRandomCharacteristics(int numToActivate)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < this.characteristicsGo.Length; i++)
        {
            //해당 특성이 10이상일 때 활성화 리스트에 추가하지 않습니다.(인덱스 번호를 맞춰야 함)
            if (this.characteristics[i] >= 10) continue;
            list.Add(i);
        }

        //특성이 3개 이상 10을 달성했을 때 2개 이하로 출력하기 위함        
        int cnt = this.characteristicsGo.Length;
        if (list.Count < this.characteristicsGo.Length) cnt = list.Count;

        // Fisher-Yates 알고리즘
        for (int i = 0; i < cnt; i++)
        {
            int idx = Random.Range(i, cnt);
            int temp = list[idx];
            list[idx] = list[i];
            list[i] = temp;
        }

        cnt = 3;
        if (list.Count < numToActivate) cnt = list.Count;

        //현재 총기 특성 수치 표시
        for (int i = 0; i < cnt; i++)
        {
            this.characteristicsGo[list[i]].SetActive(true);
            this.characteristicsGo[list[i]].transform.Find("txtCurrentCharacteristic").GetComponent<Text>().text
                = this.characteristics[list[i]].ToString();
            this.characteristicsGo[list[i]].transform.Find("txtIncreasedCharacteristic").GetComponent<Text>().text
                = (this.characteristics[list[i]] + 1).ToString();
        }
    }
}
