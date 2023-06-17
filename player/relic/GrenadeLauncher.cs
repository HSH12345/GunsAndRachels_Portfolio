using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Relic
{
    //gunshell���� ����
    public GameObject grenadeGO;
    private bool isReloaded;
    private float reloadTime;
    private float maxReloadTime;

    [System.NonSerialized]
    public Queue<GameObject> grenadePool;
    [System.NonSerialized]
    public GameObject go;

    public override void Init()
    {
        base.Init();
        this.maxReloadTime = 0.5f;
        this.grenadePool = PlayerObjectPooler.instance.grenadePool;
    }

    public void grenadeShoot(GameObject bulletPoint, Quaternion lookRotation)
    {
        if (this.isReloaded)
        {
            if (this.grenadePool.Count > 0)
            {
                go = grenadePool.Dequeue();
                if (go.activeSelf) go = Instantiate(this.grenadeGO);
                go.SetActive(true);
            }
            else
            {
                go = Instantiate(this.grenadeGO);
            }

            this.isReloaded = false;
            go.transform.position = bulletPoint.transform.position;
            go.transform.rotation = lookRotation;

            float bulletSpeed = 400;
            float maxExsitTime = 0.6f;
            go.GetComponent<Grenade>().Init(bulletSpeed, maxExsitTime, this.GetComponent<GrenadeLauncher>());
        }
    }

    private void Update()
    {
        // ������ maxReloadTime ���� �Ѿ���� �ش� ������ �ѹ� �� �� �� ���� reloadTime�� Time.deltaTime���� ���߽��ϴ�.
        if(this.maxReloadTime + this.maxReloadTime > this.reloadTime && !this.isReloaded) this.reloadTime += Time.deltaTime;
        if (this.maxReloadTime <= this.reloadTime) 
        {
            this.isReloaded = true;
            this.reloadTime = 0;
        } 
    }

    public GameObject Grenade()
    {
        return this.go;
    }
}
