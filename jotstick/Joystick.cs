using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    //조이스틱 UI
    public RectTransform frame;
    public RectTransform handle;
    public bool isButton;

    //조이스틱 범위
    protected float handleRange = 130;
    //조이스틱 위치
    protected Vector3 input;
    //조이스틱 드래그 상태
    protected bool isDragged;   

    //조이스틱 위치값
    public float Horizontal { get { return this.input.x; } }
    public float Vertical { get { return this.input.y; } }
    public Vector3 Input { get { return this.input; } }
    //조이스틱 드래그 상태
    public bool IsDraged { get { return this.isDragged; } }

    public void Init()
    { 

    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        //드래그된 스크린 좌표값을 localVector 변수에 저장합니다.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.gameObject.GetComponent<RectTransform>(), eventData.position,
            eventData.pressEventCamera, out Vector2 localVector))
        {
            //vector의 길이가 지정한 범위를 초과하는지 체크합니다.
            if (localVector.magnitude < this.handleRange)
            {
                this.handle.transform.localPosition = localVector;
            }
            else //초과했을 때 vector 값을 고정합니다.
            {
                this.handle.transform.localPosition = localVector.normalized * this.handleRange;
            }

            this.input = localVector;
        }

        this.SetJoystickColor(true);
    }

    //드래그그 끝났을 때 위치정보와 핸들을 0으로 정의합니다.
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

    //조이스틱이 클릭되었을 때 색상을 지정합니다.
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

