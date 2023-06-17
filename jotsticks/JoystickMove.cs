using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickMove : Joystick
{
    public RectTransform frame2;
    public RectTransform bg;
    private Vector2 touchPos;

    //��ġ�� ���� �̵� ���̽�ƽ UI�� �������� �ʽ��ϴ�. 
    public override void OnPointerUp(PointerEventData eventData)
    {
        this.input = Vector2.zero;
        this.handle.anchoredPosition = this.touchPos;
        this.frame2.anchoredPosition = this.touchPos;
        this.handle.gameObject.SetActive(false);
        this.frame2.gameObject.SetActive(false);

        this.SetJoystickColor(false);
        this.isDragged = false;
    }


    public override void OnDrag(PointerEventData eventData)
    {
        //��ġ�� ��ġ���� ���̽�ƽ�� �۵��ϵ��� �մϴ�.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.gameObject.GetComponent<RectTransform>(), eventData.position,
            eventData.pressEventCamera, out Vector2 localVector))
        {
            var moveJoystickDir = localVector - this.touchPos;
            
            if (moveJoystickDir.magnitude < this.handleRange)
            {
                this.handle.anchoredPosition = localVector;
            }
            else
            {
                this.handle.anchoredPosition = this.touchPos + moveJoystickDir.normalized * this.handleRange;
            }
            this.input = moveJoystickDir;
        }

        this.SetJoystickColor(true);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        this.handle.gameObject.SetActive(true);

        //��ġ�� ��ġ���� ���̽�ƽ�� �����˴ϴ�.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.gameObject.GetComponent<RectTransform>(), eventData.position,
            eventData.pressEventCamera, out Vector2 localVector))
        {
            this.touchPos = localVector;
            this.handle.anchoredPosition = localVector;
            this.frame.anchoredPosition = localVector;
            this.frame2.anchoredPosition = localVector;            
        }
        this.OnDrag(eventData);
        this.isDragged = true;
    }

    protected override void SetJoystickColor(bool isOnDraged)
    {
        base.SetJoystickColor(isOnDraged);

        Color pointedFrameColor;
        Color pointedFrame2Color;

        if (isOnDraged)
        {
            pointedFrameColor = new Color(255, 0, 0, 0.5f);
            pointedFrame2Color = new Color(255, 0, 0, 0f);
        }
        else
        {
            pointedFrameColor = new Color(255, 255, 255, 0f);
            pointedFrame2Color = new Color(255, 255, 255, 0.5f);
        }

        this.frame.gameObject.GetComponent<Image>().color = pointedFrameColor;
        this.frame2.gameObject.GetComponent<Image>().color = pointedFrame2Color;
    }
}
