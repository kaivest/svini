using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using Random = System.Random;
public class svinCalmBehaviourScript : svinDatascript
{
    public GameObject testColumn;
    private Coroutine changeBehaviour;
    private float delay = 1f;
    private WaitForSeconds waiter;
    public int TemporaryTargetSpawnRadius = 75;
    private Vector3 CurrentTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waiter = new WaitForSeconds(delay);
        walkAnimator = GetComponent<svinDatascript>().walkAnimator;
        rb = GetComponent<svinDatascript>().rb;
        player = GetComponent<svinDatascript>().player;
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
            yield return waiter;
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

    protected Vector3 EstablishTempTarget(Rigidbody rb)
    {
        Vector3 targpos = rb.transform.position;
        Random ran = new Random();
        int change =ran.Next(-TemporaryTargetSpawnRadius, TemporaryTargetSpawnRadius);
        targpos.x += change;
        change =ran.Next(-TemporaryTargetSpawnRadius, TemporaryTargetSpawnRadius);
        targpos.z += change;



        Instantiate(testColumn, targpos, rb.transform.rotation );
        Debug.Log("col spawned");
        
        
        return targpos;
    }
}


