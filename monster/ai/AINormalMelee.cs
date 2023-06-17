using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINormalMelee : PathFinding
{
    public float attackMoveSpeed;
    private GameObject scanGo;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!this.isKnockBack)
        {
            if (this.dist < 2.0f) this.MeleeAttack();
            else this.SetMove();
        }

        this.RayCast();
    }

    private void MeleeAttack()
    {
        this.anim.SetInteger("State", 1);
        this.moveSpeed = this.attackMoveSpeed;
    }

    private void SetMove()
    {
        this.anim.SetInteger("State", 0);
        this.moveSpeed = this.normalMoveSpeed;
    }

    private void RayCast()
    {
        Debug.DrawRay(this.rBody2D.position, this.dir * 1.5f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(this.rBody2D.position, this.dir, 1.5f);

        if (rayHit.collider != null)
        {
            this.scanGo = rayHit.collider.gameObject;
        }
        else this.scanGo = null;
    }
}
