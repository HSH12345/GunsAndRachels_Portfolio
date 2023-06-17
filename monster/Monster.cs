using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Damageable
{
    public PathFinding pathFinding;
    public AnimationClip deadAnimationClip;
    private MonsterGenerator monsterGenerator;
    [System.NonSerialized]
    public int id;
    [SerializeField]
    private float hpMutiplyer = 1;

    public override void Init(MonsterGenerator monsterGenerator, int maxHP, int defenseRate)
    {
        base.Init(monsterGenerator, maxHP, defenseRate);
        this.isDead = false;
        this.monsterGenerator = monsterGenerator;
        this.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        this.pathFinding.Init(GameObject.FindObjectOfType<PlayerShell>().gameObject.transform);
        this.maxHP = Mathf.RoundToInt(maxHP * this.hpMutiplyer) * InfoManager.instance.gameInfo.roundCnt;
        this.hp = this.maxHP;
        this.defenseRate = defenseRate;
    }

    private void OnDisable()
    {
        if (monsterGenerator != null) this.monsterGenerator.ReturnMonsterToPool(this.id, this.gameObject);
        var monsterKillCnt = InfoManager.instance.gameInfo.killCnt++;
    }

    protected override IEnumerator GetPoisonDamageRoutine()
    {
        while (true)
        {
            if (this.maxPoisonCnt <= this.getPoisonCnt || this.isDead) break;
            if (this.poisonBullet != null) this.poisonBullet.ShowVfxPoison(this.gameObject.transform);
            this.pathFinding.HitPlayerActiveSkill(-this.pathFinding.dir, 1000);
            this.TakeDamage((InfoManager.instance.statInfo.criticalHitAmount + InfoManager.instance.statInfo.criticalHitChance) * 3);
            yield return new WaitForSeconds(1.2f);
            this.getPoisonCnt++;
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (this.hp <= 0) this.isDead = true;
        if (this.isDead)
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.Monster_Dead, this.objectPooler.sfxSource);
            this.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.DungeonSceneMainPlayerExpUp, this.exp);
            EventDispatcher.Instance.Dispatch<Vector3>(EventDispatcher.EventName.ChestItemGeneratorMakeFieldCoin,
                this.transform.position);
            StartCoroutine(this.MonsterDeadRoutine());
        }
    }

    private IEnumerator MonsterDeadRoutine()
    {
        this.gameObject.GetComponent<Animator>().SetInteger("State", -1);
        this.gameObject.GetComponent<Animator>().Play("Dead", -1, 0);
        yield return new WaitForSeconds(this.deadAnimationClip.length);
        this.pathFinding.endInit = false;
        this.gameObject.SetActive(false);
    }
}
