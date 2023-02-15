using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFloater : MonoBehaviour
{
    private Rigidbody rigidBody;
    public float depthBeforeSublerged = 0.1f;
    public float displacementAmount = 2f;

    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (rigidBody == null)
        {
            Debug.Log("No ridgid Body found");
        }

    }

        void Update()
    {
        float waveHeight = WaterHeight.instance.GetWaveHeight(new Vector2(transform.position.x, transform.position.z));

        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSublerged) * displacementAmount;  // percentage of displacement

            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
