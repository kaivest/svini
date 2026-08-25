using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using Random = System.Random;
public class svinCalmBehaviourScript : svinDatascript
{
    public GameObject testColumn;
    private Coroutine changeBehaviour;
    private Coroutine raycastToSee;
    private float delay = 1f;
    private WaitForSeconds wait1s;
    public int TemporaryTargetSpawnRadius = 75;
    private Vector3 CurrentTarget;
    public bool playerInRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wait1s = new WaitForSeconds(delay);
        walkAnimator = GetComponent<svinDatascript>().walkAnimator;
        rb = GetComponent<svinDatascript>().rb;
        restingTimer = GetComponent<svinDatascript>().restingTimer;
        changeBehaviour = StartCoroutine(ChangeBehaviour(GetComponent<svinDatascript>().maxbehaveDelay));
    }

    // Update is called once per frame
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

   void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "floor"&&!GetComponent<svinDatascript>().resting)
        {
            Jump(GetComponent<svinDatascript>().jumpStrength);
        }
    }

    void FixedUpdate()
    {
        if (GetComponent<svinDatascript>().resting) 
        {}else
        {
            NormalBehaviourFixedUpdate();
        }
        startRaycasting();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GameObject().tag == "player")
        {
            playerInRange = true;
        }
    }


    void NormalBehaviourUpdate()
    {
        replayAnimation();
       
    }
    void NormalBehaviourFixedUpdate()
    {
        fixRotation();
        turnTowardsTarget(CurrentTarget);
        MoveForward();
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
                Debug.Log("behaviour of svin changed");
                if (!resting)
                {
                    Jump(GetComponent<svinDatascript>().jumpStrength);
                    CurrentTarget = EstablishTempTarget(GetComponent<svinDatascript>().rb); 
                }
            }
        }
    }

    IEnumerator RaycastToSee()
    {
        Debug.Log("raycastToSee coroutine started");
        Vector3 raypos = rb.transform.position;
        raypos.y += 1;
        Ray ray = new Ray(raypos, (GetComponent<svinDatascript>().player.position -rb.transform.position).normalized);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.gameObject.name + " was seen by svin from coroutine");
            Instantiate(testColumn, hit.point, Quaternion.LookRotation(hit.normal));
        }
        yield return wait1s;
        raycastToSee = null;
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

    void startRaycasting()
    {
        if (raycastToSee == null && playerInRange)
        {
            raycastToSee = StartCoroutine(RaycastToSee());

        }
    }
}


