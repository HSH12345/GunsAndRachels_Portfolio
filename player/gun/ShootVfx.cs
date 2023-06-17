using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootVfx : MonoBehaviour, IVfx
{
    public void Init()
    {
        this.gameObject.SetActive(true);
        Invoke("Inactivate", 0.16f);
    }

    public void OnDisable()
    {
        PlayerObjectPooler.instance.shootVfxPool.Enqueue(this.gameObject);
    }

    public void Inactivate()
    {
        this.gameObject.SetActive(false);
    }
}
