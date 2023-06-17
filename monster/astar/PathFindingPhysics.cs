using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PathFinding : MonoBehaviour
{
    //³Ë¹é °ü·Ã
    protected bool isKnockBack;
    private float knockBackSpeed;
    private Vector2 knockbackDir;
    [System.NonSerialized]
    public float stun = 1;
    private Coroutine knockBackRoutine;
    private float dashAttackKnockBackSpeed = 10000;

    protected bool noKnockback;
    protected bool noKnockbackAnim;

    private bool isFindingPath;
    protected bool isIgnoreMonsterCollision;

    //³Ë¹é È¿°ú
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var bullet = collision.collider.GetComponent<Bullet>();
        var skill = collision.collider.GetComponent<PlayerActiveSkill>();

        if (collision.collider.CompareTag("PlayerBullet"))
        {
            if (bullet != null)
            {
                //ÅºÈ¯ÀÇ ÁøÇà ¹æÇâÀ¸·Î ³Ë¹é
                this.knockbackDir = bullet.dir;
                this.knockBackSpeed = bullet.knockbackSpeed;
                if (!this.noKnockback) this.knockBackRoutine = this.StartCoroutine(this.KnockbackRoutine());           
                this.monster.TakeBulletDamage(bullet.damage);
            }
        }

        if (collision.collider.CompareTag("Monster"))
        {
            var monster = collision.collider.GetComponent<Monster>();
            if(monster != null) Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), this.isIgnoreMonsterCollision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDash"))
        {            
            var dashAttack = collision.transform.parent.gameObject.GetComponent<DashAttack>();
            this.knockbackDir = -dashAttack.dir;
            this.knockBackSpeed = this.dashAttackKnockBackSpeed;
            if (!this.noKnockback) this.knockBackRoutine = this.StartCoroutine(this.KnockbackRoutine());
            this.monster.TakeBulletDamage(dashAttack.damage);
        }
    }

    public void HitPlayerActiveSkill(Vector2 knockbackDir, float knockbackSpeed)
    {
        this.knockbackDir = knockbackDir;
        if (!this.noKnockback) this.knockBackRoutine = this.StartCoroutine(this.KnockbackRoutine());
        this.knockBackSpeed = knockbackSpeed;
    }

    private IEnumerator KnockbackRoutine()
    {
        this.isKnockBack = true;
        this.stun = 0;
        if (!this.noKnockbackAnim)
        {
            this.anim.SetInteger("State", -1);
            this.anim.Play("Hit", -1, 0);
        }
        yield return new WaitForSeconds(0.15f);
        if (!this.noKnockbackAnim) this.anim.SetInteger("State", 0);
        this.isKnockBack = false;
        this.rBody2D.velocity = Vector2.zero;
        this.stun = 1;
    }

    public void StopKnockback()
    {
        if(this.knockBackRoutine != null) StopCoroutine(this.knockBackRoutine);
    }

    private void KnockBack(float knockbackSpeed)
    {
        this.rBody2D.AddForce(this.knockbackDir.normalized * this.knockBackSpeed * Time.deltaTime);
    }
}
