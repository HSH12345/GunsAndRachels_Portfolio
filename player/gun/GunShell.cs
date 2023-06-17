using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public partial class GunShell : MonoBehaviour
{
    public SpriteAtlas sa;

    public enum eGunState
    {
        Idle, Shoot
    }

    public GunState currentGun;

    [System.NonSerialized]
    public JoystickGunDir joystickGunDir;
    [System.NonSerialized]
    public JoystickSkillDir[] joystickSkillDir;
    [System.NonSerialized]
    public Button[] btnSkills;

    private float angle;
    public GameObject gun;
    public Animator anim;
    public GameObject[] eyePoint;
    [System.NonSerialized]
    public Quaternion lookRotation;

    public GunForm gunForm;
    private Coroutine destroyRoutine;

    private PlayerObjectPooler objectPooler;
    [SerializeField]
    private LaserLine laserLine;

    public void Init()
    {
        this.objectPooler = GameObject.FindObjectOfType<PlayerObjectPooler>().gameObject.GetComponent<PlayerObjectPooler>();
        this.joystickGunDir = GameObject.FindObjectOfType<JoystickGunDir>();

        this.joystickSkillDir = new JoystickSkillDir[3];
        this.joystickSkillDir[0] = GameObject.Find("JoystickSkillDirFirst").GetComponent<JoystickSkillDir>();
        this.joystickSkillDir[1] = GameObject.Find("JoystickSkillDirSecond").GetComponent<JoystickSkillDir>();
        this.joystickSkillDir[2] = GameObject.Find("JoystickSkillDirThird").GetComponent<JoystickSkillDir>();

        this.btnSkills = new Button[3];
        var rectJoysticksRight = GameObject.Find("rectJoysticksRight");
        this.btnSkills[0] = rectJoysticksRight.transform.Find("btnSkillFirst").GetComponent<Button>(); 
        this.btnSkills[1] = rectJoysticksRight.transform.Find("btnSkillSecond").GetComponent<Button>();
        this.btnSkills[2] = rectJoysticksRight.transform.Find("btnSkillThird").GetComponent<Button>();

        EventDispatcher.Instance.AddListener<string>(EventDispatcher.EventName.DungeonSceneMainTakeGun, this.ChangeGun);
        this.ChangeGun(InfoManager.instance.charactoristicInfo.currentGunName);
        this.isFirstShoot = true;
        this.SetGun();
    }

    private void FixedUpdate()
    {
        // �Ϲ� ����
        if (this.joystickGunDir.IsShoot)
        {
            if (this.joystickGunDir.Input == Vector3.zero) return;
            this.shootDir = new Vector2(this.joystickGunDir.Horizontal, this.joystickGunDir.Vertical);

            //���̽�ƽ���� �Է¹��� ��ġ���� ���� -> ������ ��ȯ�ϴ� �����Դϴ�.
            this.angle = Mathf.Atan2(this.joystickGunDir.Vertical, this.joystickGunDir.Horizontal) * Mathf.Rad2Deg;
            this.lookRotation = Quaternion.Euler(angle * Vector3.forward);
            this.transform.rotation = this.lookRotation;
        }
        // ��ų ����
        for (int i = 0; i < this.joystickSkillDir.Length; i++)
        {
            if (this.joystickSkillDir[i].IsDraged)
            {
                if (this.joystickSkillDir[i].Input == Vector3.zero) return;
                this.skillDir = new Vector2(this.joystickSkillDir[i].Horizontal, this.joystickSkillDir[i].Vertical);

                this.angle = Mathf.Atan2(this.joystickSkillDir[i].Vertical, this.joystickSkillDir[i].Horizontal) * Mathf.Rad2Deg;
                this.lookRotation = Quaternion.Euler(angle * Vector3.forward);
                this.transform.rotation = this.lookRotation;
            }
        }

        this.SetEyePos();
    }

    // �ѱ��� ���� ���¸� ���ΰ�ħ �մϴ�.
    public void SetGun()
    {
        this.currentGun.Init();
        this.gun.GetComponent<Animator>().runtimeAnimatorController = this.currentGun.anim.runtimeAnimatorController;
        this.anim = this.gun.GetComponent<Animator>();
        this.gun.GetComponent<SpriteRenderer>().sprite = this.currentGun.spriteRenderer.sprite;

        GameObject.FindObjectOfType<PlayerShell>().maxFireRate = this.currentGun.gunForm.attackDelayTime;

        int gunLevel = InfoManager.instance.charactoristicInfo.gunProficiencyLevel;
        if (gunLevel >= 0 && gunLevel < 10) this.bulletGo = this.currentGun.gunForm.bullets[0];
        else if (gunLevel >= 10 && gunLevel < 20) this.bulletGo = this.currentGun.gunForm.bullets[1];
        else if (gunLevel >= 20 && gunLevel < 30) this.bulletGo = this.currentGun.gunForm.bullets[2];
        else if (gunLevel == 30) this.bulletGo = this.currentGun.gunForm.bullets[3];

        this.eSkillTypes = new eSkillType[3];

        for (int i = 0; i < this.btnSkills.Length; i++)
        {
            this.btnSkills[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < this.eSkillTypes.Length; i++)
        {
            this.eSkillTypes[i] = (eSkillType)this.currentGun.gunForm.skillTypes[i];
            this.joystickSkillDir[i].imgSkillIcon.sprite = this.sa.GetSprite(this.currentGun.gunForm.skillIconNames[i]);
            this.skillGo[i] = this.currentGun.gunForm.skills[i];

            if (this.eSkillTypes[i] == eSkillType.Button) this.btnSkills[i].gameObject.SetActive(true);
        }

        if (this.laserLine.gameObject.activeSelf) this.laserLine.Show();

        this.DestroyBullet();

        InfoManager.instance.charactoristicInfo.currentGunName = this.currentGun.gunForm.gunType;
        this.joystickGunDir.lastGunShot = this.OneShotLastGunShotSFX;
    }

    // ������Ʈ Ǯ������ źȯ�� �����մϴ�.
    private void DestroyBullet()
    {
        if (this.objectPooler.bulletPool != null)
        {
            foreach (var bullet in this.objectPooler.bulletPool)
            {
                if (bullet.activeSelf) bullet.SetActive(false);
                Destroy(bullet.gameObject);
            }
            this.objectPooler.bulletPool.Clear();
        }

        if (this.destroyRoutine != null) StopCoroutine(this.destroyRoutine);
        this.destroyRoutine = StartCoroutine(this.DestroyBulletRoutine());
    }

    // SetGun�� ȣ��� �� �̹� �߻�� źȯ�� ó���ϱ� ���� ���� �����Դϴ�.
    IEnumerator DestroyBulletRoutine()
    {
        this.isSetGun = true;
        yield return new WaitForSeconds(1.5f);
        this.isSetGun = false;
    }

    private void SetEyePos()
    {
        var gunSprite = this.gun.GetComponent<SpriteRenderer>();
        var eyeSprite0 = this.GetEyeSprite(0);
        var eyeSprite1 = this.GetEyeSprite(1);

        bool angleCondition = this.angle < -90f || this.angle > 90f;
        gunSprite.flipY = angleCondition;
        this.SetEyePointActive(0, !angleCondition);
        this.SetEyePointActive(1, angleCondition);
        this.bulletPoint.transform.localPosition = new Vector2(0.844f, angleCondition ? -0.05f : 0.05f);

        bool sortingCondition = this.angle < 130f && this.angle > 60f;
        int gunOrder = sortingCondition ? -1 : 9;
        int eyeOrder = sortingCondition ? -1 : 10;

        this.SetSortingOrder(gunSprite, eyeSprite0, eyeSprite1, gunOrder, eyeOrder);
    }

    private SpriteRenderer GetEyeSprite(int index)
    {
        return this.eyePoint[index].transform.Find("eye").GetComponent<SpriteRenderer>();
    }

    private void SetEyePointActive(int index, bool isActive)
    {
        this.eyePoint[index].SetActive(isActive);
    }

    private void SetSortingOrder(SpriteRenderer gunSprite, SpriteRenderer eyeSprite0, SpriteRenderer eyeSprite1,
        int gunOrder, int eyeOrder)
    {
        gunSprite.sortingOrder = gunOrder;
        eyeSprite0.sortingOrder = eyeOrder;
        eyeSprite1.sortingOrder = eyeOrder;
    }

    // ��ø �޼���� �ѱ⸦ �����ϰ� ����� ������ �ѱ� Ư��ġ�� �ٸ��� �����մϴ�.
    public void ChangeGun(string gunName)
    {
        var characteristic = InfoManager.instance.charactoristicInfo;

        void UpdateCharacteristic(ref int characteristicValue, int modifier)
        {
            characteristicValue = Mathf.Clamp(characteristicValue + modifier, 1, 10);
        }

        void UpdateCharacteristicConsideringOrigin(ref int characteristicValue, ref int originCharacteristicValue, int modifier)
        {
            if (originCharacteristicValue >= 9 && modifier == -2)
            {
                UpdateCharacteristic(ref characteristicValue, 10 - originCharacteristicValue);
            }
            else
            {
                UpdateCharacteristic(ref characteristicValue, modifier);
            }
        }

        void UpdateGunCharacteristics(string gunType, int modifier)
        {
            switch (gunType)
            {
                case "AssultRifle":
                    UpdateCharacteristicConsideringOrigin(ref characteristic.kncokBackCharacteristic, ref characteristic.originKncokBackCharacteristic, modifier);
                    break;
                case "SniperRifle":
                    UpdateCharacteristicConsideringOrigin(ref characteristic.penetrateCharacteristic, ref characteristic.originPenetrateCharacteristic, modifier);
                    break;
                case "ShotGun":
                    UpdateCharacteristicConsideringOrigin(ref characteristic.bulletAmountCharacteristic, ref characteristic.originBulletAmountCharacteristic, modifier);
                    break;
                case "SubmachineGun":
                    UpdateCharacteristicConsideringOrigin(ref characteristic.dashRecoverCharacteristic, ref characteristic.originDashRecoverCharacteristic, modifier);
                    break;
            }
        }

        UpdateGunCharacteristics(this.currentGun.gunForm.gunType, -2);
        this.currentGun = this.gameObject.GetComponent(gunName) as GunState;
        UpdateGunCharacteristics(gunName, 2);

        this.SetGun();
    }
}
