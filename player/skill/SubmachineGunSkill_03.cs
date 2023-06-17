using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmachineGunSkill_03 : PlayerActiveSkill
{
    public override void Init(GunShell gunShell)
    {
        this.isCircle = true;
        this.existTime = 0.783f;
        if (this.transPlayer.position.x - this.transform.position.x < 0)
            this.GetComponent<SpriteRenderer>().flipX = true;
        this.knockbackSpeed = 50000;
        base.Init(gunShell);

        this.damage = InfoManager.instance.statInfo.BattleRate * 30;
    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(this.transform.position + new Vector3(1, -0.5f), new Vector2(6, 3), 0);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                Damageable monster = collider.GetComponent<Damageable>();

                if (monster != null && monster.GetComponent<Rigidbody2D>() != null)
                {
                    if (!monster.isSkilldDamaged)
                    {
                        Vector2 dir = collider.transform.position - this.transform.position;
                        if (monster.hp > 0) monster.GetComponent<PathFinding>()?.HitPlayerActiveSkill(dir, this.knockbackSpeed);
                        monster.TakeSkillDamage(this.damage, this.existTime);
                    }
                }
            }

            if (collider.CompareTag("MonsterBullet"))
            {
                collider.gameObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(this.transform.position + new Vector3(1, -0.5f), new Vector2(6, 3));
    }
}