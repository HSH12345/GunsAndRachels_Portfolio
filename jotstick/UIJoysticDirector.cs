using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIJoysticDirector : MonoBehaviour
{
    public JoystickMove joystickMove;
    public JoystickGunDir joystickGunDir;
    public JoystickSkillDir[] joystickSkillDir;
    public Button[] btnSkills;
    public Button btnDash;
    public GameObject[] UICoolTimeSkills;
    public GameObject UICoolTimeDash;

    public void Init()
    {       
        //�÷��̾ ��Ż�� ���� �� �Ʒ� �̺�Ʈ�� Dispatch�մϴ�.
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.UIJoystickDirectorStopJoyStick, this.StopJoystick);
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.UIJoystickDirectorActiveJoyStick, this.ActiveJoystick);
    }

    public void StopJoystick()
    {
        this.SetJoystickState(false);
    }

    public void ActiveJoystick()
    {
        this.SetJoystickState(true);
    }

    //�÷��̾ ��Ż�� ���� �� ���̽�ƽ ����� �����մϴ�.
    private void SetJoystickState(bool state)
    {
        var canvasGroup = this.joystickMove.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = state;

        canvasGroup = this.joystickGunDir.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = state;

        foreach (var joystick in this.joystickSkillDir)
        {
            joystick.playerOnPortal = !state;
            canvasGroup = joystick.gameObject.GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = state;
        }

        foreach (var btn in this.btnSkills)
        {
            btn.interactable = state;
        }

        this.btnDash.interactable = state;
    }
}
