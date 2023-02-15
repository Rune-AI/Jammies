using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float downWardForce = 1;

    private Vector2 movementInput;

    private GameObject boat;

    private void Awake()
    {
        boat = transform.parent.gameObject;
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime);

        rigidBody.AddForceAtPosition(Physics.gravity * downWardForce, transform.position, ForceMode.Acceleration);

        //Vector3 moveDirection = boat.transform.TransformDirection(new Vector3(movementInput.x, 0, movementInput.y));
        //moveDirection.y = 0;
        //moveDirection.Normalize();
        //rigidBody.AddForce(moveDirection * moveSpeed * Time.deltaTime);
        //rigidBody.AddForce(new Vector3(0,-downWardForce,0) * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }
}
