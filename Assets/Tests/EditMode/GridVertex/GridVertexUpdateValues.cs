using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridVertexUpdateValues
{
    // A Test behaves as an ordinary method
    [Test]
    public void GridVertexUpdateValuesCheckDepth()
    {
        GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
        v.Setup(new Vector2Int(0, 0), new Vector3(0f, 0f, 0f), 0f, 1f);

        v.SetNewh(1f);
        v.UpdateValues();
        Assert.AreEqual(1f, v.Geth());
    }

    [Test]
    public void GridVertexUpdateValuesCheckOutflows()
    {
        GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
        v.Setup(new Vector2Int(0, 0), new Vector3(0f, 0f, 0f), 0f, 1f);

        Dictionary<Vector2Int, float> newOutflows = new Dictionary<Vector2Int, float>();
        newOutflows.Add(Vector2Int.right, 0f);
        newOutflows.Add(Vector2Int.left, 0f);
        newOutflows.Add(Vector2Int.up, 0f);
        newOutflows.Add(Vector2Int.down, 0f);

        v.UpdateValues();
        Assert.AreEqual(newOutflows, v.GetOutflows());
    }

    [Test]
    public void GridVertexUpdateValuesCheckVertex()
    {
        GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
        v.Setup(new Vector2Int(0, 0), new Vector3(0f, 0f, 0f), 0f, 1f);

        v.SetNewh(0f);
        v.UpdateValues();
        Assert.AreEqual(false, v.isVertex);
    }

    [Test]
    public void GridVertexUpdateValuesCheckNotVertex()
    {
        GridVertex v = ScriptableObject.CreateInstance<GridVertex>();
        v.Setup(new Vector2Int(0, 0), new Vector3(0f, 0f, 0f), 0f, 1f);

        v.SetNewh(1f);
        v.UpdateValues();
        Assert.AreEqual(true, v.isVertex);
    }
}
