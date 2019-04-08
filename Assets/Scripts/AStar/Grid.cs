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












    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
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

                grid[x, y] = new Node(Wall, worldPoint, new Vector2(x, y));
                


            }
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(Vector3.zero, Vector3.one);
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0.2f));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                if (!node.blocked)
                    Gizmos.color = Color.white;
                else
                    Gizmos.color = Color.yellow;

                if (FinalPath != null)
                    Gizmos.color = Color.red;

                

                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - Distance));

            }
        }

    }

}
