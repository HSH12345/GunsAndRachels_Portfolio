using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoolTime : MonoBehaviour
{
    [System.NonSerialized]
    public float coolTime;
    private float maxCoolTime;
    private Image img;
    private bool isCoolTime;

    public void Init(float coolTime)
    {
        this.img = this.GetComponent<Image>();
        this.gameObject.SetActive(true);
        this.coolTime = coolTime;
        this.maxCoolTime = coolTime;
        this.isCoolTime = true;
    }

    void Update()
    {
        if (this.isCoolTime)
        {
            this.coolTime -= Time.deltaTime;
            this.img.fillAmount = this.coolTime / this.maxCoolTime;

            if (this.coolTime <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
