using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICobraRange : AINormalRange
{
    // 애니메이션 clip에 Event로 사용합니다.
    protected override void InstantiateBullet()
    {
        GameObject go;
        float angle = Mathf.Atan2(this.dirPlayer.y, this.dirPlayer.x) * Mathf.Rad2Deg;
        var lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        var pool = MonsterBulletPooler.instance.cobraBulletPool;

        if (pool.Count > 0)
        {
            go = pool.Dequeue();
            if (go.activeSelf) go = Instantiate(this.bulletGo);            
        }
        else
        {
            go = Instantiate(this.bulletGo);
        }

        go.transform.position = this.bulletPoint.transform.position;
        go.SetActive(true);        
        go.transform.rotation = lookRotation;
        go.GetComponent<MonsterBullet>().Init(pool);
    }
}
