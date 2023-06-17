using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActiveSkill : MonoBehaviour
{
    [System.NonSerialized]
    public int damage;
    [System.NonSerialized]
    public float knockbackSpeed;
    [System.NonSerialized]
    public float existTime;
    public bool isCircle;

    public Transform transBulletPoint;
    public Transform transBuffPoint;
    public Transform transPlayer;

    protected GunShell gunShell;

    public virtual void Init(GunShell gunShell)
    {
        this.gunShell = gunShell;
        this.MakeSkillSound();
        StartCoroutine(DestoryRoutine());
    }

    private void MakeSkillSound()
    {
        var audio = AudioManager.instance;
        //Debug.Log(this.gunShell.currentGun.gunForm.gunType);
        var gunType = this.gunShell.currentGun.gunForm.gunType;
        var skill1 = GetStartGunShot(gunType);
        audio.PlaySFXOneShot(skill1, this.gunShell.gameObject.transform.Find("audioSources").gameObject.transform.Find("skill").GetComponent<AudioSource>());
    }

    private AudioManager.eSFXMusicPlayList GetStartGunShot(string gunType)
    {
        var skillNum = "";
        if (this.GetComponent<AssultRifleSkill_02>() != null || this.GetComponent<ShotGunSkill_02>() != null
            || this.GetComponent<SniperRifleSkill_02>() != null || this.GetComponent<SubmachineGunSkill_02>() != null) skillNum = "2";
        else if (this.GetComponent<AssultRifleSkill_03>() != null || this.GetComponent<ShotGunSkill_03>() != null
            || this.GetComponent<SniperRifleSkill_03>() != null || this.GetComponent<SubmachineGunSkill_03>() != null) skillNum = "3";
        else if (this.GetComponent<VfxGrenadeExplosion>() != null)
        {
            return (AudioManager.eSFXMusicPlayList)System.Enum.Parse(typeof(AudioManager.eSFXMusicPlayList), $"Relic_GrenadeExplosion");
        }
        // string -> System.Type -> System.Enum -> AudioManager.eSFXMusicPlayList º¯È¯
        return (AudioManager.eSFXMusicPlayList)System.Enum.Parse(typeof(AudioManager.eSFXMusicPlayList), $"Skill{skillNum}_{gunType}");
    }

    protected virtual IEnumerator DestoryRoutine()
    {
        yield return new WaitForSeconds(this.existTime);
        Destroy(this.gameObject);
        Debug.Log("DestroyThisSkill");
    }
}
