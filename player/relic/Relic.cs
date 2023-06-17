using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{
    [System.NonSerialized]
    public PlayerShell playerShell;
    [System.NonSerialized]
    public GunShell gunShell;

    public virtual void Init()
    {
        this.playerShell = this.gameObject.transform.parent.gameObject.GetComponentInParent<PlayerShell>();
        this.gunShell = this.playerShell.gameObject.transform.Find("GunShell").GetComponent<GunShell>();
    }
}
