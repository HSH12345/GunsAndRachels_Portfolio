using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthDirector : MonoBehaviour
{
    public GameObject[] healthGOs;
    public GameObject[] extraHeartGOs;
    [SerializeField]
    private Material whiteMaterial;
    private Material originMaterial;

    private float blinkDelay = 0.1f;
    private Coroutine blinkRoutine;

    public void Init()
    {
        this.originMaterial = this.healthGOs[0].GetComponent<Image>().material;
    }
    public void BlinkHelathBar()
    {
        if (this.blinkRoutine != null) StopCoroutine(this.blinkRoutine);
        this.blinkRoutine = StartCoroutine(this.BlinkRoutine());
    }

    // 메터리얼을 교체하여 blink 효과를 만듭니다.
    private IEnumerator BlinkRoutine()
    {
        for(int i = 0; i < this.healthGOs.Length; i++)
        {
            this.healthGOs[i].GetComponent<Image>().material = this.whiteMaterial;
        }
        
        yield return new WaitForSeconds(this.blinkDelay);

        for (int i = 0; i < this.healthGOs.Length; i++)
        {
            this.healthGOs[i].GetComponent<Image>().material = this.originMaterial;
        }
    }
}
