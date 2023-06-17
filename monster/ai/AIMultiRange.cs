using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMultiRange : PathFinding
{
    public enum MonsterName
    {
        none = -1, RatfolkMage, Golem, Witch
    }
    [SerializeField]
    private MonsterName eMonsterName;

    public float attackMoveSpeed;
    protected GameObject scanGo;
    public Transform bulletPoint;
    public GameObject bulletGo;

    protected Vector2 dirPlayer;
    protected bool isTargeting;

    private int bulletAmount;
    private bool isMultiRangedAttack;

    public override void Init(Transform player)
    {
        base.Init(player);
        this.isTargeting = false;
        this.SetMove();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!this.isTargeting)
        {
            this.SetMove();

            if (this.scanGo != null && this.scanGo.tag == "Player" && this.dist < this.attackRange)
            {
                this.isTargeting = true;
            }
        }
        else
        {
            if (this.isTargeting)
            {
                this.rBody2D.velocity = Vector2.zero;
                this.isStopForAttack = true;
                this.RangeAttack();
            }
        }

        this.RayCast();
        if (this.isBlocked) this.SetMove();
    }

    private void RangeAttack()
    {
        this.noKnockbackAnim = true;
        this.anim.SetInteger("State", 1);
    }

    private void SetMove()
    {
        this.anim.SetInteger("State", 0);
        this.isStopForAttack = false;
        this.moveSpeed = this.normalMoveSpeed;
    }

    private void RayCast()
    {
        this.dirPlayer = (this.target.transform.position - this.bulletPoint.transform.position).normalized;
        Debug.DrawRay(this.rBody2D.position, this.dirPlayer * 5f, new Color(0, 1, 0));
        var layerMask = ~LayerMask.GetMask("Monster");
        RaycastHit2D rayHit = Physics2D.Raycast(this.bulletPoint.transform.position, this.dirPlayer, 5f, layerMask); //, LayerMask.GetMask("Obstacles")

        if (rayHit.collider != null)
        {
            this.scanGo = rayHit.collider.gameObject;
        }
        else this.scanGo = null;
    }
    //애니메이션 clip에서 event로 제어합니다.
    public void InstantiateBullet()
    {
        MonsterBulletPooler bulletPooler = MonsterBulletPooler.instance;
        Queue<GameObject> pool = null;
        int bulletAmount = 0;
        float rotationZ = 15f;

        switch (this.eMonsterName)
        {
            case MonsterName.RatfolkMage:
                pool = bulletPooler.ratfolkMageBulletPool;
                bulletAmount = 3;
                rotationZ = 20;
                break;
            case MonsterName.Golem:
                pool = bulletPooler.golemBulletPool;
                bulletAmount = 3;
                rotationZ = 20;
                break;
            case MonsterName.Witch:
                pool = bulletPooler.witchBulletPool;
                bulletAmount = 5;
                rotationZ = 40;
                break;
        }

        this.isTargeting = false;
        this.moveSpeed = this.normalMoveSpeed;
        this.isStopForAttack = false;
        this.noKnockbackAnim = false;
        this.isMultiRangedAttack = false;

        if (pool != null)
        {
            InstantiateBulletForMonster(pool, bulletAmount, rotationZ);
        }
    }

    private void InstantiateBulletForMonster(Queue<GameObject> pool, int bulletAmount, float rotationZ)
    {
        GameObject go;
        float angle = Mathf.Atan2(this.dirPlayer.y, this.dirPlayer.x) * Mathf.Rad2Deg;
        var lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        float baseRotationZ = rotationZ;

        for (int i = 0; i < bulletAmount; i++)
        {
            var baseRoation = Quaternion.Euler(0, 0, baseRotationZ);

            if (pool.Count > 0)
            {
                go = pool.Dequeue();
                if (go.activeSelf) go = Instantiate(this.bulletGo);
            }
            else
            {
                go = Instantiate(this.bulletGo);
            }

            go.transform.position = this.bulletPoint.transform.position;
            go.SetActive(true);

            go.transform.rotation = lookRotation * baseRoation;
            baseRotationZ -= 20;
            go.GetComponent<MonsterBullet>().Init(pool);
        }
    }
}

