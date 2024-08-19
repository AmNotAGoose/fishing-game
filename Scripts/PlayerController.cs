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

    private Rigidbody rb;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        rb.MovePosition(transform.position + move * speed * Time.deltaTime);

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X"));
        Camera.main.transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * sensitivity);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
        }

        velocity.y += gravity * Time.deltaTime;

        rb.AddForce(Vector3.up * velocity.y);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            velocity.y = -2f;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
