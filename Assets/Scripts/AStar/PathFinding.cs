using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    

    [SerializeField]
    private Transform startPosition;
    [SerializeField]
    private Transform targetposition;


    Grid grid;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(new Vector2( startPosition.position.x,startPosition.position.y), new Vector2(targetposition.position.x, targetposition.position.y));
    }

    private void FindPath(Vector2 a_startPos, Vector2 a_targetPos)
    {
        Node startNode = grid.NodeFromWorldPos(a_startPos);
        Node targetNode = grid.NodeFromWorldPos(a_targetPos);


        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            Node currentNode = OpenList[0];

            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < currentNode.FCost || OpenList[i].FCost == currentNode.FCost && OpenList[i].hCost < currentNode.hCost)
                    currentNode = OpenList[i];
            }
            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                GetFinalPath(startNode,targetNode);
                return;
            }

            foreach (Node NeighborNode in grid.GetNeighborNodes(currentNode))
            {
                if (NeighborNode.blocked || ClosedList.Contains(NeighborNode))
                    continue;
                int MoveCost = currentNode.gCost + GetManhattenDistance(currentNode, NeighborNode);

                if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = MoveCost;
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, targetNode);
                    NeighborNode.parent = currentNode;

                    if (!OpenList.Contains(NeighborNode))
                        OpenList.Add(NeighborNode);
                }
            }
        }



    }

    private void GetFinalPath(Node a_startnode, Node a_endNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node currentNode = a_endNode;

        while (currentNode != a_startnode)
        {
            FinalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        //FinalPath.Add(new Node(false,a_endNode.position,a_endNode.gridPosition));


        FinalPath.Reverse();

        grid.FinalPath = FinalPath;
    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = (int)Mathf.Abs(a_nodeA.gridPosition.x - a_nodeB.gridPosition.x);
        int iy = (int)Mathf.Abs(a_nodeA.gridPosition.y - a_nodeB.gridPosition.y);

        return ix + iy;
    }
}
