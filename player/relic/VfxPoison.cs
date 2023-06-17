using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxPoison : MonoBehaviour, IVfx
{
    private PoisonBullet poisonBullet;

    public void Init()
    {
        this.poisonBullet = GameObject.FindObjectOfType<PoisonBullet>().GetComponent<PoisonBullet>();
        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(true);
        this.transform.localScale = Vector2.one;
        Invoke("Inactivate", 0.8f);
    }

    public void OnDisable()
    {
        this.poisonBullet.vfxPoisonPool.Enqueue(this.gameObject);        
    }

    public void Inactivate()
    {
        this.gameObject.SetActive(false);
    }
}
