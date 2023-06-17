using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterBullet
{

}

public class MonsterBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float maxExistTime;
    protected float existTime;
    protected Vector2 dir;
    protected Rigidbody2D rBody2D;
    protected Queue<GameObject> pooler;

    public void Init(Queue<GameObject> pooler)
    {
        this.pooler = pooler;
        float angle = this.transform.eulerAngles.z % 360;
        this.dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        //this.dir = dir;
        this.rBody2D = this.gameObject.GetComponent<Rigidbody2D>();
        this.existTime = 0;
    }

    protected void OnDisable()
    {
        this.pooler.Enqueue(this.gameObject);
    }

    protected virtual void FixedUpdate()
    {
        this.rBody2D.velocity = this.dir.normalized * this.bulletSpeed * Time.deltaTime;


        this.existTime += Time.deltaTime;
        if (this.existTime >= this.maxExistTime)
        {
            this.existTime = 0;
            this.gameObject.SetActive(false);
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        this.gameObject.SetActive(false);
    }
}