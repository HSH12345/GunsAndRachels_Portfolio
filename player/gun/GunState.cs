using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구조체에 현재 총기의 데이터 값 저장합니다.
public struct GunForm
{
    public string gunType;

    public double attackpower;
    public float attackDelayTime;

    public int gunLevel;
    public float bulletSpeed;
    public float bulletExistTime;    

    public int[] skillTypes;
    public string[] skillIconNames;
    public float[] skillCoolTime;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public GameObject[] skills;
    public GameObject[] bullets;
}

// 추상 클래스로 기본 상태를 정의합니다.
public abstract class GunState : MonoBehaviour
{
    public GunForm gunForm;

    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public GameObject[] skills;
    public GameObject[] bullets;

    [System.NonSerialized]
    public int gunLevel;

    [System.NonSerialized]
    public GameObject bulletPoint;
    [System.NonSerialized]
    public GameObject BuffPoint;
    [System.NonSerialized]
    public GameObject player;

    public virtual void Init()
    {
        this.gunForm.skillTypes = new int[3];
        this.gunForm.skills = new GameObject[3];
        this.gunForm.bullets = new GameObject[4];
        this.gunForm.skillIconNames = new string[3];
        this.gunForm.skillCoolTime = new float[3];

        this.gunForm.gunLevel = this.gunLevel;

        this.FindTrans();

        for(int i = 0; i < this.skills.Length; i++)
        {
            var activeSkill = this.skills[i].GetComponent<PlayerActiveSkill>();
            if(activeSkill != null)
            {
                activeSkill.transBulletPoint = this.bulletPoint.transform;
                activeSkill.transBuffPoint = this.BuffPoint.transform;
                activeSkill.transPlayer = this.player.transform;
            }
        }
    }

    private void FindTrans()
    {
        this.bulletPoint = this.gameObject.transform.Find("gun").transform.Find("bulletPoint").gameObject;
        this.player = this.GetComponentInParent<PlayerShell>().gameObject;
        this.BuffPoint = this.player.transform.Find("buffPoint").gameObject;
    }
}
