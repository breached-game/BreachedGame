using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WaterGridTest
{
    GameObject waterGrid;
    GameObject waterDrops;
    GameObject breachPoint;
    GameObject breachPlane;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        #region WaterGridObjects
        //Instantiating BreachPlane game object
        breachPlane = new GameObject();

        //Instantiating BreachPoint game object
        breachPoint = new GameObject();
        breachPoint.transform.position = new Vector3(3f, 0f, 3f);
        yield return null;

        //Instantiating WaterDrops game object
        waterDrops = new GameObject();
        yield return null;

        //Instantiating WaterGrid game object - needs Water Drops and Breach Point
        waterGrid = new GameObject();
        waterGrid.SetActive(false);
        waterGrid.AddComponent<WaterGrid>();
        waterGrid.AddComponent<Grid>();
        waterGrid.AddComponent<MeshFilter>();
        waterGrid.GetComponent<Grid>().cellSize = new Vector3(1f, 1f, 1f);
        GameObject[] inflowLocations = new GameObject[1];
        inflowLocations[0] = breachPoint;
        waterGrid.GetComponent<WaterGrid>().inflowLocations = inflowLocations;
        waterGrid.GetComponent<WaterGrid>().inflowRate = 0f;
        waterGrid.GetComponent<WaterGrid>().gravity = 4f;
        waterGrid.GetComponent<WaterGrid>().playerSpeed = 3f;
        waterGrid.GetComponent<WaterGrid>().breachPlane = breachPlane;
        #endregion WaterGridObjects
        yield return null;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(waterGrid);
        Object.Destroy(waterDrops);
        Object.Destroy(breachPoint);
        Object.Destroy(breachPlane);
    }

    [UnityTest]
    public IEnumerator WaterGridOneFixedUpdate()
    {
        waterGrid.SetActive(true);
        yield return new WaitForFixedUpdate();
        for (int x = 0; x < waterGrid.GetComponent<WaterGrid>().width; x++)
        {
            for (int z = 0; z < waterGrid.GetComponent<WaterGrid>().depth; z++)
            {
                Assert.AreEqual(0.0f, waterGrid.GetComponent<WaterGrid>().gridArray[x, z].Geth());
            }
        }
    }

    [UnityTest]
    public IEnumerator WaterGridOneFixedUpdateCorrectTerrainHeight()
    {
        waterGrid.SetActive(true);
        yield return new WaitForFixedUpdate();
        for (int x = 0; x < waterGrid.GetComponent<WaterGrid>().width; x++)
        {
            for (int z = 0; z < waterGrid.GetComponent<WaterGrid>().depth; z++)
            {
                Assert.AreEqual(0.0f, waterGrid.GetComponent<WaterGrid>().gridArray[x, z].GetH());
            }
        }
    }

    [UnityTest]
    public IEnumerator WaterGridOutflowsIntialisedCorrectly()
    {
        waterGrid.SetActive(true);
        yield return new WaitForFixedUpdate();
        Dictionary<Vector2Int, float> outflows = new Dictionary<Vector2Int, float>();
        outflows.Add(Vector2Int.right, 0f);
        outflows.Add(Vector2Int.left, 0f);
        outflows.Add(Vector2Int.up, 0f);
        outflows.Add(Vector2Int.down, 0f);
        for (int x = 0; x < waterGrid.GetComponent<WaterGrid>().width; x++)
        {
            for (int z = 0; z < waterGrid.GetComponent<WaterGrid>().depth; z++)
            {
                Assert.AreEqual(outflows, waterGrid.GetComponent<WaterGrid>().gridArray[x, z].GetOutflows());
                Assert.AreEqual(outflows, waterGrid.GetComponent<WaterGrid>().gridArray[x, z].GetNewOutflows());
            }
        }
    }

    [UnityTest]
    public IEnumerator WaterGridNoFixedUpdate()
    {
        waterGrid.SetActive(true);
        yield return new WaitForFixedUpdate();
        for (int x = 0; x < waterGrid.GetComponent<WaterGrid>().width; x++)
        {
            for (int z = 0; z < waterGrid.GetComponent<WaterGrid>().depth; z++)
            {
                Assert.AreEqual(0.0f, waterGrid.GetComponent<WaterGrid>().gridArray[x, z].Geth());
            }
        }
    }

    [UnityTest]
    public IEnumerator BreachPlaneCorrectPosition()
    {
        breachPoint.transform.position = new Vector3(3f, 3f, 3f);
        waterGrid.SetActive(true);
        yield return new WaitForFixedUpdate();
        Assert.AreEqual(new Vector3(breachPoint.transform.position.x, waterGrid.GetComponent<WaterGrid>().gridArray[1, 3].GetVertexPosition().y, breachPoint.transform.position.z), breachPlane.transform.position);
    }

    [Test]
    public void InitialInflowGridArrayHeight()
    {
        breachPoint.transform.position = new Vector3(3f, 3f, 3f);
        waterGrid.SetActive(true);
        Assert.AreEqual(3f, waterGrid.GetComponent<WaterGrid>().gridArray[3, 3].Geth());
    }

    [Test]
    public void AddingWaterPump()
    {
        Vector3 waterPumpPosition = new Vector3(0f, 0f, 0f);
        waterGrid.SetActive(true);
        waterGrid.GetComponent<WaterGrid>().AddWaterPump(waterPumpPosition);
        Assert.AreEqual(true, waterGrid.GetComponent<WaterGrid>().waterFix);
    }

    [Test]
    public void RemoveWaterPump()
    {
        Vector3 waterPumpPosition = new Vector3(0f, 0f, 0f);
        waterGrid.SetActive(true);
        waterGrid.GetComponent<WaterGrid>().AddWaterPump(waterPumpPosition);
        waterGrid.GetComponent<WaterGrid>().RemoveWaterPump(waterPumpPosition);
        Assert.AreEqual(false, waterGrid.GetComponent<WaterGrid>().waterFix);
    }

    [Test]
    public void AddedBoxCollider()
    {
        waterGrid.SetActive(true);
        Assert.AreEqual(true, (waterGrid.GetComponent<BoxCollider>() != null));
    }

    [Test]
    public void CheckBoxColliderSize()
    {
        waterGrid.SetActive(true);
        Assert.AreEqual(new Vector3(waterGrid.GetComponent<WaterGrid>().width, waterGrid.GetComponent<WaterGrid>().height, waterGrid.GetComponent<WaterGrid>().depth), waterGrid.GetComponent<BoxCollider>().size);
    }

    [Test]
    public void CheckBoxColliderIsTrigger()
    {
        waterGrid.SetActive(true);
        Assert.AreEqual(true, waterGrid.GetComponent<BoxCollider>().isTrigger);
    }

    [Test]
    public void CheckVerticesAndBoundaries()
    {
        waterGrid.SetActive(true);
        for (int x = 0; x < waterGrid.GetComponent<WaterGrid>().width; x++)
        {
            Assert.AreEqual(true, waterGrid.GetComponent<WaterGrid>().gridArray[x, 0].isVertex);
            Assert.AreEqual(true, waterGrid.GetComponent<WaterGrid>().gridArray[x, 0].boundary);

            Assert.AreEqual(true, waterGrid.GetComponent<WaterGrid>().gridArray[x, waterGrid.GetComponent<WaterGrid>().depth - 1].isVertex);
            Assert.AreEqual(true, waterGrid.GetComponent<WaterGrid>().gridArray[x, waterGrid.GetComponent<WaterGrid>().depth - 1].boundary);
        }

        for (int z = 0; z < waterGrid.GetComponent<WaterGrid>().depth; z++)
        {
            Assert.AreEqual(true, waterGrid.GetComponent<WaterGrid>().gridArray[0, z].isVertex);
            Assert.AreEqual(true, waterGrid.GetComponent<WaterGrid>().gridArray[0, z].boundary);

            Assert.AreEqual(true, waterGrid.GetComponent<WaterGrid>().gridArray[waterGrid.GetComponent<WaterGrid>().width - 1, z].isVertex);
            Assert.AreEqual(true, waterGrid.GetComponent<WaterGrid>().gridArray[waterGrid.GetComponent<WaterGrid>().width - 1, z].boundary);
        }
    }

    [UnityTest]
    public IEnumerator InflowBeingAdded()
    {
        float dt = waterGrid.GetComponent<WaterGrid>().dt;
        float dt_A_g_l = dt * waterGrid.GetComponent<WaterGrid>().gravity;
        waterGrid.GetComponent<WaterGrid>().inflowRate = 10f;
        waterGrid.SetActive(true);
        yield return new WaitForFixedUpdate();
        //Geth + dv / (dt^2)
        Assert.AreEqual(Mathf.Round(((10f * dt) - (dt * dt_A_g_l * 10f * dt * 4f)) * 100000) / 100000, Mathf.Round(waterGrid.GetComponent<WaterGrid>().gridArray[3, 3].Geth() * 100000) / 100000);
    }

    [UnityTest]
    public IEnumerator WaterInflowFalse()
    {
        waterGrid.GetComponent<WaterGrid>().StopBreach();
        waterGrid.SetActive(true);
        yield return new WaitForFixedUpdate();
        for (int x = 0; x < waterGrid.GetComponent<WaterGrid>().width; x++)
        {
            for (int z = 0; z < waterGrid.GetComponent<WaterGrid>().depth; z++)
            {
                Assert.AreEqual(0.0f, waterGrid.GetComponent<WaterGrid>().gridArray[x, z].Geth());
            }
        }
    }

    [UnityTest]
    public IEnumerator WaterGridSetToInactive()
    {
        Vector3 waterPumpPosition = new Vector3(0f, 0f, 0f);
        waterGrid.SetActive(true);
        waterGrid.GetComponent<WaterGrid>().AddWaterPump(waterPumpPosition);
        yield return new WaitForFixedUpdate();
        Assert.AreEqual(false, waterGrid.activeSelf);
    }

    [UnityTest]
    public IEnumerator OnTriggerEnterWorking()
    {
        GameObject collider = GameObject.CreatePrimitive(PrimitiveType.Cube);
        collider.transform.localScale = new Vector3(2f, 2f, 2f);
        collider.transform.position = new Vector3(10f, 10f, 10f);
        collider.AddComponent<Rigidbody>();
        waterGrid.transform.position = new Vector3(0f, 0f, 0f);
        waterGrid.SetActive(true);
        yield return null;
        Assert.AreEqual(2 * (9f + (collider.transform.localScale.y / 2)), waterGrid.GetComponent<WaterGrid>().gridArray[10, 10].GetH());
        Object.Destroy(collider);
    }

    [UnityTest]
    public IEnumerator OnTriggerExitWorking()
    {
        GameObject collider = GameObject.CreatePrimitive(PrimitiveType.Cube);
        collider.transform.localScale = new Vector3(2f, 2f, 2f);
        collider.transform.position = new Vector3(10f, 10f, 10f);
        collider.AddComponent<Rigidbody>();
        waterGrid.transform.position = new Vector3(0f, 0f, 0f);
        waterGrid.SetActive(true);
        yield return null;
        collider.GetComponent<Rigidbody>().detectCollisions = false;
        yield return new WaitForFixedUpdate();
        Object.Destroy(collider);
        yield return null;
        Assert.AreEqual(0f, waterGrid.GetComponent<WaterGrid>().gridArray[10, 10].GetH());
    }

    [UnityTest]
    public IEnumerator InactiveBreachPlane()
    {
        waterGrid.GetComponent<WaterGrid>().StopBreach();
        waterGrid.SetActive(true);
        yield return null;
        Assert.AreEqual(false, waterGrid.GetComponent<WaterGrid>().breachPlane.activeSelf);
    }

    [UnityTest]
    public IEnumerator OutflowVertexIsCorrect()
    {
        Vector3 waterPumpPosition = new Vector3(2f, 2f, 2f);
        waterGrid.GetComponent<WaterGrid>().inflowRate = 150f;
        waterGrid.SetActive(true);
        waterGrid.GetComponent<WaterGrid>().AddWaterPump(waterPumpPosition);
        yield return new WaitForFixedUpdate();
        Assert.AreEqual(0.2f, waterGrid.GetComponent<MeshFilter>().mesh.vertices[2].y);
    }

    [UnityTest]
    public IEnumerator WaterGridElementsWithWaterAreVertices()
    {
        waterGrid.GetComponent<WaterGrid>().inflowRate = 150f;
        waterGrid.SetActive(true);
        yield return new WaitForFixedUpdate();
        for (int x = 0; x < waterGrid.GetComponent<WaterGrid>().width; x++)
        {
            for (int z = 0; z < waterGrid.GetComponent<WaterGrid>().depth; z++)
            {
                if (waterGrid.GetComponent<WaterGrid>().gridArray[x, z].Geth() > 0f)
                {
                    Assert.AreEqual(true, waterGrid.GetComponent<WaterGrid>().gridArray[x, z].isVertex);
                }
            }
        }
    }
}
