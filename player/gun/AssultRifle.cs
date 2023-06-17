using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssultRifle : GunState
{

    public override void Init()
    {
        base.Init();
        this.gunForm.gunType = "AssultRifle";
        this.gunForm.attackpower = (double)InfoManager.instance.statInfo.powerStat / 
            ((double)InfoManager.instance.statInfo.powerStat + 100d) * 600d;
        this.gunForm.attackDelayTime = 1 / (0.05f * InfoManager.instance.statInfo.fireRateStat + 0.5f);
        this.gunForm.spriteRenderer = this.spriteRenderer;
        this.gunForm.anim = this.anim;
        this.gunForm.bulletExistTime = 0.7f;
        this.gunForm.bulletSpeed = 800f;

        for (int i = 0; i < 3; i++)
        {
            this.gunForm.skills[i] = this.skills[i];
        }
        for(int i = 0; i < 4; i++)
        {
            this.gunForm.bullets[i] = this.bullets[i];
        }

        //line = 0, arc = 1, circle = 2, Button = 3
        this.gunForm.skillTypes[0] = 0;
        this.gunForm.skillTypes[1] = 3;
        this.gunForm.skillTypes[2] = 1;

        this.gunForm.skillIconNames[0] = "IconAssultRifleSkill_01";
        this.gunForm.skillIconNames[1] = "IconAssultRifleSkill_02";
        this.gunForm.skillIconNames[2] = "IconAssultRifleSkill_03";

        this.gunForm.skillCoolTime[0] = 0.5f;
        this.gunForm.skillCoolTime[1] = 10f;
        this.gunForm.skillCoolTime[2] = 5f;
    }
}
