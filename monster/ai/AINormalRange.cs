using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINormalRange : PathFinding
{
    public float attackMoveSpeed;    
    protected GameObject scanGo;
    public Transform bulletPoint;
    public GameObject bulletGo;
    protected float attackTime;
    public float maxAttacktime = 3f;
    protected bool isRangeAttacking;

    protected Vector2 dirPlayer;
    protected bool isTargeting;

    public override void Init(Transform player)
    {
        base.Init(player);
        this.isTargeting = false;
        this.attackTime = 0;
        this.SetMove();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();


        this.dirPlayer = (this.target.transform.position - this.bulletPoint.transform.position).normalized;

        if (!this.isTargeting)
        {
            this.SetMove();

            // Ray가 player에 hit하고 player와의 거리가 사거리 안에 들어왔을 때
            if (this.scanGo != null && this.scanGo.tag == "Player" && this.dist <= this.attackRange)
            {
                this.isTargeting = true;
            }
        }

        if(this.isTargeting) 
        {
            this.SetIdle();
            this.attackTime += Time.deltaTime;
            if (this.attackTime >= this.maxAttacktime)
            {
                this.RangeAttack();
            }
        } 
        
        this.RayCast();
    }

    protected virtual void RangeAttack()
    {
        this.anim.SetInteger("State", 1);
    }

    protected virtual void SetMove()
    {
        this.anim.SetInteger("State", 0);        
        this.moveSpeed = this.normalMoveSpeed;
        this.isStopForAttack = false;
    }

    protected virtual void SetIdle()
    {
        this.noKnockbackAnim = true;
        this.anim.SetInteger("State", 3);
        if(!this.isKnockBack) this.rBody2D.velocity = Vector2.zero;
        this.isStopForAttack = true;
    }

    protected void RayCast()
    {
        Debug.DrawRay(this.bulletPoint.transform.position, this.dirPlayer * 5f, new Color(0, 1, 0));
        var layerMask = ~LayerMask.GetMask("Monster");
        RaycastHit2D rayHit = Physics2D.Raycast(this.bulletPoint.transform.position, this.dirPlayer, 5.0f, layerMask);

        if (rayHit.collider != null)
        {
            this.scanGo = rayHit.collider.gameObject;
        }
        else this.scanGo = null;
    }
    //애니메이션 이벤트에서 사용합니다.
    protected virtual void InstantiateBullet()
    {
        var go = Instantiate(this.bulletGo);
        go.transform.position = this.bulletPoint.transform.position;
        go.SetActive(true);
        var shootDir = (this.target.transform.position - go.transform.position).normalized;
    }

    protected virtual void StopInstantateBullet()
    {
        this.attackTime = 0;
        this.anim.SetInteger("State", 0);
        this.isTargeting = false;
        this.noKnockbackAnim = false;
        this.isRangeAttacking = false;
    }
}
