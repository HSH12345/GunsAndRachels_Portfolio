using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBullet : Relic
{
    // 6 : PlayerBullet, 9 : MonsterBullet
    public override void Init()
    {
        base.Init();
        if(this.gameObject.activeSelf) Physics2D.IgnoreLayerCollision(6, 9, false);
        else Physics2D.IgnoreLayerCollision(6, 9, true); 
    }
}
