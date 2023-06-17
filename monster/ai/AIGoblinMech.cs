using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGoblinMech : AIDashMelee
{
    [SerializeField]
    private GameObject attackTriggerGO;

    public override void Init(Transform player)
    {
        base.Init(player);
        this.attackTriggerGO.GetComponent<Collider2D>().enabled = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // trigger�� ���Ե� ������Ʈ�� Ȱ���Ͽ� ���ݹ����� �����մϴ�.
    protected override void DashAttack()
    {
        base.DashAttack();
        this.attackTriggerGO.transform.position = this.transform.position;
        this.attackTriggerGO.GetComponent<Collider2D>().enabled = true;
    }

    private void EndSmash()
    {
        this.attackTriggerGO.GetComponent<Collider2D>().enabled = false;
    }
}
