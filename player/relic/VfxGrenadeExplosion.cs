using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxGrenadeExplosion : PlayerActiveSkill
{
    public GameObject leftTop;
    public GameObject rightBottom;

    private HashSet<Damageable> damagedMonsters;

    public override void Init(GunShell gunShell)
    {
        this.existTime = 0.85f;
        this.knockbackSpeed = 30000;
        this.damage = InfoManager.instance.statInfo.fireRateStat * 10;
        this.damagedMonsters = new HashSet<Damageable>();

        base.Init(gunShell);
    }

    public void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(this.leftTop.transform.position, this.rightBottom.transform.position);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                Damageable monster = collider.GetComponent<Damageable>();

                if (!damagedMonsters.Contains(monster))
                {
                    if(monster != null)
                    {
                        this.damagedMonsters.Add(monster);
                        monster.TakeSkillDamage(this.damage, this.existTime);

                        if (monster != null && monster.GetComponent<Rigidbody2D>() != null)
                        {
                            Vector2 dir = monster.transform.position - this.transform.position;
                            if (monster.hp > 0) monster.GetComponent<PathFinding>()?.HitPlayerActiveSkill(dir, this.knockbackSpeed);
                        }
                    }
                 
                }
            }
        }
    }
}
