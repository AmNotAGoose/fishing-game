using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public float jumpMultiplier = 5f;
    public float gravity = -10f;
    public bool isGrounded;
    public GameObject playerObject;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerObject = this.transform.Find("Capsule").gameObject;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small value to keep the player grounded
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpMultiplier * -2f * gravity);
        } 

        velocity.y += gravity * Time.deltaTime;

        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * speed * Time.deltaTime);


        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X"));
        Camera.main.transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * sensitivity);
        
        controller.Move(velocity * Time.deltaTime);
    }
}
