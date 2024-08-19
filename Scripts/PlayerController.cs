using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 50f;
    public float sensitivity = 2f;
    public float jumpMultiplier = 5f;
    public float gravity = -20f;
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

        Vector3 move = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        controller.Move(move * speed * Time.deltaTime);


        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X"));
        Camera.main.transform.Rotate(Vector3.left * Input.GetAxisRaw("Mouse Y") * sensitivity);
        
        controller.Move(velocity * Time.deltaTime);
    }
}
