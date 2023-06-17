using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 localPosiotion;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    // 길 되추적을 위한 parent변수.
    public Node parent;

    // F cost 계산 속성.
    public int fCost{ get { return gCost + hCost;} }

    // Node 생성자.
    public Node(bool walkable, Vector2 localPos, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.localPosiotion = localPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}
