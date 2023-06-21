using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifleSkill_03 : PlayerActiveSkill
{
    private bool isSmash;

    public override void Init(GunShell gunShell)
    {
        this.isCircle = true;
        this.existTime = 0.550f;
        if (this.transPlayer.position.x - this.transform.position.x < 0)
            this.GetComponent<SpriteRenderer>().flipX = true;
        this.knockbackSpeed = 80000;
        base.Init(gunShell);
        this.damage = InfoManager.instance.statInfo.BattleRate * 33;
    }

    public void Update()
    {
        if (this.isSmash)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 2.5f);

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 2.5f);
    }

    public void Smashed()
    {
        StartCoroutine(this.SmashRoutine());
    }

    IEnumerator SmashRoutine()
    {
        this.isSmash = true;
        yield return new WaitForSeconds(0.25f);
        this.isSmash = false;
    }
}
