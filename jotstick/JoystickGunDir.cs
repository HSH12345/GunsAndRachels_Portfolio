using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickGunDir : Joystick
{
    //���̽�ƽ�� �巡�� �� ���¸� ��ȯ�մϴ�.
    public bool IsShoot { get { return this.IsDraged; } }
    //�Ǽ��� ������ �� ���带 ����ϱ� ���� action
    public System.Action lastGunShot;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        this.lastGunShot();
    }
}
