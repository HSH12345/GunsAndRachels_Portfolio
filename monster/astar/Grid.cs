using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    // 플레이어의 위치
    public Transform player;
    // 장애물 레이어
    public LayerMask Obstacle;
    public LayerMask ObstacleWater;
    // 화면의 크기
    public Vector2 gridLocalSize;
    // 반지름
    public float nodeRadius;
    Node[,] grid;

    // 격자의 지름
    float nodeDiameter;
    // x,y축 사이즈
    int gridSizeX, gridSizeY;

    public void Init()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").transform;
        this.nodeDiameter = this.nodeRadius * 2;
        this.gridSizeX = Mathf.RoundToInt(this.gridLocalSize.x / this.nodeDiameter);
        this.gridSizeY = Mathf.RoundToInt(this.gridLocalSize.y / this.nodeDiameter);
        // 격자 생성
        this.CreateGrid();
    }

    // A*에서 사용할 PATH.
    [SerializeField]
    public List<Node> path;

    // Scene view 출력용 기즈모.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector2(this.gridLocalSize.x, this.gridLocalSize.y));
        if (this.grid != null)
        {
            Node playerNode = NodeFromLocalPoint(this.transform.InverseTransformPoint(this.player.transform.position));
            foreach (Node n in this.grid)
            {
                Gizmos.color = (n.walkable) ? new Color(1, 1, 1, 0.3f) : new Color(1, 0, 0, 0.3f);
                if (n.walkable == false)

                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Gizmos.color = new Color(0, 0, 0, 0.3f);
                            Debug.Log("?");
                        }
                    }
                if (playerNode == n) Gizmos.color = new Color(0, 1, 1, 0.3f);
                Gizmos.DrawCube(n.localPosiotion, Vector2.one * (this.nodeDiameter - 0.1f));
            }
        }
    }

    // 격자 생성 함수
    void CreateGrid()
    {
        this.grid = new Node[gridSizeX, gridSizeY];
        // 격자 생성은 좌측 최하단부터 시작. transform은 월드 중앙에 위치한다. 
        // 이에 x와 y좌표를 반반 씩 왼쪽, 아래쪽으로 옮겨준다.
        Vector2 localBottomLeft = (Vector2)this.transform.position - Vector2.right * this.gridLocalSize.x / 2 - Vector2.up * this.gridLocalSize.y / 2;

        for (int x = 0; x < this.gridSizeX; x++)
        {
            for (int y = 0; y < this.gridSizeY; y++)
            {
                Vector2 localPoint = localBottomLeft + Vector2.right * (x * this.nodeDiameter + this.nodeRadius) + Vector2.up * (y * this.nodeDiameter + this.nodeRadius);
                // 해당 격자가 Walkable한지 아닌지 판단.
                bool walkable = !Physics2D.OverlapCircle(localPoint, this.nodeRadius, this.Obstacle)
                    && !Physics2D.OverlapCircle(localPoint, this.nodeRadius, this.ObstacleWater);

                   // 노드 할당.
                this.grid[x, y] = new Node(walkable, localPoint, x, y);
            }
        }
    }

    // node 상하 좌우 대각 노드를 반환하는 함수.
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    if (!this.grid[node.gridX, checkY].walkable && !this.grid[checkX, node.gridY].walkable) continue;
                    if (!this.grid[node.gridX, checkY].walkable || !this.grid[checkX, node.gridY].walkable) continue;

                    neighbours.Add(this.grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    // 입력으로 들어온 로컬좌표를 node좌표계로 변환.
    public Node NodeFromLocalPoint(Vector2 localPosition)
    {
        float percentX = (localPosition.x + this.gridLocalSize.x / 2) / this.gridLocalSize.x;
        float percentY = (localPosition.y + this.gridLocalSize.y / 2) / this.gridLocalSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((this.gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((this.gridSizeY - 1) * percentY);

        return this.grid[x, y];
    }
}
