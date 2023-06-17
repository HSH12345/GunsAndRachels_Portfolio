using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExpSlideDirector : MonoBehaviour
{
    public Slider sliderExp;
    public Text txtExp;
    public Image imgFill;

    private float blinkDelay = 0.1f;

    private Material originMaterial;
    [SerializeField]
    private Material whiteMaterial;

    private Coroutine blinkRoutine;

    public void Init()
    {
        this.originMaterial = this.imgFill.material;
    }

    public void BlinkEXPFill()
    {
        if (this.blinkRoutine != null) StopCoroutine(this.blinkRoutine);
        this.blinkRoutine = StartCoroutine(this.BlinkRoutine());
    }

    // 메터리얼을 교체하여 blink효과를 만듭니다.
    private IEnumerator BlinkRoutine()
    {
        this.imgFill.material = this.whiteMaterial;
        yield return new WaitForSeconds(this.blinkDelay);
        this.imgFill.material = this.originMaterial;
    }
}
