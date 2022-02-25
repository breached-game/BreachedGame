using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class WaterGrid : MonoBehaviour
{
    private Grid water_grid;
    private GridVertex[,] gridArray;
    private float[,] terrain;
    public int width = 60;
    public int height = 60;
    public int depth = 60;
    private bool inflow;
    private float dx;
    private float cellSize;
    public float inflowRate;
    public float gravity;
    public float dt = 0.05f;
    public Mesh columnMesh;
    private MeshFilter meshFilter;
    public Vector3Int[] inflowLocations;
    private BoxCollider boxCollider;
    public bool run = false;
    public float playerSpeed;

    void Awake()
    {
        boxCollider = gameObject.AddComponent<BoxCollider>();
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh = columnMesh = new Mesh();
        columnMesh.name = "Water Mesh";
        Time.fixedDeltaTime = dt;
        water_grid = gameObject.GetComponent<Grid>();
        boxCollider.center = water_grid.CellToLocal(new Vector3Int(width / 2, height / 2, depth / 2));
        cellSize = water_grid.cellSize[0];
        boxCollider.size = (new Vector3(width, height, depth) * cellSize);
        boxCollider.isTrigger = true;
        //boxCollider.size /= 2;
        dx = cellSize;
        inflow = true;
        gridArray = new GridVertex[width, depth];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                GridVertex column = ScriptableObject.CreateInstance<GridVertex>();
                column.Setup(new Vector2Int(x, z), water_grid.CellToLocal(new Vector3Int(x, 0, z)), 0.0f, cellSize);
                if (x == 0 || z == 0 || x == width - 1 || z == depth - 1)
                {
                    column.boundary = true;
                    column.isVertex = true;
                }
                gridArray[x, z] = column;
            }
        }
        Setup();
    }

    private void Setup()
    {
        int xInflow;
        int yInflow;
        int zInflow;
        int inflowLocationsSize = inflowLocations.Length;

        for (int i = 0; i < inflowLocationsSize; i++)
        {
            xInflow = inflowLocations[i][0];
            yInflow = inflowLocations[i][1];
            zInflow = inflowLocations[i][2];

            gridArray[xInflow, zInflow].Seth(yInflow);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger");
        Debug.Log(other.gameObject.name);
        Vector3Int cellPos = water_grid.LocalToCell(other.gameObject.transform.position - water_grid.transform.position);
        Vector3Int cellWidth = water_grid.LocalToCell(other.gameObject.transform.localScale / 2);
        for (int x = cellPos.x - cellWidth.x + 1; x < cellPos.x + cellWidth.x; x++)
        {
            for (int z = cellPos.z - cellWidth.z + 1; z < cellPos.z + cellWidth.z; z++)
            {
                Debug.Log("x: " + x + " z: " + z);
                if (x < width && z < depth)
                {
                    Debug.Log("x: " + x + " z: " + z);
                    gridArray[x, z].SetH(2 * (cellWidth.y + cellPos.y));
                }
            }
        }
    }

    private float SumInflows(GridVertex currentColumn)
    {
        float iR = 0.0f;
        float iL = 0.0f;
        float iT = 0.0f;
        float iB = 0.0f;
        Vector2Int pos = currentColumn.GetPos();
        iL = gridArray[pos.x - 1, pos.y].GetNewOutflows()[Vector2Int.right];
        iR = gridArray[pos.x + 1, pos.y].GetNewOutflows()[Vector2Int.left];
        iT = gridArray[pos.x, pos.y + 1].GetNewOutflows()[Vector2Int.down];
        iB = gridArray[pos.x, pos.y - 1].GetNewOutflows()[Vector2Int.up];

        return iL + iR + iT + iB;
    }

    public void FixedUpdate()
    {
        float dhL, dhR, dhT, dhB;
        float dt_A_g_l = (dt * Mathf.Pow(dx, 2) * gravity) / dx;
        float K;
        float dV;
        float totalHeight;
        float totalFlux;
        int vCount = 0;
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        Dictionary<Vector2Int, float> tempFlux = new Dictionary<Vector2Int, float>();
        GridVertex currentColumn;

        int xInflow;
        int zInflow;
        int inflowLocationsSize = inflowLocations.Length;
        if (inflow)
        {
            for (int i = 0; i < inflowLocationsSize; i++)
            {
                xInflow = inflowLocations[i][0];
                zInflow = inflowLocations[i][2];
                gridArray[xInflow, zInflow].Seth(gridArray[xInflow, zInflow].Geth() + inflowRate * dt);
            }
        }


        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < depth - 1; z++)
            {
                tempFlux.Clear();
                currentColumn = gridArray[x, z];
                if (currentColumn.Geth() > 0.0f)
                {
                    totalHeight = currentColumn.GetH() + currentColumn.Geth();
                    dhL = totalHeight - gridArray[x - 1, z].GetH() - gridArray[x - 1, z].Geth();
                    dhR = totalHeight - gridArray[x + 1, z].GetH() - gridArray[x + 1, z].Geth();
                    dhT = totalHeight - gridArray[x, z + 1].GetH() - gridArray[x, z + 1].Geth();
                    dhB = totalHeight - gridArray[x, z - 1].GetH() - gridArray[x, z - 1].Geth();

                    tempFlux.Add(Vector2Int.left, Mathf.Max(0.0f, currentColumn.GetOutflows()[Vector2Int.left] + (dt_A_g_l * dhL)));
                    tempFlux.Add(Vector2Int.right, Mathf.Max(0.0f, currentColumn.GetOutflows()[Vector2Int.right] + (dt_A_g_l * dhR)));
                    tempFlux.Add(Vector2Int.up, Mathf.Max(0.0f, currentColumn.GetOutflows()[Vector2Int.up] + (dt_A_g_l * dhT)));
                    tempFlux.Add(Vector2Int.down, Mathf.Max(0.0f, currentColumn.GetOutflows()[Vector2Int.down] + (dt_A_g_l * dhB)));

                    if (x == 1)
                    {
                        tempFlux[Vector2Int.left] = 0.0f;
                    }
                    else if (x == width - 2)
                    {
                        tempFlux[Vector2Int.right] = 0.0f;
                    }
                    if (z == 1)
                    {
                        tempFlux[Vector2Int.down] = 0.0f;
                    }
                    else if (z == depth - 2)
                    {
                        tempFlux[Vector2Int.up] = 0.0f;
                    }

                    totalFlux = tempFlux.Sum(x => x.Value);

                    if (totalFlux > 0.0f)
                    {
                        K = Mathf.Min(1.0f, currentColumn.Geth() * dx * dx / totalFlux / dt);
                        tempFlux[Vector2Int.left] = K * tempFlux[Vector2Int.left];
                        tempFlux[Vector2Int.right] = K * tempFlux[Vector2Int.right];
                        tempFlux[Vector2Int.up] = K * tempFlux[Vector2Int.up];
                        tempFlux[Vector2Int.down] = K * tempFlux[Vector2Int.down];
                    }
                }
                else
                {
                    tempFlux.Add(Vector2Int.left, 0.0f);
                    tempFlux.Add(Vector2Int.right, 0.0f);
                    tempFlux.Add(Vector2Int.up, 0.0f);
                    tempFlux.Add(Vector2Int.down, 0.0f);
                }
                currentColumn.SetNewOutflows(tempFlux);
            }
        }
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < depth - 1; z++)
            {
                currentColumn = gridArray[x, z];
                dV = dt * (SumInflows(currentColumn) - currentColumn.GetNewOutflows().Sum(x => x.Value));
                if (currentColumn.Geth() + dV / (dx * dx) + currentColumn.GetH() >= height)
                {
                    inflow = false;
                    currentColumn.SetNewh(height - currentColumn.GetH() - 1);
                }
                else
                {
                    currentColumn.SetNewh(currentColumn.Geth() + dV / (dx * dx));
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                currentColumn = gridArray[x, z];
                currentColumn.UpdateValues();
                if (currentColumn.isVertex)
                {
                    vertices.Add(currentColumn.GetVertexPosition());
                    currentColumn.vertex = vCount;
                    vCount++;
                }
            }
        }
        columnMesh.Clear();
        columnMesh.SetVertices(vertices);
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                currentColumn = gridArray[x, z];
                if (currentColumn.isVertex)
                {
                    if (x != width - 1 & z != depth - 1)
                    {
                        if (gridArray[x + 1, z].isVertex & gridArray[x, z + 1].isVertex & gridArray[x+1, z+1].isVertex)
                        {
                            triangles.Add(currentColumn.vertex);
                            triangles.Add(gridArray[x, z + 1].vertex);
                            triangles.Add(gridArray[x + 1, z].vertex);
                        }
                    }
                    if (x != 0 & z != depth - 1)
                    {
                        if (gridArray[x - 1, z + 1].isVertex & gridArray[x, z + 1].isVertex & gridArray[x-1, z].isVertex)
                        {
                            triangles.Add(currentColumn.vertex);
                            triangles.Add(gridArray[x - 1, z + 1].vertex);
                            triangles.Add(gridArray[x, z + 1].vertex);
                        }
                    }
                }
            }
        }
        columnMesh.SetTriangles(triangles, 0);
        columnMesh.RecalculateNormals();
    }
}
