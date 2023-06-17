using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : Relic
{
    public GameObject dashAttackGO;
    [System.NonSerialized]
    public Vector2 dir;
    [System.NonSerialized]
    public int damage = InfoManager.instance.statInfo.powerStat * 15;
    private bool isStart;

    public override void Init()
    {
        base.Init();
    }

    void FixedUpdate()
    {
        // ��� ��� �� Tigger�� �۵��ϴ� ������Ʈ�� Ȱ��ȭ �Ͽ� ���� �����մϴ�.
        this.dashAttackGO.transform.localPosition = Vector3.zero;
        if (this.playerShell != null && this.playerShell.isDash)
        {
            this.dashAttackGO.SetActive(true);
            if (!this.isStart)
            {
                this.isStart = true;
                this.dir = this.playerShell.tmpDir;
                if (this.dir == Vector2.zero) this.dir = Vector2.right;
            } 
        }
        else 
        {
            this.dashAttackGO.SetActive(false);
            this.isStart = false;
        } 
    }
}
