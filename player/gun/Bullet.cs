using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactVfxGo;

    public float bulletSpeed;
    [System.NonSerialized]
    public float knockbackSpeed;
    [System.NonSerialized]
    public int damage;
    private float maxExistTime;
    private float existTime;
    [System.NonSerialized]
    public int penetrateCnt;
    [System.NonSerialized]

    public Vector2 dir;
    private Rigidbody2D rBody2D;
    private GunShell gunShell;
    private PlayerObjectPooler obejctPooler;
    [SerializeField]
    private bool isSkill;
    [SerializeField]
    private AudioSource sfxSkill;

    public virtual void Init(float bulletSpeed, float maxExistTime, int damage, GunShell gunShell = null)
    {
        this.existTime = 0;
        this.bulletSpeed = bulletSpeed;
        this.maxExistTime = maxExistTime;
        this.rBody2D = this.GetComponent<Rigidbody2D>();
        //방향이 항상 정면으로 진행하도록 합니다.
        float angle = this.transform.eulerAngles.z % 360;
        this.dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        this.damage = damage;
        this.penetrateCnt = InfoManager.instance.charactoristicInfo.penetrateCharacteristic;
        this.knockbackSpeed = InfoManager.instance.charactoristicInfo.kncokBackCharacteristic * 2000;
        this.gunShell = gunShell;        
        this.obejctPooler = PlayerObjectPooler.instance;

        if (this.isSkill) 
        {
            this.penetrateCnt = 1;
            this.MakeSkillSound();
        } 
    }

    private void MakeSkillSound()
    {
        var audio = AudioManager.instance;
        Debug.Log(this.gunShell.currentGun.gunForm.gunType);
        var gunType = this.gunShell.currentGun.gunForm.gunType;
        var skill1 = GetStartGunShot(gunType);
        audio.PlaySFXOneShot(skill1, this.gunShell.gameObject.transform.Find("audioSources").gameObject.transform.Find("skill").GetComponent<AudioSource>());
    }

    private AudioManager.eSFXMusicPlayList GetStartGunShot(string gunType)
    {
        return (AudioManager.eSFXMusicPlayList)System.Enum.Parse(typeof(AudioManager.eSFXMusicPlayList), $"Skill1_{gunType}");
    }

    public void OnDisable()
    {
        if (this.isSkill) Destroy(this.gameObject);
        else
        {
            this.obejctPooler.bulletPool.Enqueue(this.gameObject);
            // 총기 새로고침 중 탄환 오브젝트를 파괴합니다.
            if (this.gunShell.isSetGun)
            {
                this.obejctPooler.bulletPool.Dequeue();
                Destroy(this.gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        this.rBody2D.velocity = this.dir.normalized * this.bulletSpeed * Time.deltaTime;

        this.existTime += Time.deltaTime;
        if (this.existTime >= this.maxExistTime)
        {
            this.existTime = 0;
            this.gameObject.SetActive(false);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go;
        if (!this.isSkill) 
        {
            // 탄환 폭발 vfx 오브젝트풀링
            if (this.obejctPooler.bulletVfxPool.Count > 0)
            {
                go = this.obejctPooler.bulletVfxPool.Dequeue();
                if (go.activeSelf) go = Instantiate(this.impactVfxGo);
                go.SetActive(true);
            }
            else
            {
                go = Instantiate(this.impactVfxGo);
            }
            
            go.GetComponent<bulletVfx>().Init();

            // 탄환 충돌기능 구현
            if (!collision.collider.CompareTag("Monster")) this.gameObject.SetActive(false);
            else
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            }

            this.penetrateCnt--;
            if (this.penetrateCnt <= 0) this.gameObject.SetActive(false);
        }
        else
        {
            if (collision.collider.CompareTag("MonsterBullet")) return;

            go = Instantiate(this.impactVfxGo);
            if (!collision.collider.CompareTag("Monster")) 
            {
                Destroy(this.gameObject);
            }
            else
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            }
        }
        go.transform.position = this.transform.position;
    }
}
