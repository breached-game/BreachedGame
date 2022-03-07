using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public WaterGrid waterGrid;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 1f;

    public void FixedUpdate()
    {
        if (transform.position.y < waterGrid.GetComponent<WaterGrid>().GetWaterHeight(transform.position) + waterGrid.transform.position.y)
        {
            float displacementMultiplier = Mathf.Clamp01((-transform.position.y + waterGrid.GetComponent<WaterGrid>().GetWaterHeight(transform.position) + waterGrid.transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
            print("CHECK");
        }
    }
}
