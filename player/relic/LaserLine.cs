using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLine : Relic
{
    [SerializeField]
    private float rayDist = 15;
    public GameObject laser;
    private List<LineRenderer> lasers;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject laserPoint;
    [SerializeField]
    private GameObject[] laserPoints;

    private Vector3[] arrLaserPos;

    private bool playerOnPortal;

    public override void Init()
    {
        base.Init();
        this.arrLaserPos = new Vector3[2];
        this.lasers = PlayerObjectPooler.instance.laserPool;
        this.CreateLasers();
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.LaserLineInactivateLaser, this.InactivateLaser);
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.LaserLineActivateLaser, this.ActivateLaser);
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveListener(EventDispatcher.EventName.LaserLineInactivateLaser, this.InactivateLaser);
        EventDispatcher.Instance.RemoveListener(EventDispatcher.EventName.LaserLineActivateLaser, this.ActivateLaser);
    }

    // 이벤트를 이용해 플레이어가 포탈에 있을 때 레이저를 비활성화 합니다.
    public void InactivateLaser()
    {
        foreach (var laser in this.lasers)
        {
            laser.gameObject.SetActive(false);
        }

        this.playerOnPortal = true;
    }

    public void ActivateLaser()
    {
        foreach (var laser in this.lasers)
        {
            laser.gameObject.SetActive(true);
        }
        this.playerOnPortal = false;
    }

    private void Update()
    {
        this.Show();
    }

    private void CreateLasers()
    {
        int maxBulletAmount = 10;

        if(this.lasers.Count == 0)
        {
            for (int i = 0; i < maxBulletAmount; i++)
            {
                var go = Instantiate(this.laser);
                LineRenderer lineRenderer = go.GetComponent<LineRenderer>();
                lineRenderer.startWidth = 0.05f;
                lineRenderer.endWidth = 0.05f;
                lineRenderer.sortingLayerName = "Monster";
                lineRenderer.sortingOrder = 0;
                this.lasers.Add(lineRenderer);
                go.SetActive(false);
            }
        }
    }

    // GunShell과 같은 로직으로 작동합니다.
    public void Show()
    {
        int bulletAmount = InfoManager.instance.charactoristicInfo.bulletAmountCharacteristic;
        if (bulletAmount >= 10) bulletAmount = 10;

        float rotationZ = 0;
        int bulletCnt = 0;
        bool isEven = false;

        switch (bulletAmount)
        {
            case 1:
                bulletCnt = 1;
                break;
            case 2:
                bulletCnt = 2;
                isEven = true;
                break;
            case 3:
                rotationZ = 20;
                bulletCnt = bulletAmount;
                break;
            case 4:
                rotationZ = 20;
                bulletCnt = bulletAmount - 2;
                isEven = true;
                break;
            case 5:
                rotationZ = 40;
                bulletCnt = bulletAmount;
                break;
            case 6:
                rotationZ = 40;
                bulletCnt = bulletAmount - 2;
                isEven = true;
                break;
            case 7:
                rotationZ = 60;
                bulletCnt = bulletAmount;
                break;
            case 8:
                rotationZ = 60;
                bulletCnt = bulletAmount - 2;
                isEven = true;
                break;
            case 9:
                rotationZ = 80;
                bulletCnt = bulletAmount;
                break;
            case 10:
                rotationZ = 80;
                bulletCnt = bulletAmount - 2;
                isEven = true;
                break;
            default:
                break;
        }

        foreach (var laser in this.lasers)
        {
            laser.gameObject.SetActive(false);
        }

        int laserIndex = 0;
        if (isEven)
        {
            for (int i = 0; i < this.laserPoints.Length; i++)
            {
                var lineRenderer = this.lasers[laserIndex];
                lineRenderer.transform.rotation = this.gunShell.lookRotation;
                if (!this.playerOnPortal) lineRenderer.gameObject.SetActive(true);
                else lineRenderer.gameObject.SetActive(false);

                if (bulletAmount == 2) this.arrLaserPos[i] = this.gunShell.bulletPoints[i].transform.position;
                else this.arrLaserPos[i] = this.laserPoints[i].transform.position;

                RaycastHit2D hit = Physics2D.Raycast(this.arrLaserPos[i], lineRenderer.transform.right, this.rayDist, ~this.layerMask);

                if (hit.collider != null)
                {
                    // 충돌이 발생한 경우, 충돌 지점까지의 길이만큼 렌더링합니다.
                    lineRenderer.SetPosition(0, this.arrLaserPos[i]);
                    lineRenderer.SetPosition(1, hit.point);
                }
                else
                {
                    // 충돌이 발생하지 않은 경우, rayDist 길이만큼 렌더링합니다.
                    lineRenderer.SetPosition(0, this.arrLaserPos[i]);
                    lineRenderer.SetPosition(1, this.arrLaserPos[i] + lineRenderer.transform.right * rayDist);
                }

                laserIndex++;
            }
        }

        for (int i = 0; i < bulletCnt; i++)
        {
            if (bulletAmount == 2) continue;
            var lineRenderer = this.lasers[laserIndex];
            var rotation = Quaternion.Euler(0, 0, rotationZ);
            if (!this.playerOnPortal) lineRenderer.gameObject.SetActive(true);
            else lineRenderer.gameObject.SetActive(false);

            lineRenderer.transform.rotation = this.gunShell.lookRotation * rotation;
            if (isEven && rotationZ == 20) rotationZ = 0;
            rotationZ -= 20;

            RaycastHit2D hit = Physics2D.Raycast(this.laserPoint.transform.position, lineRenderer.transform.right, rayDist, ~this.layerMask);

            if (hit.collider != null)
            {
                lineRenderer.SetPosition(0, this.laserPoint.transform.position);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.SetPosition(0, this.laserPoint.transform.position);
                lineRenderer.SetPosition(1, this.laserPoint.transform.position + lineRenderer.transform.right * rayDist);
            }

            laserIndex++;
        }

    }
}
