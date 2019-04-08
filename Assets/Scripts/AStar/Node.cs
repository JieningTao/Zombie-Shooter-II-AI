using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector2 gridPosition;

    public bool blocked;
    public Vector2 position;

    public Node parent;

    public int gCost;
    public int hCost;

    public int FCost { get { return gCost + hCost; } }

    public Node(bool isBlocked, Vector2 postionOfnode, Vector2 positionOfNodeInGrid)
    {
        blocked = isBlocked;
        position = positionOfNodeInGrid;
        gridPosition = positionOfNodeInGrid;
    }
}
