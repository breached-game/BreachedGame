using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;

public class WaterGrid : MonoBehaviour
{
    public Grid water_grid;
    public GridVertex[,] gridArray;
    public int width = 60;
    public int height = 60;
    public int depth = 60;
    private bool inflow;
    private float dx;
    public float cellSize;
    public float inflowRate;
    public float gravity;
    public float dt = 0.05f;
    public Mesh columnMesh;
    private MeshFilter meshFilter;
    public GameObject[] inflowLocations;
    private List<Vector3Int> outflowLocations = new List<Vector3Int>();
    private int randomIndex;
    private BoxCollider boxCollider;
    public bool run = false;
    public float playerSpeed;
    private bool full = false;
    private float[] savedSpeeds = new float[2];
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();
    private Dictionary<Vector2Int, float> tempFlux = new Dictionary<Vector2Int, float>();
    public Transform waterParticleSystem;
    private Vector3Int playerGridPos;
    public GameObject waterDrops;
    public GameObject[] players;
    private GameObject firstPersonCamera;
    private FogEffects fogController;
    public bool waterFix = false;
    private Vector3Int breachPosition = new Vector3Int();
    private bool muffle = false;
    private Coroutine positionCoroutine;

    void Awake()
    {
        int inflowLocationsSize = inflowLocations.Length;
        System.Random rnd = new System.Random();
        randomIndex = rnd.Next(0, inflowLocationsSize);

        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                firstPersonCamera = player.GetComponent<PlayerManager>().FirstPersonCamera;
                fogController = firstPersonCamera.GetComponent<FogEffects>();
            }
        }
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
                GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
                v.Setup(new Vector2Int(x, z), water_grid.CellToLocal(new Vector3Int(x, 0, z)), 0.0f, cellSize);
                if (x == 0 || z == 0 || x == width - 1 || z == depth - 1)
                {
                    v.boundary = true;
                    v.isVertex = true;
                }
                gridArray[x, z] = v;
            }
        }
        Setup();
    }

    private void Setup()
    {
        int xInflow;
        int yInflow;
        int zInflow;

        breachPosition = water_grid.LocalToCell(inflowLocations[randomIndex].transform.position - water_grid.transform.position);


        xInflow = breachPosition.x;
        yInflow = breachPosition.y;
        zInflow = breachPosition.z;

        gridArray[xInflow, zInflow].Seth(yInflow);
        //Instantiate(waterParticleSystem, water_grid.transform.position + water_grid.CellToLocal(new Vector3Int(breachPosition.x, 0, breachPosition.z)), Quaternion.Euler(new Vector3(0, 0, 180)));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                positionCoroutine = StartCoroutine(CheckPlayerPos(other.gameObject));
            }
        }
        else if (other.gameObject.tag != "Floater")
        {
            Vector3Int cellPos = water_grid.LocalToCell(other.gameObject.transform.position - water_grid.transform.position);
            Vector3Int cellWidth = water_grid.LocalToCell(other.gameObject.transform.localScale / 2);
            if (cellWidth.x < 1)
            {
                cellWidth.x = 1;
            }
            if (cellWidth.z < 1)
            {
                cellWidth.z = 1;
            }
            for (int x = cellPos.x - cellWidth.x + 1; x < cellPos.x + cellWidth.x; x++)
            {
                for (int z = cellPos.z - cellWidth.z + 1; z < cellPos.z + cellWidth.z; z++)
                {
                    if (x < width && z < depth && x > 0 && z > 0)
                    {
                        gridArray[x, z].SetH(2 * (cellWidth.y + cellPos.y));
                    }
                }
            }
        }
    }
    IEnumerator CheckPlayerPos(GameObject player)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        float waterHeight;
        savedSpeeds[0] = playerManager.defaultSpeed;
        savedSpeeds[1] = playerManager.defaultSprintSpeed;
        while (true) {
            playerGridPos = water_grid.LocalToCell(player.transform.position - water_grid.transform.position);
            if (!(playerGridPos.x < 0 || playerGridPos.x > width || playerGridPos.z < 0 || playerGridPos.z > depth))
            {
                try
                {
                    waterHeight = gridArray[playerGridPos.x, playerGridPos.z].GetVertexPosition().y;
                    if (waterHeight > 0)
                    {
                        // put water muffle on here
                        playerManager.Speed = playerSpeed;
                        playerManager.SprintSpeed = playerSpeed;
                        if (firstPersonCamera.transform.position.y < waterHeight + transform.position.y)
                        {
                            fogController.fog = true;
                            waterDrops.SetActive(true);
                        }
                        else
                        {
                            fogController.fog = false;
                            waterDrops.SetActive(false);
                        }
                        if (!muffle && Application.platform == RuntimePlatform.WebGLPlayer)
                        {
                            VoiceWrapper.waterMic();
                            muffle = true;
                        }
                    }
                    else
                    {
                        // put water muffle off here
                        playerManager.Speed = savedSpeeds[0];
                        playerManager.SprintSpeed = savedSpeeds[1];
                        fogController.fog = false;
                        waterDrops.SetActive(false);
                        if (muffle && Application.platform == RuntimePlatform.WebGLPlayer)
                        {
                            VoiceWrapper.waterMic();
                            muffle = false;
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    playerManager.Speed = savedSpeeds[0];
                    playerManager.SprintSpeed = savedSpeeds[1];
                    fogController.fog = false;
                    waterDrops.SetActive(false);
                }
            }
            if (waterFix)
            {
                if (gridArray[breachPosition.x, breachPosition.z].Geth() < 0.1f) {
                    gameObject.SetActive(false);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AddWaterPump(Vector3 position)
    {
        outflowLocations.Add(water_grid.LocalToCell(position - water_grid.transform.position));
        waterFix = true;
    }

    public void RemoveWaterPump(Vector3 position)
    {
        outflowLocations.Remove(water_grid.LocalToCell(position - water_grid.transform.position));
        waterFix = false;
    }

    public void StopBreach()
    {
        inflow = false;
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
        if (other.gameObject.tag == "Player")
        {
            StopCoroutine(positionCoroutine);
            playerManager.ResetSpeed();
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                VoiceWrapper.waterMic();
            }
            
        }
        else if (other.gameObject.tag != "Floater")
        {
            Vector3Int cellPos = water_grid.LocalToCell(other.gameObject.transform.position - water_grid.transform.position);
            Vector3Int cellWidth = water_grid.LocalToCell(other.gameObject.transform.localScale / 2);
            if (cellWidth.x < 1)
            {
                cellWidth.x = 1;
            }
            if (cellWidth.z < 1)
            {
                cellWidth.z = 1;
            }
            for (int x = cellPos.x - cellWidth.x + 1; x < cellPos.x + cellWidth.x; x++)
            {
                for (int z = cellPos.z - cellWidth.z + 1; z < cellPos.z + cellWidth.z; z++)
                {
                    if (x < width && z < depth && x > 0 && z > 0)
                    {
                        gridArray[x, z].SetH(0);
                    }
                }
            }
        }
    }

    private float SumDictionary(Dictionary<Vector2Int, float> d)
    {
        float sum = 0;
        foreach (var x in d)
        {
            sum += x.Value;
        }
        return sum;
    }

    private float SumInflows(GridVertex currentColumn)
    {
        float iR;
        float iL;
        float iT;
        float iB;
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
        Vector3Int inflowPosition = water_grid.LocalToCell(inflowLocations[randomIndex].transform.position - water_grid.transform.position);
        vertices.Clear();
        triangles.Clear();
        tempFlux.Clear();
        uvs.Clear();
        Dictionary<Vector2Int, float> currentOutflows;
        GridVertex currentColumn;

        int xInflow = 0;
        int zInflow = 0;
        int xOutflow;
        int zOutflow;
        int inflowLocationsSize = inflowLocations.Length;
        if (inflow)
        {
            breachPosition = water_grid.LocalToCell(inflowLocations[randomIndex].transform.position - water_grid.transform.position);

            xInflow = breachPosition.x;
            zInflow = breachPosition.z;

            gridArray[xInflow, zInflow].Seth(gridArray[xInflow, zInflow].Geth() + inflowRate * dt);
        }

        for (int i = 0; i < outflowLocations.Count; i++)
        {
            xOutflow = outflowLocations[i].x;
            zOutflow = outflowLocations[i].z;

            if (inflow)
            {
                gridArray[xOutflow, zOutflow].Seth(gridArray[xOutflow, zOutflow].Geth() - inflowRate * dt);
            } else {
                gridArray[xOutflow, zOutflow].Seth(gridArray[xOutflow, zOutflow].Geth() - 2 * inflowRate * dt);
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

                    currentOutflows = currentColumn.GetOutflows();
                    tempFlux.Add(Vector2Int.left, Mathf.Max(0.0f, currentOutflows[Vector2Int.left] + (dt_A_g_l * dhL)));
                    tempFlux.Add(Vector2Int.right, Mathf.Max(0.0f, currentOutflows[Vector2Int.right] + (dt_A_g_l * dhR)));
                    tempFlux.Add(Vector2Int.up, Mathf.Max(0.0f, currentOutflows[Vector2Int.up] + (dt_A_g_l * dhT)));
                    tempFlux.Add(Vector2Int.down, Mathf.Max(0.0f, currentOutflows[Vector2Int.down] + (dt_A_g_l * dhB)));

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

                    totalFlux = SumDictionary(tempFlux);

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
        int vertexCount = columnMesh.vertexCount;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                currentColumn = gridArray[x, z];
                if (x != 0 & x != width - 1 & z != 0 & z != depth - 1)
                {
                    dV = dt * (SumInflows(currentColumn) - SumDictionary(currentColumn.GetNewOutflows()));
                    if (currentColumn.Geth() + dV / (dx * dx) + currentColumn.GetH() >= height && x != xInflow && z != zInflow)
                    {
                        //inflow = false;
                        currentColumn.SetNewh(height - currentColumn.GetH() - 1);
                    }
                    else
                    {
                        currentColumn.SetNewh(currentColumn.Geth() + dV / (dx * dx));
                    }
                    currentColumn.UpdateValues();
                }
                if (currentColumn.isVertex)
                {
                    if (inflowPosition.x == x && inflowPosition.z == z)
                    {
                        vertices.Add(new Vector3(currentColumn.GetVertexPosition().x, gridArray[x - 2, z].GetVertexPosition().y, currentColumn.GetVertexPosition().z));
                    }
                    else { vertices.Add(currentColumn.GetVertexPosition()); }
                    uvs.Add(new Vector2(currentColumn.GetVertexPosition().x, currentColumn.GetVertexPosition().z));
                    currentColumn.vertex = vCount;
                    vCount++;
                    if (x != 0 & z != 0 & !full)
                    {
                        if (gridArray[x, z - 1].isVertex & gridArray[x - 1, z - 1].isVertex & gridArray[x - 1, z].isVertex)
                        {
                            triangles.Add(currentColumn.vertex);
                            triangles.Add(gridArray[x, z - 1].vertex);
                            triangles.Add(gridArray[x - 1, z - 1].vertex);
                            triangles.Add(gridArray[x - 1, z].vertex);
                            triangles.Add(currentColumn.vertex);
                            triangles.Add(gridArray[x - 1, z - 1].vertex);
                        }
                    }
                    if (vertices.Count == vertexCount)
                    {
                        full = true;
                        if (triangles.Count == 0)
                        {
                            foreach (var t in columnMesh.triangles)
                            {
                                triangles.Add(t);
                            }
                        }
                    }
                    else
                    {
                        full = false;
                    }
                }
            }
        }
        columnMesh.Clear();
        columnMesh.SetVertices(vertices);
        columnMesh.SetTriangles(triangles, 0);
        columnMesh.SetUVs(0,uvs);
        columnMesh.RecalculateNormals();
        columnMesh.RecalculateTangents();
        columnMesh.Optimize();
    }
}
