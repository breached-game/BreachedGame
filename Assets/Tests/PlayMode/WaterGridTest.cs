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

    [UnitySetUp]
    public IEnumerator SetUp()
    {
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
        yield return null;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(waterGrid);
        Object.Destroy(waterDrops);
        Object.Destroy(breachPoint);
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
    public IEnumerator WaterGridNoFixedUpdate()
    {
        yield return null;
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
}
