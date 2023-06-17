using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVfx
{
    public void Init();
    public void OnDisable();
    public void Inactivate();
}

public class bulletVfx : MonoBehaviour, IVfx
{
    private PlayerObjectPooler obejctPooler;

    public void Init()
    {
        this.obejctPooler = PlayerObjectPooler.instance;
        this.gameObject.SetActive(true);
        Invoke("Inactivate", 0.16f);
    }

    public void OnDisable()
    {
        this.obejctPooler.bulletVfxPool.Enqueue(this.gameObject);
    }

    public void Inactivate()
    {
        this.gameObject.SetActive(false);
    }
}
