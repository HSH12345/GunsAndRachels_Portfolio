using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GunShell : MonoBehaviour
{
    public float Angle { get { return this.angle; } }
    public bool IsShoot { get { return this.joystickGunDir.IsShoot; } }

    public bool IsFirstSkillOperated { get { return this.joystickSkillDir[0].IsSkillOperated; } }
    public bool IsSecondSkillOperated { get { return this.joystickSkillDir[1].IsSkillOperated; } }
    public bool IsThirdSkillOperated { get { return this.joystickSkillDir[2].IsSkillOperated; } }

    //슬로우 기능 제거
    //public bool IsSlow { get { return this.joystickSkillDir.IsSlow; } }

    public enum eSkillType
    {
        Line, Arc, Circle, Button
    }

    public eSkillType[] eSkillTypes;

    // 탄환 관련 컴포넌트
    [System.NonSerialized]
    public GameObject bulletGo;
    public GameObject bulletPoint;
    public GameObject[] bulletPoints;
    public GameObject shootVfx;

    public GameObject[] skillGo = new GameObject[3];
    // 공격 방향vector
    private Vector2 shootDir;
    public Vector2 skillDir;

    public bool isSetGun;
    [SerializeField]
    private GrenadeLauncher grenadeLauncher;

    //사운드 관련
    private bool isFirstShoot;
    private bool isLastShoot;
    private int sfxIdx;
    [SerializeField]
    private AudioSource sfxGunShot;
    [SerializeField]
    private AudioSource[] sfxGunShots;

    public void Shoot()
    {
        // 탄환 갯수
        int bulletAmount = InfoManager.instance.charactoristicInfo.bulletAmountCharacteristic;

        // 탄환 방향 관련
        float rotationZ = 0;
        int bulletCnt = 0;
        bool isEven = false;

        switch (bulletAmount)
        {
            case 1:
                bulletCnt = 1;
                break;
            case 2:
                bulletCnt = 2;
                isEven = true;
                break;
            case 3:
                rotationZ = 20;
                bulletCnt = bulletAmount;
                break;
            case 4:
                rotationZ = 20;
                bulletCnt = bulletAmount - 2;
                isEven = true;
                break;
            case 5:
                rotationZ = 40;
                bulletCnt = bulletAmount;
                break;
            case 6:
                rotationZ = 40;
                bulletCnt = bulletAmount - 2;
                isEven = true;
                break;
            case 7:
                rotationZ = 60;
                bulletCnt = bulletAmount;
                break;
            case 8:
                rotationZ = 60;
                bulletCnt = bulletAmount - 2;
                isEven = true;
                break;
            case 9:
                rotationZ = 80;
                bulletCnt = bulletAmount;
                break;
            case 10:
                rotationZ = 80;
                bulletCnt = bulletAmount - 2;
                isEven = true;
                break;
            default:
                break;
        }

        // 현재 총기의 데미지 공식
        double attackpower = this.currentGun.gunForm.attackpower;
        int damage = Mathf.RoundToInt((float)attackpower);
        GameObject go;
        if (isEven)
        {
            for (int i = 0; i < 2; i++)
            {
                if (this.objectPooler.bulletPool.Count > 0)
                {
                    go = this.objectPooler.bulletPool.Dequeue();
                    go.SetActive(true);
                }
                else
                {
                    go = Instantiate(this.bulletGo);
                }

                go.transform.rotation = this.lookRotation;
                //데미지는 공식에서 90%~110% 중 렌덤하게 전달됩니다.
                go.GetComponent<Bullet>().Init(this.currentGun.gunForm.bulletSpeed, this.currentGun.gunForm.bulletExistTime,
                    Random.Range(Mathf.RoundToInt(damage * 0.9f), Mathf.RoundToInt(damage * 1.1f)), this);
                go.transform.position = this.bulletPoints[i].transform.position;
            }
        }

        // 탄환 오브젝트 풀링
        for (int i = 0; i < bulletCnt; i++)
        {
            if (bulletAmount == 2) continue;
            var rotation = Quaternion.Euler(0, 0, rotationZ);

            if (this.objectPooler.bulletPool.Count > 0)
            {
                go = this.objectPooler.bulletPool.Dequeue();
                if (go.activeSelf) go = Instantiate(this.bulletGo);
                go.SetActive(true);
            }
            else
            {
                go = Instantiate(this.bulletGo);
            }

            // 탄환 초기화
            go.transform.rotation = this.lookRotation * rotation;
            go.GetComponent<Bullet>().Init(this.currentGun.gunForm.bulletSpeed, this.currentGun.gunForm.bulletExistTime,
                Random.Range(Mathf.RoundToInt(damage * 0.9f), Mathf.RoundToInt(damage * 1.1f)), this);
            go.transform.position = this.bulletPoint.transform.position;
            if (isEven && rotationZ == 20) rotationZ = 0;
            rotationZ -= 20;
        }

        // 그레네이드 런쳐 구현
        if (this.grenadeLauncher.gameObject.activeSelf)
        {
            this.grenadeLauncher.grenadeShoot(this.bulletPoint, this.lookRotation);
        }

        // 발사 vfx 오브젝트풀링
        GameObject vfxGo;
        if (this.objectPooler.shootVfxPool.Count > 0)
        {
            vfxGo = this.objectPooler.shootVfxPool.Dequeue();
            if (vfxGo.activeSelf) vfxGo = Instantiate(this.shootVfx);
            vfxGo.SetActive(true);
        }
        else
        {
            vfxGo = Instantiate(this.shootVfx);
        }
        vfxGo.GetComponent<ShootVfx>().Init();
        vfxGo.transform.parent = this.bulletPoint.transform;
        vfxGo.transform.localPosition = Vector3.zero;

        this.gun.GetComponent<Animator>().Play("GunShot", -1, 0);        
    }

    public bool IsSkillOperated()
    {
        if (this.IsFirstSkillOperated || this.IsSecondSkillOperated || this.IsThirdSkillOperated) return true;
        else return false;
    }

    public bool IsAllSkillStoped()
    {
        if (!this.IsFirstSkillOperated && !this.IsSecondSkillOperated && !this.IsThirdSkillOperated) return true;
        else return false;
    }


    // 건샷 사운드 구현
    public void OneShotGunShotSFX()
    {
        var audio = AudioManager.instance;
        var gunType = this.currentGun.gunForm.gunType;
        var gunshotSFXPlayList = GetGunshotSFXPlayList(gunType);

        if (isFirstShoot)
        {
            var startGunShot = GetStartGunShot(gunType);
            audio.PlaySFXOneShot(startGunShot, this.sfxGunShot);
            this.isFirstShoot = false;
        }
        else
        {
            this.PlayRandomGunShot(audio, gunshotSFXPlayList);
        }
    }

    private AudioManager.eSFXMusicPlayList GetStartGunShot(string gunType)
    {
        // gunType 이름에 맞는 Start_GunShot 반환
        return (AudioManager.eSFXMusicPlayList)System.Enum.Parse(typeof(AudioManager.eSFXMusicPlayList), $"GunShot_Start_{gunType}");
    }

    private AudioManager.eSFXMusicPlayList[] GetGunshotSFXPlayList(string gunType)
    {
        // gunType 이름에 맞는 GunShot 반환
        List<AudioManager.eSFXMusicPlayList> result = new List<AudioManager.eSFXMusicPlayList>();
        for (int i = 1; i <= 5; i++)
        {
            var gunshot = (AudioManager.eSFXMusicPlayList)System.Enum.Parse(typeof(AudioManager.eSFXMusicPlayList), $"GunShot_{i}_{gunType}");
            result.Add(gunshot);
        }
        return result.ToArray();
    }

    private void PlayRandomGunShot(AudioManager audio, AudioManager.eSFXMusicPlayList[] gunshotSFXPlayList)
    {
        var dice = Random.value;
        for (int i = 0; i < gunshotSFXPlayList.Length; i++)
        {
            if (dice > i * 0.2f && dice <= (i + 1) * 0.2f)
            {
                if (i == this.sfxIdx)
                {
                    i = i - 1;
                    if (i < 0) i = 4;
                }
                audio.PlaySFXOneShot(gunshotSFXPlayList[i], this.sfxGunShots[i]);

                this.sfxIdx = i;
                break;
            }
        }
    }

    private void OneShotLastGunShotSFX()
    {
        var audio = AudioManager.instance;
        var gunType = this.currentGun.gunForm.gunType;
        var lastGunShot = GetLastGunShot(gunType);

        this.isFirstShoot = true;
    }

    private AudioManager.eSFXMusicPlayList GetLastGunShot(string gunType)
    {
        // gunType 이름에 맞는 Last_GunShot 반환
        return (AudioManager.eSFXMusicPlayList)System.Enum.Parse(typeof(AudioManager.eSFXMusicPlayList), $"GunShot_Last_{gunType}");
    }

}
