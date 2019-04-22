using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Transform StartPosition;
    [SerializeField]
    public LayerMask ObstacleMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float Distance;

    Node[,] grid;

    public List<Node> FinalPath;


    float nodeDiameter;
    int gridSizeX, gridSizeY;
    private Transform parentTransform;












    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        parentTransform = GetComponentInParent<Transform>();
    }

    private void FixedUpdate()
    {
        //CreateGrid();
    }

    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = parentTransform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        Debug.Log(bottomLeft);
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool Wall = false;

                if (Physics2D.OverlapCircle(worldPoint, nodeRadius, ObstacleMask)!=null)
                {
                    Wall = true;
                }

                //Debug.Log(worldPoint);
                grid[x, y] = new Node(Wall, worldPoint, new Vector2(x, y));

                
                //Debug.Log(grid[x,y].position);
            }
        }
        
    }

    public Node NodeFromWorldPos(Vector2 a_WorldPosition)
    {
        int x = Mathf.RoundToInt((gridSizeX - 1) * Mathf.Clamp01(((a_WorldPosition.x - parentTransform.position.x ) + gridWorldSize.x / 2) / gridWorldSize.x));
        int y = Mathf.RoundToInt((gridSizeY - 1) * Mathf.Clamp01(((a_WorldPosition.y - parentTransform.position.y ) + gridWorldSize.y / 2) / gridWorldSize.y));

        return grid[x, y];
    }

    public List<Node> GetNeighborNodes(Node a_Node)
    {
        List<Node> NeighborNodes = new List<Node>();
        int xCheck;
        int yCheck;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                xCheck = (int)a_Node.gridPosition.x+i;
                yCheck = (int)a_Node.gridPosition.y+j;
                if (i == 0 && j == 0)
                {

                }
                else
                {
                    if (xCheck >= 0 && xCheck < gridSizeX)
                    {
                        if (yCheck >= 0 && yCheck < gridSizeY)
                        {
                            NeighborNodes.Add(grid[xCheck, yCheck]);
                        }
                    }
                }
            }
        }

        return NeighborNodes;
    }


    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(Vector3.zero, Vector3.one);
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0.2f));
        Gizmos.DrawCube(new Vector3(-15.5f,-15.5f,0), Vector3.one * (nodeDiameter - Distance));
        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Color boxColor = new Color();
                if (!node.blocked)
                    boxColor = Color.cyan;
                else
                    boxColor = Color.yellow;

                if (FinalPath != null )
                {
                    if (FinalPath.Contains(node))
                    {
                        boxColor = Color.red;
                    }
                    //boxColor = Color.red;
                }
                    

                Gizmos.color = boxColor;

                Gizmos.DrawWireCube(node.position, Vector3.one * (nodeDiameter - Distance));

                boxColor.a = 0.5f;
                Gizmos.color = boxColor;
                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - Distance));

            }
            
        }

        
        
        
    }

}
