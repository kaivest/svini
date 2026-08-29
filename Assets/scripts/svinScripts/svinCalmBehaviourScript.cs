using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using Random = System.Random;
public class svinCalmBehaviourScript :  svinDatascript
{
    
    public GameObject testColumn;
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
        GetComponent<svinDatascript>().jumpTrigger.OnStayed += JumpTriggerOnStay;
        Debug.Log("Awake ");
        
    }

    void JumpTriggerOnStay(Collider other)
    {

        if (!GetComponent<svinDatascript>().resting&& (GetComponent<svinDatascript>().jumpMask.value&(1<< other.gameObject.layer))!=0)
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

    private void OnTriggerStay(Collider other)
    {
        if (enabled)
        {
            if (other.GameObject().tag == "player")
            {
                startRaycasting();
                Debug.Log("svin raycasting");
            }
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
                    Jump(GetComponent<svinDatascript>().jumpStrength,  GetComponent<svinDatascript>().rb);
                    CurrentTarget = EstablishTempTarget(GetComponent<svinDatascript>().rb); 
                }
            }
        }
    }

    IEnumerator RaycastToSee()
    {
        Debug.Log("raycasting tosee");
        Vector3 raypos = rb.transform.position;
        raypos.y += 1;
        Ray ray = new Ray(raypos, (GetComponent<svinDatascript>().player.position -rb.transform.position).normalized);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.gameObject.name + " was seen by "+rb.gameObject.name);
            if (hit.transform.gameObject.tag == "player")
            {
                rb.gameObject.AddComponent<svinAgroBehaviourScript>();
                Destroy(GetComponent<svinDatascript>().jumpTrigger.gameObject.GetComponent<customTrigger>());
                Destroy(rb.gameObject.GetComponent<svinCalmBehaviourScript>());
                Debug.Log(rb.gameObject.name+" is now agressive");
            }
            Instantiate(testColumn, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else
        {
            Debug.Log("svin saw nothing");
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
        if (raycastToSee == null)
        {
            raycastToSee = StartCoroutine(RaycastToSee());
            Debug.Log(name + " raycasting coroutine must have started");
        }
    }
}


