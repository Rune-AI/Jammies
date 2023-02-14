using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatWind : MonoBehaviour
{
    public float windForce = 1f;
    public float windDirection = 0f;
    public float windSpeed = 1f;
    public float windSpeedChange = 0.1f;
    public float windDirectionChange = 0.1f;
    public float windForceChange = 0.1f;
    
    public Rigidbody rigidBody;

    private void Update()
    {
        windSpeed += Random.Range(-windSpeedChange, windSpeedChange);
        windDirection += Random.Range(-windDirectionChange, windDirectionChange);
        windForce += Random.Range(-windForceChange, windForceChange);
    }

    private void FixedUpdate()
    {
        Vector3 wind = new Vector3(Mathf.Cos(windDirection), 0f, Mathf.Sin(windDirection)) * windForce;
        rigidBody.AddForceAtPosition(wind * windSpeed, transform.position, ForceMode.Acceleration);
    }
}
