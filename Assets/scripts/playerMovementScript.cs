using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class playerMovementScript : MonoBehaviour
{
    
    public GameObject head;
    public GameObject headPosition;
    public GameObject handAxis;
    public GameObject rightHandWearpon;
    public GameObject plar;
    public static GameObject player;
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject wearpon;
    public GameObject pile;
    public float playerPushForce = 7f;
    public float playerSpeed = 4f;
    public float jumpStrength = 5f;
    public static float playerhp = 100f;
    private bool isGoingForward = false;
    private bool isGoingBack = false;
    public Rigidbody playerRB;
    public float sensivity= 1000f;
    public float mouseX;
    public float mouseY;
    public Camera mainCamera;
    private Coroutine swingCoroutine;
    private Coroutine pileSwingCoroutine;
    public bool movesForward = false;
    public bool movesBackward = false;
    public bool movesRight = false;
    public bool movesLeft = false;
    public bool jumps = false;
    public bool strikes = false;
    public bool PileStrikes = false;
    public bool isFlying = false;
    public bool moves = true;
    public WaitForSeconds wait001 = new WaitForSeconds(0.01f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState=CursorLockMode.Locked;
        rightHandWearpon.GetComponent<BoxCollider>().enabled = false;
        player = plar;

    }

    IEnumerator PileSwingCoroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            wearpon.transform.rotation *= Quaternion.Euler(0, -9, 0);
            yield return wait001;
        }
        for (int i = 0; i < 10; i++)
        {
            Vector3 wearponPos = wearpon.transform.position;
            Vector3 backward = wearpon.transform.up;
            wearponPos = backward*((-15)*Time.deltaTime);
            wearpon.transform.position += wearponPos;
            yield return wait001;
        }
        for (int i = 0; i < 10; i++)
        {
            rightHand.transform.rotation *= Quaternion.Euler(0, 0, -7);
            yield return wait001;
        }
        for (int i = 0; i < 10; i++)
        {
            Vector3 wearponPos = wearpon.transform.position;
            Vector3 backward = wearpon.transform.up;
            wearponPos = backward*((35)*Time.deltaTime);
            wearpon.transform.position += wearponPos;
            yield return wait001;
        }
        for (int i = 0; i < 20; i++)
        {
            rightHand.transform.rotation *= Quaternion.Euler(0, 0, 3.5f);
            yield return wait001;
        }
        Vector3 pilePos =player.transform.position;
        Vector3 Forward = player.transform.right;
        pilePos = pilePos + Forward*4;
        pilePos.y = -2.8f;
        
        Instantiate(pile, pilePos, player.transform.rotation);
        for (int i = 0; i < 10; i++)
        {
            Vector3 wearponPos = wearpon.transform.position;
            Vector3 backward = wearpon.transform.up;
            wearponPos = backward*((-20)*Time.deltaTime);
            wearpon.transform.position += wearponPos;
            yield return wait001;
        }
        for (int i = 0; i < 10; i++)
        {
            wearpon.transform.rotation *= Quaternion.Euler(0, 9, 0);
            yield return wait001;
        }
        Vector3 normalization = wearpon.transform.localPosition;
        normalization.x = 1.1491f;
        normalization.y = 0.784f;
        normalization.z = 0.02f;
        wearpon.transform.localPosition = normalization;
        yield return new WaitForSeconds(0.1f);
             PileStrikes = false;
             pileSwingCoroutine = null;
    }
    IEnumerator SwingCoroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            rightHand.transform.rotation *= Quaternion.Euler(0, 0, -6);
            yield return wait001;
        }

        wearpon.GetComponent<BoxCollider>().enabled = true;
        for (int i = 0; i < 10; i++)
        {
            Vector3 wearponPos = wearpon.transform.position;
            Vector3 forward = wearpon.transform.up;
            wearponPos = forward*((25)*Time.deltaTime);
            wearpon.transform.position += wearponPos;
            yield return wait001;
        }
        for (int i = 0; i < 10; i++)
        {
            Vector3 wearponPos = wearpon.transform.position;
            Vector3 forward = -wearpon.transform.up;
            wearponPos = forward*((25)*Time.deltaTime);
            wearpon.transform.position += wearponPos;
            yield return wait001;
            wearpon.GetComponent<BoxCollider>().enabled = false;
        }
        for (int i = 0; i < 10; i++)
        {
            rightHand.transform.rotation *= Quaternion.Euler(0, 0, 6);
            yield return wait001;
        }

        Vector3 normalization = wearpon.transform.localPosition;
        normalization.x = 1.1491f;
        normalization.y = 0.784f;
        normalization.z = 0.02f;
        wearpon.transform.localPosition = normalization;
        yield return new WaitForSeconds(0.1f);
        swingCoroutine = null;
        strikes = false;
    }
    // Update is called once per frame
    void Update()
    {
        svinAttack.targe = playerRB.transform;
        head.transform.position = headPosition.transform.position;
        head.transform.rotation = headPosition.transform.rotation;
            
        mouseX = Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime; 
        Mathf.Clamp(head.transform.localEulerAngles.z, -90, 90);
        headPosition.transform.rotation *= Quaternion.Euler(0, 0, mouseY);
        transform.rotation*=Quaternion.Euler(0,mouseX,0);
        if (playerRB.transform.position.y > 1.5f)
        {
            isFlying = true;
        }
        if (Input.GetKey("w"))
        {
            movesForward = true;
        }

        if (Input.GetKey("s"))
        {
            movesBackward = true;
        }

        if (Input.GetKey("a"))
        {
            movesRight = true;
        }
        if (Input.GetKey("d"))
        {
            movesLeft = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            strikes = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            PileStrikes = true;
        }
        if (Input.GetKeyDown("space")&&playerRB.transform.position.y <= 1.55f)
        {
            jumps = true;
            isFlying = true;
        }
    }

    private void FixedUpdate()
    {

        if (movesForward)
        {
            Vector3 movement = transform.position;
            Vector3 forwardMov = transform.right;
            movement = forwardMov * (playerSpeed * Time.deltaTime);
            transform.position += movement;
            movesForward = false;
        }
        if (movesBackward)
        {
            Vector3 movement = transform.position;
            Vector3 backwardMov = transform.right;
            movement = backwardMov * (-playerSpeed * Time.deltaTime);
            transform.position += movement;
            movesBackward = false;
        }

        if (movesRight)
        {
            Vector3 movement = transform.position;
            Vector3 backwardMov = transform.forward;
            movement = backwardMov * (playerSpeed * Time.deltaTime);
            transform.position += movement;
            movesRight = false;
        }

        if (movesLeft)
        {
            Vector3 movement = transform.position;
            Vector3 backwardMov = transform.forward;
            movement = backwardMov * (-playerSpeed * Time.deltaTime);
            transform.position += movement;
            movesLeft = false;
        }

        if (jumps)
        {
            playerRB.AddForce(0,jumpStrength,0,ForceMode.Impulse);
            jumps = false;
        }

        if (isFlying == false)
        {
            Vector3 velocity = playerRB.angularVelocity;
            velocity.y = 0;
            velocity.x = 0;
            velocity.z = 0;
            playerRB.angularVelocity = velocity;
        }

        if (strikes)
        {
            if (swingCoroutine == null&& pileSwingCoroutine == null)
            {
                swingCoroutine = StartCoroutine(SwingCoroutine());
            }
        }
        if(PileStrikes)
        {
            if (pileSwingCoroutine == null&&swingCoroutine == null)
            {
                pileSwingCoroutine = StartCoroutine(PileSwingCoroutine());
            }  
        }
        Quaternion noRot = transform.rotation;
        noRot.x = 0;
        noRot.z = 0;
        transform.rotation = noRot;

        Quaternion viewRot = head.transform.rotation;
        handAxis.transform.rotation = viewRot;
    }
}
