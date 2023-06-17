using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDashMelee : PathFinding
{
    public float attackMoveSpeed;
    private GameObject scanGo;
    private Vector2 attackTarget;
    private Vector2 attackDir;
    private bool isLockOn;
    private bool isAttacking;
    private Vector2 dirPlayer;

    public override void Init(Transform player)
    {
        base.Init(player);
        this.isLockOn = false;
        this.isAttacking = false;
        this.isIgnoreMonsterCollision = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (this.dist < 4.0f && this.dist != 0)
        {
            this.RayCast();

            if (this.scanGo != null && this.scanGo.tag == "Player" && this.dist <= this.attackRange)
            {                
                if (!this.isLockOn) 
                {
                    this.isLockOn = true;
                    this.noKnockbackAnim = true;
                    this.moveSpeed = 0;
                    this.rBody2D.velocity = Vector2.zero;
                    StartCoroutine(LockOnRoutine());
                    this.anim.SetInteger("State", 1);
                    this.isStopForAttack = true;
                }
            }
        }

        if (this.isAttacking)
        {            
            this.moveSpeed = this.normalMoveSpeed;
            this.rBody2D.velocity = this.attackDir.normalized * this.attackMoveSpeed * Time.deltaTime;
            StartCoroutine(this.SetStateRoutine());
        }
    }

    protected virtual void DashAttack()
    {
        this.isAttacking = true;
        this.isIgnoreMonsterCollision = true;
    } 

    IEnumerator LockOnRoutine()
    {
        yield return new WaitForSeconds(0.8f);
        this.attackTarget = this.target.transform.position;
        this.attackDir = this.attackTarget - (Vector2)this.transform.position;
    }

    IEnumerator SetStateRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        this.noKnockbackAnim = false;
        this.moveSpeed = this.normalMoveSpeed;
        this.attackTarget = Vector2.zero;
        this.anim.SetInteger("State", 0);
        this.moveSpeed = this.normalMoveSpeed;
        this.isAttacking = false;
        this.isLockOn = false;
        this.isIgnoreMonsterCollision = false;
        this.isStopForAttack = false;
    }

    private void RayCast()
    {
        this.dirPlayer = (this.target.transform.position - this.transform.position).normalized;
        var layerMask = ~LayerMask.GetMask("Monster");
        Debug.DrawRay(this.rBody2D.position, this.dirPlayer * 5.0f, new Color(0, 1, 0), layerMask);
        RaycastHit2D rayHit = Physics2D.Raycast(this.rBody2D.position, this.dirPlayer, 5.0f, layerMask);

        if (rayHit.collider != null)
        {
            this.scanGo = rayHit.collider.gameObject;
        }
        else this.scanGo = null;
    }
}
