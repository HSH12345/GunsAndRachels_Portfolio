using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssultRifleSkill_03 : PlayerActiveSkill
{
    public GameObject leftTop;
    public GameObject rightBottom;

    public GameObject dirFront;
    public GameObject dirBack;

    private HashSet<Damageable>[] damagedMonsters;
    private int attackCnt;

    public override void Init(GunShell gunShell)
    {
        this.attackCnt = 0;
        this.existTime = 0.652f;
        this.transform.parent = this.transBulletPoint;
        this.knockbackSpeed = 20000;

        this.damagedMonsters = new HashSet<Damageable>[3];
        for (int i = 0; i < this.damagedMonsters.Length; i++)
        {
            this.damagedMonsters[i] = new HashSet<Damageable>();
        }

        base.Init(gunShell);
        this.damage = InfoManager.instance.statInfo.BattleRate * 10;
    }


    public void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(this.leftTop.transform.position, this.rightBottom.transform.position);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                Damageable monster = collider.GetComponent<Damageable>();

                //스킬 사용중 이미 피격된 몬스터를 HashSet에 저장하여 연속으로 타격하지 않습니다.
                if (!damagedMonsters[this.attackCnt].Contains(monster))
                {
                    this.damagedMonsters[this.attackCnt].Add(monster);
                    monster.TakeSkillDamage(this.damage, 0);

                    if (monster != null && monster.GetComponent<Rigidbody2D>() != null)
                    {
                        Vector2 dir = this.dirFront.transform.position - this.dirBack.transform.position;
                        if (this.attackCnt == 2)
                        {
                            this.knockbackSpeed = 60000;
                            this.damage *= 2;
                        }
                        if (monster.hp > 0) monster.GetComponent<PathFinding>()?.HitPlayerActiveSkill(dir, this.knockbackSpeed);
                    }
                }
            }

            if (collider.CompareTag("MonsterBullet"))
            {
                collider.gameObject.SetActive(false);
            }
        }
    }

    public void AttackCounting()
    {
        this.attackCnt++;
    }
}
