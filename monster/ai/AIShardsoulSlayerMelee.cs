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

    // trigger 오브젝트를 활용하여 범위 공격을 구현합니다.
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
