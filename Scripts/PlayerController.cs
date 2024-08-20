using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    public float speed = 50f;
    public float sensitivity = 2f;
    public float jumpMultiplier = 5f;
    public float gravity = -20f;
    public bool isGrounded;
    public GameObject playerObject;
    public GameObject fishingRod;
    public GameObject fishingCastPoint;
    public GameObject fishingPoint;
    public CharacterController controller;
    public Vector3 velocity;
    public Animator rodAnimator;
    public LineRenderer lineRenderer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        fishingCastPoint = this.transform.Find("Main Camera").Find("FishingRod").Find("CastPoint").gameObject;
        fishingPoint = this.transform.Find("Main Camera").Find("FishingRod").Find("Point").gameObject;
        playerObject = this.transform.Find("Capsule").gameObject;


        //chatgpt help with this part
        lineRenderer = fishingRod.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
    }

    void Update()
    {
        // movement

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


        // gameplay
        if (Input.GetMouseButtonDown(0))
        {
            CastRod();
        }
        lineRenderer.SetPosition(0, fishingCastPoint.transform.position);
        lineRenderer.SetPosition(1, fishingPoint.transform.position);
    }

    public void CastRod()
    {
        rodAnimator.SetTrigger("Cast");
    }
}
