using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickSkillDir : Joystick
{
    //스킬 취소관련
    public RectTransform skillCancelPoint;
    public Image imgSkillCancelIcon;
    private float distSkillCancelPoint;
    //스킬 쿨타임 관련
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
        //스킬 취소 아이콘과 거리를 확인하여 스킬을 사용합니다.
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
        //스킬이 버튼으로 사용된다면 조준 ui를 끕니다.
        if (this.isButton) this.frame.gameObject.SetActive(false);
        else this.frame.gameObject.SetActive(true);

        //스킬 취소 ui와 터치 포인트의 거리를 계산하여 ui의 색상을 지정합니다.
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

