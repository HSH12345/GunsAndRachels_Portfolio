using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEditor;

public class DamagePopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro txtDmg;
    [SerializeField]
    private AnimationClip clip;
    [SerializeField]
    [Range(0, 1)]
    private float value;
    private float moveSpeed;    

    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;
    public Vector3 p4;

    public void Init(Vector2 pos, int damage, bool isCritial)
    {
        this.txtDmg.gameObject.SetActive(true);
        this.value = 0;
        this.moveSpeed = 2.5f;
        this.txtDmg.text = damage.ToString();
        this.gameObject.transform.position = pos;

        this.p1 = Vector3.zero;
        this.p2 = Vector3.zero;
        this.p3 = Vector3.zero;
        this.p4 = Vector3.zero;

        if (isCritial)
        {
            this.txtDmg.color = Color.red;
            this.txtDmg.gameObject.GetComponent<Transform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
        else 
        {
            this.txtDmg.color = Color.black;
            this.txtDmg.gameObject.GetComponent<Transform>().localScale = new Vector3(0.07f, 0.07f, 0.07f);
        } 

        float p3XDice = Random.Range(1f, 3f);
        float p3YDice = Random.Range(3.5f, 5.5f);
        float p4XDice = Random.Range(1f, 3f);
        float p4YDice = Random.Range(-0.5f, -1.5f);

        int dirDice = Random.Range(0, 2);
        if (dirDice == 0)
        {
            p3XDice *= -1;
            p4XDice *= -1;            
        }

        this.p3 = new Vector3(p3XDice, p3YDice, 0);
        this.p4 = new Vector3(p4XDice, p4YDice, 0);

        this.p1 += this.transform.position;
        this.p2 += this.transform.position;
        this.p3 += this.transform.position;
        this.p4 += this.transform.position;

        Invoke("DisableText", this.clip.length);
    }

    private void OnDisable()
    {
        var pool = MonsterBulletPooler.instance;
        pool.damagePopupPool.Enqueue(this.gameObject);
    }

    private void FixedUpdate()
    {
        // 랜덤으로 지정된 포인트들을 기준으로 베지어곡선을 그리며 이동합니다.
        this.txtDmg.gameObject.transform.position = Bezier(this.p1, this.p2, this.p3, this.p4, this.value);
        this.value += this.moveSpeed * Time.deltaTime;
        this.moveSpeed *= 0.99f;
    }

    public Vector3 Bezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float value)
    {
        Vector3 a = Vector3.Lerp(p1, p2, value);
        Vector3 b = Vector3.Lerp(p2, p3, value);
        Vector3 c = Vector3.Lerp(p3, p4, value);

        Vector3 d = Vector3.Lerp(a, b, value);
        Vector3 e = Vector3.Lerp(b, c, value);

        Vector3 f = Vector3.Lerp(d, e, value);

        return f;
    }

    private void DisableText()
    {
        this.gameObject.SetActive(false);
    }
}

// 에디터로 곡선을 확인할 때 사용합니다.
//[CanEditMultipleObjects]
//[CustomEditor(typeof(DamagePopup))]

//public class DamagePopupEditor: Editor
//{
//    private void OnSceneGUI()
//    {
//        DamagePopup generator = (DamagePopup) target;

//        generator.p1 = Handles.PositionHandle(generator.p1, Quaternion.identity);
//        generator.p2 = Handles.PositionHandle(generator.p2, Quaternion.identity);
//        generator.p3 = Handles.PositionHandle(generator.p3, Quaternion.identity);
//        generator.p4 = Handles.PositionHandle(generator.p4, Quaternion.identity);

//        Handles.DrawLine(generator.p1, generator.p2);
//        Handles.DrawLine(generator.p3, generator.p4);

//        for(int i =0; i < 10; i++)
//        {
//            float valueBefore = (float)i / 10;
//            float valueAfter = (float)(i + 1) / 10;
//            Vector3 before = generator.Bezier(generator.p1, generator.p2, generator.p3, generator.p4, valueBefore);
//            Vector3 after = generator.Bezier(generator.p1, generator.p2, generator.p3, generator.p4, valueAfter);

//            Handles.DrawLine(before, after);        }
//    }
//}
