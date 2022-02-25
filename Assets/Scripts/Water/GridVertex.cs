using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVertex : ScriptableObject
{
    private Vector2Int position;
    private Vector3 localPosition;
    private float cellSize;
    private float h;
    private float H;
    private float newh;
    private Dictionary<Vector2Int, float> newOutflows;
    private Dictionary<Vector2Int, float> outflows;
    public int vertex;
    public bool isVertex = false;
    public bool boundary = false;
    public void Setup(Vector2Int arg_position, Vector3 arg_localPosition, float arg_H, float arg_cellSize)
    {
        position = arg_position;
        localPosition = arg_localPosition;
        cellSize = arg_cellSize;
        H = arg_H;
        h = 0f;
        outflows = new Dictionary<Vector2Int, float>();
        outflows.Add(Vector2Int.right, 0f);
        outflows.Add(Vector2Int.left, 0f);
        outflows.Add(Vector2Int.up, 0f);
        outflows.Add(Vector2Int.down, 0f);
        newOutflows = new Dictionary<Vector2Int, float>();
        newOutflows.Add(Vector2Int.right, 0f);
        newOutflows.Add(Vector2Int.left, 0f);
        newOutflows.Add(Vector2Int.up, 0f);
        newOutflows.Add(Vector2Int.down, 0f);
    }
    public Vector2Int GetPos()
    {
        return position;
    }

    public void UpdateValues()
    {
        h = newh;
        if (h > 0 || boundary)
        {
            isVertex = true;
        }
        else
        {
            isVertex = false;
        }
        outflows = newOutflows;
    }

    public void SetNewh(float arg_h)
    {
        if (arg_h < 0)
        {
            newh = 0;
        }
        else
        {
            newh = arg_h;
        }
    }

    public float Geth()
    {
        return h;
    }

    public void Seth(float arg_h)
    {
        h = arg_h;
    }

    public float GetH()
    {
        return H;
    }
    public Dictionary<Vector2Int, float> GetNewOutflows()
    {
        return newOutflows;
    }

    public Dictionary<Vector2Int, float> GetOutflows()
    {
        return outflows;
    }

    public void SetNewOutflows(Dictionary<Vector2Int, float> fs)
    {
        newOutflows[Vector2Int.right] = fs[Vector2Int.right];
        newOutflows[Vector2Int.left] = fs[Vector2Int.left];
        newOutflows[Vector2Int.up] = fs[Vector2Int.up];
        newOutflows[Vector2Int.down] = fs[Vector2Int.down];
    }

    public void SetOutflows(Dictionary<Vector2Int, float> fs)
    {
        outflows = fs;
    }

    public Vector3 GetVertexPosition()
    {
        return new Vector3(localPosition.x, localPosition.y + (cellSize * (h + H)) / 2, localPosition.z);
    }

    public void SetH(float arg_H)
    {
        H = arg_H;
    }
}
