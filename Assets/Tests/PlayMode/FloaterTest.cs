using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FloaterTest
{
    GameObject floater;
    GameObject floatingObject;
    GameObject waterGrid;
    GameObject waterDrops;
    GameObject breachPoint;
    GameObject breachPlane;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        #region FloaterObjects
        //Instantiating waterGrid
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

        //Instantiating floating object
        floatingObject = new GameObject();
        floatingObject.AddComponent<Rigidbody>();
        floatingObject.GetComponent<Rigidbody>().useGravity = false;
        floatingObject.GetComponent<Rigidbody>().mass = 5f;
        floatingObject.GetComponent<Rigidbody>().drag = 5f;
        floatingObject.GetComponent<Rigidbody>().drag = 5f;
        floatingObject.GetComponent<Rigidbody>().angularDrag = 2f;


        //Instantiating Floater
        floater = new GameObject();
        floater.transform.SetParent(floatingObject.transform);
        floater.SetActive(false);
        floater.AddComponent<Floater>();
        floater.GetComponent<Floater>().rigidBody = floatingObject.GetComponent<Rigidbody>();
        floater.GetComponent<Floater>().water = waterGrid;
        floater.GetComponent<Floater>().floaterCount = 1;
        floater.GetComponent<Floater>().dt = 0.05f;
        #endregion FloaterObjects
        yield return null;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(floater);
        Object.Destroy(floatingObject);
        Object.Destroy(waterGrid);
        Object.Destroy(waterDrops);
        Object.Destroy(breachPoint);
        Object.Destroy(breachPlane);
    }

    [UnityTest]
    public IEnumerator GravityTestYDirection()
    {
        floatingObject.GetComponent<Rigidbody>().mass = 1f;
        floatingObject.GetComponent<Rigidbody>().drag = 0f;
        waterGrid.SetActive(true);
        floater.SetActive(true);
        yield return new WaitForFixedUpdate();
        Assert.AreEqual(Mathf.Round(Physics.gravity.y * 0.05f * 100000f) / 100000f, Mathf.Round(floatingObject.GetComponent<Rigidbody>().velocity.y * 100000f) / 100000f);
    }

    [UnityTest]
    public IEnumerator GravityTestXAndZDirection()
    {
        waterGrid.SetActive(true);
        floater.SetActive(true);
        yield return new WaitForFixedUpdate();
        Assert.AreEqual(0f, floatingObject.GetComponent<Rigidbody>().velocity.x);
        Assert.AreEqual(0f, floatingObject.GetComponent<Rigidbody>().velocity.z);
    }

    [UnityTest]
    public IEnumerator AddingForceWhenUnderWater()
    {
        floatingObject.transform.position = new Vector3(3f, 0f, 3f);
        waterGrid.GetComponent<WaterGrid>().inflowRate = 10f;
        waterGrid.SetActive(true);
        floater.SetActive(true);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        //I.e. Force is greater than gravity causing the object to move up.
        Assert.AreEqual(true, floatingObject.GetComponent<Rigidbody>().velocity.y > 0f);
    }

    [UnityTest]
    public IEnumerator GravityBeingAppliedWhenAboveWater()
    {
        floatingObject.transform.position = new Vector3(3f, 0f, 3f);
        waterGrid.SetActive(true);
        floater.SetActive(true);
        yield return new WaitForFixedUpdate();
        Assert.AreEqual(true, floatingObject.GetComponent<Rigidbody>().velocity.y < 0f);
    }
}
