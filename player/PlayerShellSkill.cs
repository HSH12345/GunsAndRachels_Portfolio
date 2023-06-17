using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerShell : MonoBehaviour
{
    //스킬 동작 방식 0 : Line, 1 : Arc, 2 : Circle, 3 : Button
    public void SetSkill(int index)
    {
        this.eSkillType = this.gunShell.eSkillTypes[index];

        if ((int)this.eSkillType != 3) this.indicators[(int)this.eSkillType].gameObject.SetActive(true);
        //circle 방식 이므로 bg 필요합니다.
        if ((int)this.eSkillType == 2) this.bg.SetActive(true);

        // line, arc 방식은 joystick의 각도값으로 조작합니다.
        if ((int)this.eSkillType < 2)
        {
            var angle = Mathf.Atan2(this.joystickSkillDir[index].Vertical, this.joystickSkillDir[index].Horizontal) * Mathf.Rad2Deg;
            var lookRotation = Quaternion.Euler(angle * Vector3.forward);
            this.indicators[(int)this.eSkillType].transform.rotation = lookRotation;

            this.skillPoint = this.gunShell.bulletPoint.transform.position;
        }
        // circle 방식은 joystick handle의 localPos를 indicator의 localPos로 사용하여 움직입니다.
        else if ((int)this.eSkillType == 2)
        {
            this.indicators[(int)this.eSkillType].transform.localPosition = this.joystickSkillDir[index].HandlePos / 27;
            this.skillPoint = this.indicators[(int)this.eSkillType].transform.position;
        }
        // button 방식은 ui를 활성화하고 handle의 위치를 고정합니다.
        else if ((int)this.eSkillType == 3)
        {
            this.joystickSkillDir[index].isButton = true;
            if (index == 1)
                this.joystickSkillDir[index].handle.localPosition = Vector2.zero;
        }
    }

    public void SetFirstSkill()
    {
        SetSkill(0);
    }

    public void SetSecondSkill()
    {
        SetSkill(1);
    }

    public void SetThirdSkill()
    {
        SetSkill(2);
    }
}
