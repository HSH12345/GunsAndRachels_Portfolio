using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShardsoulSlayerMelee : AINormalMelee
{
    [SerializeField]
    private GameObject attackTriggerGO;

    public override void Init(Transform player)
    {
        this.attackTriggerGO.GetComponent<Collider2D>().enabled = false;
        base.Init(player);        
    }

    // trigger ������Ʈ�� Ȱ���Ͽ� ���� ������ �����մϴ�.
    public void StartSmash()
    {
        this.attackTriggerGO.transform.position = this.transform.position;
        this.attackTriggerGO.GetComponent<Collider2D>().enabled = true;
    }

    public void EndSmash()
    {
        this.attackTriggerGO.GetComponent<Collider2D>().enabled = false;
    }
}
