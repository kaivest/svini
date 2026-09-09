using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using Random = System.Random;
public class svinCalmBehaviourScript :  svinDatascript
{
    //player
    private Coroutine changeBehaviour;
    private Coroutine raycastToSee;
    private float delay = 1f;
    private WaitForSeconds wait1s;
    public int TemporaryTargetSpawnRadius = 75;
    private Vector3 CurrentTarget;
    void Start()
    {
        wait1s = new WaitForSeconds(delay);
        walkAnimator = GetComponent<svinDatascript>().walkAnimator;
        rb = GetComponent<svinDatascript>().rb;
        restingTimer = GetComponent<svinDatascript>().restingTimer;
        changeBehaviour = StartCoroutine(ChangeBehaviour(GetComponent<svinDatascript>().maxbehaveDelay));
    }

    void Awake()
    {
        GetComponent<svinDatascript>().jumpTrigger.OnEntered += JumpTriggerOnEnter;
        GetComponent<svinDatascript>().visionTrigger.OnEntered += VisionTriggerOnEnter;
        GetComponent<svinDatascript>().visionTrigger.OnExited += VisionTriggerOnExit;
    }


    
    IEnumerator RaycastToSee()
    {
        while (GetComponent<svinDatascript>().playerInRange)
        {
            Vector3 raypos = rb.transform.position;
            raypos.y += 1;
            Ray ray = new Ray(raypos,
                (GetComponent<svinDatascript>().player.position - rb.transform.position).normalized);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.name + " was seen by " + rb.gameObject.name);
                if (hit.transform.gameObject.tag == "player")
                {
                    rb.gameObject.AddComponent<svinAgroBehaviourScript>();
                    Destroy(rb.gameObject.GetComponent<svinCalmBehaviourScript>());
                    Debug.Log(rb.gameObject.name + " is now agressive");
                }

                //Instantiate(GetComponent<svinDatascript>().testColumn, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Debug.Log("svin saw nothing");
            }

            yield return wait1s;
        }

        raycastToSee = null;
    }
    void startRaycasting()
    {
        if (raycastToSee == null)
        {
            raycastToSee = StartCoroutine(RaycastToSee());
        }
    }
    void VisionTriggerOnEnter(Collider other)
    {
        Debug.Log("player1");
        if (enabled)
        {
            if (other.GameObject().CompareTag("player"))
            {
                Debug.Log("player2");
                if (this.gameObject.GetComponent<svinDatascript>().player == null)
                {Debug.Log("player3");
                    this.gameObject.GetComponent<svinDatascript>().player =  other.gameObject.transform;
                    Debug.Log("player4");
                }
                startRaycasting();
                GetComponent<svinDatascript>().playerInRange = true;
            }
        }
    }
    void VisionTriggerOnExit(Collider other)
    {
        if (enabled)
        {
            if (other.GameObject().CompareTag("player"))
            {
                raycastToSee = null;
                GetComponent<svinDatascript>().playerInRange = false;
            }
        }
    }
    
    void JumpTriggerOnEnter(Collider other)
    {

        if (!GetComponent<svinDatascript>().resting)
        {
            Jump(GetComponent<svinDatascript>().jumpStrength, GetComponent<svinDatascript>().rb);
        }
    }

    
    void Update()
    {
        if (GetComponent<svinDatascript>().resting)
        {
        }else
        {
            NormalBehaviourUpdate();
            replayAnimation();
        }
    }
    
    void FixedUpdate()
    {
        if (GetComponent<svinDatascript>().resting) 
        {}else
        {
            NormalBehaviourFixedUpdate();
        }
    }
    
    void NormalBehaviourUpdate()
    {
        replayAnimation();
    }
    void NormalBehaviourFixedUpdate()
    {
        turnTowardsTarget(CurrentTarget, rb, GetComponent<svinDatascript>().rotationSpeed);
        MoveForward(gameObject.GetComponent<svinDatascript>().svinSpeed, rb);
        replayAnimation();
    }
    
    IEnumerator ChangeBehaviour( int chance)
    {
        while (rb != null)
        {
            yield return wait1s;
            GetComponent<svinDatascript>().restingTimer -= 1;
            if (GetComponent<svinDatascript>().restingTimer <= 0)
            {
                resting = !resting;
                GetComponent<svinDatascript>().resting = resting;
                Random ranDelay = new Random();
                GetComponent<svinDatascript>().restingTimer = ranDelay.Next(chance);
                if (!resting)
                {
                    Jump(GetComponent<svinDatascript>().jumpStrength,  GetComponent<svinDatascript>().rb);
                    CurrentTarget = EstablishTempTarget(GetComponent<svinDatascript>().rb); 
                }
            }
        }
    }
    protected Vector3 EstablishTempTarget(Rigidbody rb)
    {
        Vector3 targpos = rb.transform.position;
        Random ran = new Random();
        int change =ran.Next(-TemporaryTargetSpawnRadius, TemporaryTargetSpawnRadius);
        targpos.x += change;
        change =ran.Next(-TemporaryTargetSpawnRadius, TemporaryTargetSpawnRadius);
        targpos.z += change;
        return targpos;
    }


    private void OnDestroy()
    {
        GetComponent<svinDatascript>().jumpTrigger.OnEntered-= JumpTriggerOnEnter;
        GetComponent<svinDatascript>().visionTrigger.OnEntered-= VisionTriggerOnEnter;
        GetComponent<svinDatascript>().visionTrigger.OnExited-= VisionTriggerOnExit;
    }
}


