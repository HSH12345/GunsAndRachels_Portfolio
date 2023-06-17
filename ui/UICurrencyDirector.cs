using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UICurrencyDirector : MonoBehaviour
{
    public Text txtGold;
    public Text txtEther;
    private PossessionAmountInfo possessionInfo;

    [SerializeField] private CoinSpinner coinSpinner;

    public void Init()
    {
        this.possessionInfo = InfoManager.instance.possessionAmountInfo;
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.UICurrencyDirectorUpdateGoldUI, this.UpdateGoldUI);
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.UICurrencyDirectorUpdateEtherUI, this.UpdateEtherUI);

        this.UpdateEtherUI();
        this.UpdateGoldUI();
        this.coinSpinner.Init();
    }

    // ���� Sanctuary�� ���� ���� ��带 �ҷ����� ���������� 0���� �����մϴ�.
    public void UpdateGoldUI()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        this.txtGold.text = (sceneName == "SanctuaryScene") ? 
            this.possessionInfo.goldAmount.ToString() : this.possessionInfo.dungeonGoldAmount.ToString();
    }

    public void UpdateEtherUI()
    {
        this.txtEther.text = this.possessionInfo.etherAmount.ToString();
    }
}
