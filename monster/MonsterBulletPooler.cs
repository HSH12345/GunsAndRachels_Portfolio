using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터가 존재하는 씬에 항상 배치합니다.
public class MonsterBulletPooler : MonoBehaviour
{
    public static MonsterBulletPooler instance;

    public MonsterBulletPooler()
    {

    }

    [System.NonSerialized]
    public Queue<GameObject> cobraBulletPool;
    [System.NonSerialized]
    public Queue<GameObject> damagePopupPool;
    [System.NonSerialized]
    public Queue<GameObject> ratfolkMageBulletPool;
    [System.NonSerialized]
    public Queue<GameObject> koboldPriestBulletPool;
    [System.NonSerialized]
    public Queue<GameObject> golemBulletPool;
    [System.NonSerialized]
    public Queue<GameObject> witchBulletPool;
    [System.NonSerialized]
    public Queue<GameObject> militaryIncursionBoBullet1Pool;
    [System.NonSerialized]
    public Queue<GameObject> militaryIncursionBoBullet2Pool;
    public AudioSource sfxSource;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        this.cobraBulletPool = new Queue<GameObject>();
        this.damagePopupPool = new Queue<GameObject>();
        this.ratfolkMageBulletPool = new Queue<GameObject>();
        this.koboldPriestBulletPool = new Queue<GameObject>();
        this.golemBulletPool = new Queue<GameObject>();
        this.witchBulletPool = new Queue<GameObject>();
        this.militaryIncursionBoBullet1Pool = new Queue<GameObject>();
        this.militaryIncursionBoBullet2Pool = new Queue<GameObject>();
    }

}
