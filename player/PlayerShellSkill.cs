using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerShell : MonoBehaviour
{
    //��ų ���� ��� 0 : Line, 1 : Arc, 2 : Circle, 3 : Button
    public void SetSkill(int index)
    {
        this.eSkillType = this.gunShell.eSkillTypes[index];

        if ((int)this.eSkillType != 3) this.indicators[(int)this.eSkillType].gameObject.SetActive(true);
        //circle ��� �̹Ƿ� bg �ʿ��մϴ�.
        if ((int)this.eSkillType == 2) this.bg.SetActive(true);

        // line, arc ����� joystick�� ���������� �����մϴ�.
        if ((int)this.eSkillType < 2)
        {
            var angle = Mathf.Atan2(this.joystickSkillDir[index].Vertical, this.joystickSkillDir[index].Horizontal) * Mathf.Rad2Deg;
            var lookRotation = Quaternion.Euler(angle * Vector3.forward);
            this.indicators[(int)this.eSkillType].transform.rotation = lookRotation;

            this.skillPoint = this.gunShell.bulletPoint.transform.position;
        }
        // circle ����� joystick handle�� localPos�� indicator�� localPos�� ����Ͽ� �����Դϴ�.
        else if ((int)this.eSkillType == 2)
        {
            this.indicators[(int)this.eSkillType].transform.localPosition = this.joystickSkillDir[index].HandlePos / 27;
            this.skillPoint = this.indicators[(int)this.eSkillType].transform.position;
        }
        // button ����� ui�� Ȱ��ȭ�ϰ� handle�� ��ġ�� �����մϴ�.
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
