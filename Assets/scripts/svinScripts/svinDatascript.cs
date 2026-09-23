using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;


public class svinDatascript : entityData
{
    public Transform player;
    public int saloCount;
    public GameObject salo;
    public GameObject testColumn;
    public LayerMask jumpMask;
    public LayerMask visionMask;
    public customTrigger jumpTrigger;
    public customTrigger visionTrigger;
    public customTrigger damageTrigger;
    public Animator walkAnimator;
    public Rigidbody rb;
    public int jumpStrength = 100;
    public WaitForSeconds wait001 = new WaitForSeconds(0.01f);
    public float rotationSpeed = 0.01f;
    public float agroRotationSpeed = 0.02f;
    public int svinSpeed = 5;
    public int svinAgroSpeed = 7;
    public int maxbehaveDelay=15;
    public bool resting = true;
    public bool svinSeesYou = false;
    public int restingTimer = 5;
    public bool playerInRange = false;

    void Awake()
    {
        GetComponent<svinDatascript>().visionTrigger.Mask = GetComponent<svinDatascript>().visionMask;
        GetComponent<svinDatascript>().jumpTrigger.Mask = GetComponent<svinDatascript>().jumpMask;
        GetComponent<svinDatascript>().damageTrigger.Mask = EnemydamagerMask;
    }


    private void Start()
    {
        Debug.Log("svinDatascript Start");

            player = GameObject.FindGameObjectWithTag("player").transform;
            if (player != null)
            {
                Debug.Log("player found");
            }

            

    }
    
    protected void Jump(int js, Rigidbody rb)
    {
       rb.AddForce(0,js,0, ForceMode.Impulse); 
    }

    protected void turnTowardsTarget(Vector3 targetPos, Rigidbody rb, float rotationSpeed)
    {
        Vector3 direction = (targetPos - rb.transform.position).normalized;
        direction.y = 0;
        if (Vector3.Angle(rb.transform.right, direction) > 5f)
        {
            rb.transform.right =
                Vector3.RotateTowards(rb.transform.right, -direction, rotationSpeed, 0.02f * Time.deltaTime);
        }
    }

    protected void replayAnimation()
    {
       AnimatorStateInfo state = walkAnimator.GetCurrentAnimatorStateInfo(0);
       if (state.normalizedTime >=1f)
       {
           walkAnimator.Play("Scene", 0, 0f);
           Debug.Log("something played");
       }
    }


    protected void MoveForward(int speed, Rigidbody rb)
    {
        Vector3 pos = rb.transform.position;
        Vector3 forward = -rb.transform.right;
        pos = forward*(speed*Time.deltaTime);
        rb.transform.position += pos;
    }

    void reduceVelocity(Rigidbody rb)
    {
        if (rb.linearVelocity.magnitude > 4)
        {
            rb.linearVelocity *= 0.99f;
        }
    }

    void Update()
    {
        reduceVelocity(rb);
    }

    
}


