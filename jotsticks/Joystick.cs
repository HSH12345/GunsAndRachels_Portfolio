using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    //���̽�ƽ UI
    public RectTransform frame;
    public RectTransform handle;
    public bool isButton;

    //���̽�ƽ ����
    protected float handleRange = 130;
    //���̽�ƽ ��ġ
    protected Vector3 input;
    //���̽�ƽ �巡�� ����
    protected bool isDragged;   

    //���̽�ƽ ��ġ��
    public float Horizontal { get { return this.input.x; } }
    public float Vertical { get { return this.input.y; } }
    public Vector3 Input { get { return this.input; } }
    //���̽�ƽ �巡�� ����
    public bool IsDraged { get { return this.isDragged; } }

    public void Init()
    { 

    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        //�巡�׵� ��ũ�� ��ǥ���� localVector ������ �����մϴ�.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.gameObject.GetComponent<RectTransform>(), eventData.position,
            eventData.pressEventCamera, out Vector2 localVector))
        {
            //vector�� ���̰� ������ ������ �ʰ��ϴ��� üũ�մϴ�.
            if (localVector.magnitude < this.handleRange)
            {
                this.handle.transform.localPosition = localVector;
            }
            else //�ʰ����� �� vector ���� �����մϴ�.
            {
                this.handle.transform.localPosition = localVector.normalized * this.handleRange;
            }

            this.input = localVector;
        }

        this.SetJoystickColor(true);
    }

    //�巡�ױ� ������ �� ��ġ������ �ڵ��� 0���� �����մϴ�.
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        this.input = Vector2.zero;
        this.handle.anchoredPosition = Vector2.zero;

        this.SetJoystickColor(false);
        this.isDragged = false;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        this.OnDrag(eventData);
        this.isDragged = true;
    }

    //���̽�ƽ�� Ŭ���Ǿ��� �� ������ �����մϴ�.
    protected virtual void SetJoystickColor(bool isOnDraged)
    {
        Color pointedFrameColor;
        Color pointedHandleColor;

        if (isOnDraged)
        {
            pointedFrameColor = new Color(255, 0, 0, 0.5f);
            pointedHandleColor = new Color(255, 0, 0, 0.6f);
        }
        else
        {
            pointedFrameColor = new Color(255, 255, 255, 0.5f);
            pointedHandleColor = new Color(255, 255, 255, 0.6f);
        }

        this.frame.gameObject.GetComponent<Image>().color = pointedFrameColor;
        this.handle.gameObject.GetComponent<Image>().color = pointedHandleColor;
    }
}

