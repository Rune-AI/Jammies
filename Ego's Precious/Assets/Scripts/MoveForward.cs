using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        //transform.position += transform.forward * Time.fixedDeltaTime;
        rb.AddForce(transform.forward * 10f, ForceMode.Acceleration);

    }
}
