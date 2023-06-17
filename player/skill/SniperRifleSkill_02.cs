using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifleSkill_02 : PlayerActiveSkill
{
    private bool isSmash;
    public GameObject leftTop;
    public GameObject rightBottom;

    public GameObject dirFront;
    public GameObject dirBackd;

    public override void Init(GunShell gunShell)
    {
        this.existTime = 0.680f;
        this.transform.parent = this.transBuffPoint;
        this.knockbackSpeed = 50000;
        base.Init(gunShell);
        this.damage = this.damage = this.damage = InfoManager.instance.statInfo.BattleRate * 5;
    }
    public void Update()
    {        
        if (this.isSmash)
        {
            Collider2D[] colliders = Physics2D.OverlapAreaAll(this.leftTop.transform.position, this.rightBottom.transform.position);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Monster"))
                {
                    Damageable monster = collider.GetComponent<Damageable>();

                    if (monster != null && monster.GetComponent<Rigidbody2D>() != null)
                    {
                        if (!monster.isSkilldDamaged)
                        {
                            Vector2 dir = this.dirFront.transform.position - this.dirBackd.transform.position;
                            if (monster.hp > 0) monster.GetComponent<PathFinding>()?.HitPlayerActiveSkill(dir, this.knockbackSpeed);
                            monster.TakeSkillDamage(this.damage, this.existTime);
                        }
                    }
                }
            }
        }
    }

    public void Smashed()
    {
        StartCoroutine(this.SmashRoutine());
    }

    IEnumerator SmashRoutine()
    {
        this.isSmash = true;
        yield return new WaitForSeconds(0.15f);
        this.isSmash = false;
    }
}
