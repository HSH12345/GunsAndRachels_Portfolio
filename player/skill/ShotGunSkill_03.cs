using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunSkill_03 : PlayerActiveSkill
{
    public GameObject leftTop;
    public GameObject rightBottom;

    public override void Init(GunShell gunShell)
    {
        this.existTime = 0.950f;
        this.transform.parent = this.transPlayer;
        this.transform.localPosition = Vector2.zero;
        this.knockbackSpeed = 50000;
        base.Init(gunShell);
        this.damage = this.damage = InfoManager.instance.statInfo.BattleRate * 15;
    }

    public void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(this.leftTop.transform.position, this.rightBottom.transform.position);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                Damageable monster = collider.GetComponent<Damageable>();

                if (monster != null && monster.GetComponent<Rigidbody2D>() != null)
                {
                    Vector2 dir = collider.transform.position - this.transform.position;
                    if (monster.hp > 0) monster.GetComponent<PathFinding>()?.HitPlayerActiveSkill(dir, this.knockbackSpeed);
                }

                if (!monster.isSkilldDamaged)
                {
                    monster.TakeSkillDamage(this.damage, this.existTime / 2);
                }
            }

            if (collider.CompareTag("MonsterBullet"))
            {
                collider.gameObject.SetActive(false);
            }
        }
    }
}
