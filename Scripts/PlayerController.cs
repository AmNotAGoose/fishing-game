using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using TMPro;

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
    public bool fishing = false;
    public bool caughtFish = true;
    public FishingManager fishingManager;
    public GameObject curFish;

    public TextMeshProUGUI totalCaughtFish;
    public TextMeshProUGUI score;
    public TextMeshProUGUI curCaughtFish;
    public TextMeshProUGUI curScore;
    public TextMeshProUGUI curDescription;
    public GameObject curFishPanel;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerObject = this.transform.Find("Capsule").gameObject;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        if (Input.GetMouseButtonDown(0) && !fishing)
        {
            CastRod();
        }

        if (Input.GetMouseButtonDown(1) && fishing)
        {
            CancelFishing();
        }
        if (fishing)
        {
            lineRenderer.SetPosition(0, fishingCastPoint.transform.position);
        } else
        {
            lineRenderer.SetPosition(0, fishingCastPoint.transform.position);
            lineRenderer.SetPosition(1, fishingCastPoint.transform.position);
        }
    }

    public void CastRod()
    {
        rodAnimator.SetTrigger("Cast");

        Vector3 randomPoint = fishingCastPoint.transform.position + transform.forward * UnityEngine.Random.Range(10f, 50f);
        fishingPoint.transform.position = new Vector3(randomPoint.x, -1, randomPoint.z);
        lineRenderer.SetPosition(1, fishingPoint.transform.position);

        StartCoroutine(WaitForFish());
    }

    public IEnumerator WaitForFish()
    {
        fishing = true;
        caughtFish = true;
        
        float waitTime = UnityEngine.Random.Range(0, 30);
        float passedTime = 0f;

        while (passedTime < waitTime && fishing)
        {
            passedTime += Time.deltaTime;
            yield return null;
        }

        if (caughtFish)
        {
            StartCoroutine(ShowEndScreen());
        }

        fishing = false;
    }

    public void CancelFishing()
    {
        fishing = false;
        caughtFish = false;
        StopCoroutine(WaitForFish());
    }

    public IEnumerator ShowEndScreen()
    {
        Fish randomFish = fishingManager.GetRandomFish();
        curFish = Instantiate(randomFish.obj, transform.position + transform.forward * 2f, Quaternion.identity);
        totalCaughtFish.SetText("Total Caught Fish: " + (Int32.Parse(totalCaughtFish.text.Split(" ")[3]) + 1).ToString());
        score.SetText("Total $$$: " + (Int32.Parse(score.text.Split(" ")[2]) + randomFish.value).ToString());
        curCaughtFish.SetText(randomFish.name);
        curScore.SetText("$" + randomFish.value.ToString());
        curDescription.SetText(randomFish.description);
        curFishPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        curFishPanel.SetActive(false);
    }
}
