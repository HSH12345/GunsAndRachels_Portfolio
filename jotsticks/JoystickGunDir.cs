using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickGunDir : Joystick
{
    //조이스틱이 드래그 된 상태를 반환합니다.
    public bool IsShoot { get { return this.IsDraged; } }
    //건샷이 끝났을 때 사운드를 재생하기 위한 action
    public System.Action lastGunShot;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        this.lastGunShot();
    }
}
