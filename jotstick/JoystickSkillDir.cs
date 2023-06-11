using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickSkillDir : Joystick
{
    //��ų ��Ұ���
    public RectTransform skillCancelPoint;
    public Image imgSkillCancelIcon;
    private float distSkillCancelPoint;
    //��ų ��Ÿ�� ����
    public GameObject imgMaskSkillGO;
    public Image imgSkillIcon;

    [System.NonSerialized]
    public bool playerOnPortal;
    public System.Action shootSkill;
    public bool IsSkillOperated { get { return this.IsDraged; } }
    public Vector2 HandlePos { get { return this.handle.transform.localPosition; } }

    [SerializeField]
    private AudioSource sfxSource;

    public override void OnPointerUp(PointerEventData eventData)
    {
        //��ų ��� �����ܰ� �Ÿ��� Ȯ���Ͽ� ��ų�� ����մϴ�.
        if (this.distSkillCancelPoint > 70)
        {
            if(!this.playerOnPortal || this.shootSkill != null) this.shootSkill();
        }
        else
        {
            this.CancelSkill();
        }

        base.OnPointerUp(eventData);        
                
        this.frame.gameObject.SetActive(false);
        this.skillCancelPoint.gameObject.SetActive(false);
        this.imgMaskSkillGO.SetActive(true);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        //��ų�� ��ư���� ���ȴٸ� ���� ui�� ���ϴ�.
        if (this.isButton) this.frame.gameObject.SetActive(false);
        else this.frame.gameObject.SetActive(true);

        //��ų ��� ui�� ��ġ ����Ʈ�� �Ÿ��� ����Ͽ� ui�� ������ �����մϴ�.
        this.skillCancelPoint.gameObject.SetActive(true);
        this.distSkillCancelPoint = Vector2.Distance(eventData.position, this.skillCancelPoint.transform.position);
        Color pointedSkillCancelPointColor;
        if (this.distSkillCancelPoint < 70)
        {
            pointedSkillCancelPointColor = new Color(255, 0, 0, 0.5f);
        }
        else
        {
            pointedSkillCancelPointColor = new Color(255, 255, 255, 0.5f);
        }
        this.skillCancelPoint.gameObject.GetComponent<Image>().color = pointedSkillCancelPointColor;
        this.imgSkillCancelIcon.color = pointedSkillCancelPointColor;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_SkillButton, this.sfxSource);
        this.imgMaskSkillGO.SetActive(false);
    }   

    private void CancelSkill()
    {
        Debug.Log("Cancel Skill");
    }
}

