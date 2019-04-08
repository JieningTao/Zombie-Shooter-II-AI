using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector2 gridPosition;

    public bool blocked;
    public Vector3 position;

    public Node parent;

    public int gCost;
    public int hCost;

    public int FCost { get { return gCost + hCost; } }

    public Node(bool isBlocked, Vector3 positionOfNode, Vector2 positionOfNodeInGrid)
    {
        blocked = isBlocked;
        position = positionOfNode;
        gridPosition = positionOfNodeInGrid;
    }
}
