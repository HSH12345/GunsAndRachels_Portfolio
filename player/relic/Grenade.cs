using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private float bulletSpeed;
    private float maxExistTime;
    private float existTime;
    private Rigidbody2D rBody2D;

    private Vector2 dir;
    public GameObject impactVfxGo;
    private GrenadeLauncher grenadeLauncher;
    private bool isExploded;

    public void Init(float bulletSpeed, float maxExistTime, GrenadeLauncher grenadeLauncher)
    {
        this.isExploded = false;
        this.existTime = 0;
        this.bulletSpeed = bulletSpeed;
        this.maxExistTime = maxExistTime;
        this.rBody2D = this.gameObject.GetComponent<Rigidbody2D>();
        this.grenadeLauncher = grenadeLauncher;
        // 탄환이 항상 정면을 향합니다.
        float angle = this.transform.eulerAngles.z % 360;
        this.dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    void FixedUpdate()
    {
        this.rBody2D.velocity = this.dir.normalized * this.bulletSpeed * Time.deltaTime;

        this.existTime += Time.deltaTime;
        if (this.existTime >= this.maxExistTime)
        {
            if (!this.isExploded)
            {
                this.existTime = 0;
                this.gameObject.SetActive(false);
                this.isExploded = true;
            }
        }
    }

    public void OnDisable()
    {
        if (!this.isExploded)
        {
            this.GrenadeExplosion();
            this.grenadeLauncher.grenadePool.Enqueue(this.gameObject);
            this.isExploded = true;
        }
    }

    // 유탄은 폭발 오브젝트를 생성하고 데미지 기능은 아래 오브젝트에서 구현합니다.
    public void GrenadeExplosion()
    {
        GameObject go = Instantiate(this.impactVfxGo);
        go.GetComponent<PlayerActiveSkill>().Init(this.grenadeLauncher.gunShell);
        go.transform.position = this.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.gameObject.SetActive(false);        
    }
}
