using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINormalExplosion : PathFinding
{
    public float attackMoveSpeed;
    private GameObject scanGo;
    private Vector2 destination;
    private bool isAttacking;
    protected bool isExplosion;
    private float destinationDist;
    private float explosionTime;
    private float maxExplosionTime = 1.5f;

    public override void Init(Transform player)
    {
        this.destination = Vector2.zero;
        base.Init(player);
        this.noKnockbackAnim = true;
        this.isAttacking = false;
        this.isExplosion = false;
        this.transform.localScale = new Vector3(1f, 1f, 1f);
        this.explosionTime = 0;
        this.isIgnoreMonsterCollision = false;
    }

    protected override void FixedUpdate()
    {
        if (this.dist < 5.0f && this.dist != 0)
        {
            this.RayCast();
        }

        if (this.isAttacking) this.Dash();
        else base.FixedUpdate();
    }

    private void Dash()
    {
        this.destinationDist = Vector2.Distance(this.destination, this.transform.position);
        this.explosionTime += Time.deltaTime;
        if (destinationDist <= 0.1f || this.explosionTime >= this.maxExplosionTime)
        {
            if (!this.isExplosion)
            {
                this.Explosion();
                this.isExplosion = true;                
            }    
        }
        else 
        {
            if(!this.isExplosion)
            {
                this.isIgnoreMonsterCollision = true;
                var attackDir = this.destination - (Vector2)this.transform.position;
                this.anim.SetInteger("State", 1);
                this.moveSpeed = this.attackMoveSpeed;
                this.rBody2D.velocity = attackDir.normalized * this.moveSpeed * Time.deltaTime;
            }
        }

        StartCoroutine(StopRoutine());
    }

    protected virtual void Explosion()
    {
        this.GetComponent<Monster>().isInvincibility = true;
        this.rBody2D.velocity = Vector2.zero;
        this.isStopForAttack = true;
        this.noKnockback = true;
        this.transform.localScale = new Vector3(2f, 2f, 2f);
        this.anim.SetInteger("State", 2);

        // 폭발 후 collider를 실제 오브젝트가 제거되는 시간보다 빠르게 하여 플레이어와 부자연스러운 접촉을 제어합니다.
        Invoke("DisableCollider", this.GetComponent<Monster>().deadAnimationClip.length / 2);
        Invoke("DestroyGo", this.GetComponent<Monster>().deadAnimationClip.length);
    }

    public IEnumerator StopRoutine()
    {
        yield return new WaitForSeconds(2.0f);        
        this.Explosion();
    }

    public void DestroyGo()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableCollider()
    {
        this.GetComponent<Collider2D>().enabled = false;
    }

    private void RayCast()
    {
        Debug.DrawRay(this.rBody2D.position, this.dir * 7f, new Color(0, 1, 0));
        var layerMask = ~LayerMask.GetMask("Monster");
        RaycastHit2D rayHit = Physics2D.Raycast(this.rBody2D.position, this.dir, 7f, layerMask);

        // 레이와 충돌하면 상태를 변화시킵니다.
        if (rayHit.collider != null)
        {
            this.scanGo = rayHit.collider.gameObject;
            if (this.scanGo.gameObject.tag == "Player" && !isAttacking)
            {
                this.destination = this.target.transform.position;
                this.isAttacking = true;
            }
        }
        else this.scanGo = null;
    }
}
