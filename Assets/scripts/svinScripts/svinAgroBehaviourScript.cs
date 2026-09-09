using System;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

public class svinAgroBehaviourScript : svinDatascript
{
    private Coroutine forgetCoroutine;
    public int timeToForget = 15;
    [SerializeField] private int timeLeftToForget = 15;
    private WaitForSeconds wait1s =  new WaitForSeconds(1);
    void Awake()
    {
        walkAnimator = GetComponent<svinDatascript>().walkAnimator;
        GetComponent<svinDatascript>().jumpTrigger.OnEntered += JumpTriggerOnEnter;
        GetComponent<svinDatascript>().visionTrigger.OnEntered += VisionTriggerOnEnter;
        GetComponent<svinDatascript>().visionTrigger.OnExited += VisionTriggerOnExit;
        Debug.Log(GetComponent<svinDatascript>().jumpTrigger.enabled + " is datascript jumptrigger status for "+ name);
    }
    void JumpTriggerOnEnter(Collider other)
    {
            Jump(GetComponent<svinDatascript>().jumpStrength, GetComponent<svinDatascript>().rb);
    }

    void VisionTriggerOnEnter(Collider other)
    {
        if (enabled)
        {
            if (other.GameObject().tag == "player")
            {
                GetComponent<svinDatascript>().playerInRange = true;
            }
        }
    }

    void VisionTriggerOnExit(Collider other)
    {
        if (enabled)
        {
            if (other.GameObject().tag == "player")
            {
                GetComponent<svinDatascript>().playerInRange = false;
            }
        }
    }
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
       replayAnimation(); 
       startForgetCoroutine();
    }

    void FixedUpdate()
    {
        turnTowardsTarget(gameObject.GetComponent<svinDatascript>().player.position, gameObject.GetComponent<svinDatascript>().rb, GetComponent<svinDatascript>().agroRotationSpeed);
        MoveForward(gameObject.GetComponent<svinDatascript>().svinAgroSpeed,  GetComponent<svinDatascript>().rb);
    }

    void startForgetCoroutine()
    {
        if (forgetCoroutine == null)
        {
            forgetCoroutine = StartCoroutine(ForgetCoroutine());
        }
    }
    private IEnumerator ForgetCoroutine()
    {
        yield return wait1s;
        Debug.Log(GetComponent<svinDatascript>().rb.name+" started forgetting you");
        timeLeftToForget = timeToForget;
        while (forgetCoroutine != null)
        {
            yield return wait1s;
            Debug.Log("while cycle");
            Vector3 raypos = GetComponent<svinDatascript>().rb.transform.position;
            raypos.y += 1;
            Ray ray = new Ray(raypos,
                (GetComponent<svinDatascript>().player.position - GetComponent<svinDatascript>().rb.transform.position).normalized);
            RaycastHit hit = new RaycastHit();
            if (GetComponent<svinDatascript>().playerInRange)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    //Instantiate(GetComponent<svinDatascript>().testColumn, hit.point, Quaternion.LookRotation(hit.normal));
                    if (hit.collider.gameObject.CompareTag("player"))
                    {
                        timeLeftToForget = timeLeftToForget+1;
                    }
                }
            }
            timeLeftToForget--;
            if (timeLeftToForget <= 0)
            {
                Debug.Log(name+" forgot about your existence");
                GetComponent<svinDatascript>().rb.gameObject.AddComponent<svinCalmBehaviourScript>();
                Destroy(GetComponent<svinDatascript>().rb.gameObject.GetComponent<svinAgroBehaviourScript>());
                forgetCoroutine = null;
                
            }
            Debug.Log(timeLeftToForget+" seckonds is left to forget you for " +GetComponent<svinDatascript>().rb.name);
        }
    }

    private void OnDestroy()
    {
        GetComponent<svinDatascript>().jumpTrigger.OnEntered -= JumpTriggerOnEnter;
        GetComponent<svinDatascript>().visionTrigger.OnEntered -= VisionTriggerOnEnter;
        GetComponent<svinDatascript>().visionTrigger.OnExited -= VisionTriggerOnExit;
    }

    void dash()
    {
        
    }

    void rushStraight()
    {
        
    }
    
}
