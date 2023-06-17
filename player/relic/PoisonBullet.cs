using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : Relic
{
    public GameObject vfxPoison;
    [System.NonSerialized]
    public Queue<GameObject> vfxPoisonPool;
    private bool isCreated;

    public override void Init()
    {
        base.Init();
        if(!this.isCreated)this.vfxPoisonPool = PlayerObjectPooler.instance.vfxPoisonPool;
        this.isCreated = true;
    }

    //Monster 스크립트에서 구현
    public bool IsPoison()
    {
        return true;
    }    

    public void ShowVfxPoison(Transform monsterTrans)
    {
        if(monsterTrans != null)
        {
            GameObject go;

            if (vfxPoisonPool.Count > 0)
            {
                go = vfxPoisonPool.Dequeue();
                if (go == null) go = Instantiate(this.vfxPoison);
                go.SetActive(true);
                go.transform.SetParent(monsterTrans);
            }
            else
            {
                go = Instantiate(this.vfxPoison, monsterTrans);
            }

            go.GetComponent<VfxPoison>().Init();
        }
    }
}
