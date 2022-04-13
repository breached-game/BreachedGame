using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridVertexInitialisationTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void DepthInitialisingCorrectly()
    {
        GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
        v.Setup(new Vector2Int(0, 0), new Vector3(0f, 0f, 0f), 0f, 1f);
        Assert.AreEqual(0f, v.Geth());
    }

    [Test]
    public void TerrainInitialisingCorrectly()
    {
        GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
        v.Setup(new Vector2Int(0, 0), new Vector3(0f, 0f, 0f), 0f, 1f);
        Assert.AreEqual(0f, v.GetH());
    }

    [Test]
    public void CellPositionInitialisingCorrectly()
    {
        GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
        v.Setup(new Vector2Int(0, 0), new Vector3(0f, 0f, 0f), 0f, 1f);
        Assert.AreEqual(new Vector2Int(0, 0), v.GetPos());
    }

    [Test]
    public void OutflowsInitialisingCorrectly()
    {
        GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
        v.Setup(new Vector2Int(0, 0), new Vector3(0f, 0f, 0f), 0f, 1f);
        Dictionary<Vector2Int, float> outflows = new Dictionary<Vector2Int, float>();
        outflows.Add(Vector2Int.right, 0f);
        outflows.Add(Vector2Int.left, 0f);
        outflows.Add(Vector2Int.up, 0f);
        outflows.Add(Vector2Int.down, 0f);

        Assert.AreEqual(outflows, v.GetOutflows());
    }

    [Test]
    public void NewOutflowsInitialisingCorrectly()
    {
        GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
        v.Setup(new Vector2Int(0, 0), new Vector3(0f, 0f, 0f), 0f, 1f);
        Dictionary<Vector2Int, float> newOutflows = new Dictionary<Vector2Int, float>();
        newOutflows.Add(Vector2Int.right, 0f);
        newOutflows.Add(Vector2Int.left, 0f);
        newOutflows.Add(Vector2Int.up, 0f);
        newOutflows.Add(Vector2Int.down, 0f);

        Assert.AreEqual(newOutflows, v.GetNewOutflows());
    }

    [Test]
    public void VertexPositionInitialisingCorrectly_0()
    {
        GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
        v.Setup(new Vector2Int(0, 0), new Vector3(0f, 0f, 0f), 0f, 1f);

        Assert.AreEqual(new Vector3(0f, 0f, 0f), v.GetVertexPosition());
    }

    [Test]
    public void VertexPositionInitialisingCorrectly_1()
    {
        GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
        v.Setup(new Vector2Int(0, 0), new Vector3(1f, 1f, 1f), 0.5f, 1f);

        Assert.AreEqual(new Vector3(1f, 1.25f, 1f), v.GetVertexPosition());
    }
}
