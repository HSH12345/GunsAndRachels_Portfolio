using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunSkill_02 : PlayerActiveSkill, IPlayerBuffSkill
{
    private int buffAmount;
    private float buffTime;
    private Coroutine buffRoutine;
    [SerializeField]
    private GameObject vfxBuffGO;

    public override void Init(GunShell gunShell)
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        this.buffTime = 5;
        this.existTime = 0.933f;
        this.transform.position = this.transBuffPoint.transform.position;
        base.Init(gunShell);
        this.buffAmount = (int)InfoManager.instance.statInfo.fireRateStat / 2;
        this.Buff();
        this.vfxBuffGO.GetComponentInChildren<MeshRenderer>().sortingLayerName = "FlyingObject";
        this.vfxBuffGO.GetComponentInChildren<MeshRenderer>().sortingOrder = 99;
        this.vfxBuffGO.transform.parent = gunShell.transform.parent;
        this.vfxBuffGO.transform.localPosition = Vector3.zero;
    }

    private void FixedUpdate()
    {
        this.transform.position = this.transBuffPoint.transform.position;
    }

    public void Buff()
    {
        this.buffRoutine = StartCoroutine(this.BuffRoutine());
    }

    private IEnumerator BuffRoutine()
    {
        InfoManager.instance.statInfo.fireRateStat += this.buffAmount;
        this.gunShell.SetGun();
        yield return new WaitForSeconds(this.existTime);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    protected override IEnumerator DestoryRoutine()
    {
        yield return new WaitForSeconds(this.buffTime);
        InfoManager.instance.statInfo.fireRateStat -= this.buffAmount;
        this.gunShell.SetGun();
        Destroy(this.vfxBuffGO);
        Destroy(this.gameObject);
    }
}

