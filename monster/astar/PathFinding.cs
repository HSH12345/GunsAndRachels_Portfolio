using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class PathFinding : MonoBehaviour
{ 
    public GameObject target;
    // 맵을 격자로 분할한다.
    [System.NonSerialized]
    public Grid grid;
    // 남은거리를 넣을 큐 생성.
    public Queue<Vector2> wayQueue;
    private Coroutine findPathRoutine;

    public Animator anim;
    public Monster monster;

    //rigidbody
    protected Rigidbody2D rBody2D;

    // 플레이어 이동/회전 속도 등 저장할 변수
    public float moveSpeed;
    public float normalMoveSpeed;
    protected float tmpSpeed;
    // 장애물/NPC 판단시 멈추게 할 범위
    public float eyeRange;
    public float attackRange;
    //타겟과 위치값 비교
    protected float dist;
    public Vector2 dir;
    protected float dirX;

    [System.NonSerialized]
    public bool endInit;
    public bool pathSuccess;

    //길 막힘 관련
    private Vector2 prevPos;
    private float lastMovedTime;
    protected bool isStopForAttack;
    protected bool isBlocked;
    private Coroutine resetBlockedStateRoutine;

    public virtual void Init(Transform player)
    {
        if(this.grid == null) this.grid = GameObject.FindObjectOfType<Grid>().gameObject.GetComponent<Grid>();
        if (this.rBody2D == null) this.rBody2D = gameObject.GetComponent<Rigidbody2D>();
        this.target = player.gameObject;
        this.tmpSpeed = this.moveSpeed;
        this.stun = 1;
        this.moveSpeed = this.normalMoveSpeed;

        //초기화
        this.wayQueue = new Queue<Vector2>();
        this.dir = Vector2.zero;
        this.isFindingPath = false;
        this.pathSuccess = false;
        this.endInit = true;

        //길막힘 관련
        this.isStopForAttack = false;
        this.isBlocked = false;
        this.prevPos = transform.position;
        this.lastMovedTime = Time.time;
    }

    protected virtual void FixedUpdate()
    {
        if(this.endInit) this.StartFindPath((Vector2)this.transform.position, (Vector2)this.target.transform.position);
        this.dist = Vector2.Distance(this.transform.position, this.target.transform.position);

        //스프라이트 플립
        this.FlipMonster();

        if (this.isKnockBack && !this.noKnockback)
        {
            this.KnockBack(this.knockBackSpeed);
        }

        this.CheckForMovement();
    }

    // start to target 이동.
    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        if (this.pathSuccess == true)
        {
            // wayQueue를 따라 이동시킨다.
            if (this.wayQueue.Count > 0)
            {
                this.dir = (Vector2)this.wayQueue.First() - (Vector2)this.transform.position;

                //길은 찾았으나 collider가 통과 못할 때 대응
                if (this.isBlocked) 
                {
                    //bool ranDice = Random.value > 0.5f;
                    //int xDice = ranDice ? 0 : 1;
                    //int yDice = ranDice ? 1 : 0;

                    //this.dir = new Vector2(this.dir.x * xDice, this.dir.y * yDice);

                    float randomAngle = Random.Range(0f, 360f);
                    this.dir = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

                    if (this.resetBlockedStateRoutine == null) this.resetBlockedStateRoutine = StartCoroutine(this.ResetIsBlockedAfterDelay(0.5f));
                } 

                //길찾기 조건 - 탐색 범위
                if (this.dist <= this.eyeRange)
                {
                    this.rBody2D.velocity = this.dir.normalized * this.moveSpeed * Time.deltaTime * this.stun;
                }
                if ((Vector2)this.transform.position == this.wayQueue.First())
                {
                    this.wayQueue.Dequeue();
                }
            }
        }

        if (this.isFindingPath) return;
        //this.isFindingPath = true;

        if (this.findPathRoutine != null) this.StopCoroutine(this.findPathRoutine);
        this.findPathRoutine = this.StartCoroutine(this.FindPath(startPos, targetPos));
    }

    //길막힘 관련
    private IEnumerator ResetIsBlockedAfterDelay(float delay)
    {
        //Debug.LogFormat("{0} Monster start to Find Random Direction", this.gameObject.name);
        yield return new WaitForSeconds(delay);
        this.isBlocked = false;
        this.resetBlockedStateRoutine = null;
    }

    // 길찾기 로직.
    public IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {   
        // start, target의 좌표를 grid로 분할한 좌표로 지정.
        Node startNode = grid.NodeFromLocalPoint(this.grid.transform.InverseTransformPoint(startPos));
        Node targetNode = grid.NodeFromLocalPoint(this.grid.transform.InverseTransformPoint(targetPos));

        bool startNodeOriginalWalkable = startNode.walkable;
        bool targetNodeOriginalWalkable = targetNode.walkable;

        startNode.walkable = true;
        targetNode.walkable = true;

        // target에 도착했는지 확인하는 변수.
        this.pathSuccess = false;

        if (targetNode.walkable)
        {
            // openSet, closedSet 생성.
            // closedSet은 이미 계산 고려한 노드들.
            // openSet은 계산할 가치가 있는 노드들.
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            if (!startNode.walkable)
            {
                closedSet.Add(startNode);
            }
            else
            {
                openSet.Add(startNode);
            }

            // closedSet에서 가장 최저의 F를 가지는 노드를 빼낸다. 
            while (openSet.Count > 0)
            {
                // currentNode를 계산 후 openSet에서 빼야 한다.
                Node currentNode = openSet[0];
                // 모든 openSet에 대해, current보다 f값이 작거나, h(휴리스틱)값이 작으면 그것을 current로 지정.
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                        currentNode = openSet[i];
                }
                // openSet에서 current를 뺀 후, closedSet에 추가.
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // 방금 들어온 노드가 목적지 인 경우
                if (currentNode == targetNode)
                {
                    // seeker가 위치한 지점이 target이 아닌 경우
                    if(this.pathSuccess == false)
                    {
                       // wayQueue에 PATH를 넣어준다.
                       PushWay(RetracePath(startNode, targetNode) ) ;
                    }
                    this.pathSuccess = true;
                    break;
                }

                // current의 상하좌우 노드들에 대하여 g,h cost를 고려한다.
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                    // F cost 생성.
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    // 이웃으로 가는 F cost가 이웃의 G보다 짧거나, 방문해볼 Openset에 그 값이 없다면,
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        // openSet에 추가.
                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    }
                }
            }
        }

        startNode.walkable = startNodeOriginalWalkable;
        targetNode.walkable = targetNodeOriginalWalkable;

        this.isFindingPath = false;
        yield return null;
    }

    // WayQueue에 새로운 PATH를 넣어준다.
    private void PushWay(Vector2[] array)
    {
        this.wayQueue.Clear();
        foreach (Vector2 item in array) this.wayQueue.Enqueue(item);
    }

    // 현재 큐에 거꾸로 저장되어있으므로, 역순으로 wayQueue를 뒤집어준다. 
    private Vector2[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode  = endNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        // Grid의 path에 찾은 길을 등록한다.
        this.grid.path = path;
        Vector2[] wayPoints = SimplifyPath(path);
        return wayPoints;
    }

    // Node에서 Vector 정보만 빼낸다.
    private Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> wayPoints = new List<Vector2>();

        for(int i = 0; i < path.Count; i++)
        {
            //var way = this.grid.transform.parent.TransformPoint(path[i].localPosiotion);
            wayPoints.Add(path[i].localPosiotion);
        }
        return wayPoints.ToArray();
    }

    // custom g cost 또는 휴리스틱 추정치를 계산하는 함수.
    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        // 대각선 - 14, 상하좌우 - 10.
        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    //몬스터와 플레이어의 위치의 x값을 비교하여 몬스터를 플립합니다.
    protected virtual void FlipMonster()
    {
        this.dirX = this.transform.position.x - this.target.transform.position.x;
        if (dirX > 0) this.transform.rotation = Quaternion.Euler(0, 180, 0);
        else this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void CheckForMovement()
    {
        if (!this.isStopForAttack)
        {
            float distMoved = Vector2.Distance(this.gameObject.transform.position, this.prevPos);
            if (distMoved > 0)
            {
                this.isBlocked = false;
                this.lastMovedTime = Time.time;
                this.prevPos = this.gameObject.transform.position;
            }
            else
            {
                if (Time.time - this.lastMovedTime >= 0.5f)
                {
                    this.isBlocked = true;
                }
            }
        }
    }
}
