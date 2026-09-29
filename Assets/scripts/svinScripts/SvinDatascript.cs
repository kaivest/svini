using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using Random = System.Random;

public enum BehaviourType
{
    Common,
    Agressive
}
public class SvinDatascript : entityData
{
    public Action CurrentUpdate;
    public Action CurrentFixedUpdate;
    public BehaviourType behaviour = BehaviourType.Common;
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
    public float rotationSpeed = 0.01f;
    public float agroRotationSpeed = 0.02f;
    public int svinSpeed = 5;
    public int svinAgroSpeed = 7;
    public int maxbehaveDelay=15;
    public bool resting = true;
    public bool svinSeesYou;
    public int restingTimer = 5;
    public bool playerInRange;
    
    
    //calm
    private Coroutine changeBehaviour;
    private Coroutine raycastToSee;
    private WaitForSeconds wait1S =  new WaitForSeconds(1);
    public int temporaryTargetSpawnRadius = 75;
    private Vector3 currentTarget;
    
    
    //agro
    private Coroutine forgetCoroutine;
    public int timeToForget = 15;
    [SerializeField] private int timeLeftToForget = 15;

    void Awake()
    {
        visionTrigger.Mask = visionMask;
        jumpTrigger.Mask = jumpMask;
       damageTrigger.Mask = EnemydamagerMask;
       jumpTrigger.OnStayed += JumpTriggerOnEnter;
       visionTrigger.OnEntered += VisionTriggerOnEnter;
       visionTrigger.OnExited += VisionTriggerOnExit;
    }
    private void Start()
    {
        Debug.Log("svinDatascript Start");
            player = GameObject.FindGameObjectWithTag("player").transform;
            if (player != null)
            {
                Debug.Log("player found");
            }
            CurrentUpdate = CommonUpdate;
            CurrentFixedUpdate = CommonFixedUpdate;
            CommonReStart();
    }
    
    protected void Jump(int js, Rigidbody localRb)
    {
       localRb.AddForce(0,js,0, ForceMode.Impulse); 
    }

    protected void TurnTowardsTarget(Vector3 targetPos, Rigidbody localRb, float localRotationSpeed)
    {
        Vector3 direction = (targetPos - localRb.transform.position).normalized;
        direction.y = 0;
        if (Vector3.Angle(localRb.transform.right, direction) > 5f)
        {
            localRb.transform.right =
                Vector3.RotateTowards(localRb.transform.right, -direction, localRotationSpeed, 0.02f * Time.deltaTime);
        }
    }
    IEnumerator ChangeBehaviour( int chance)
    {
        while (rb != null)
        {
            yield return wait1S;
            restingTimer -= 1;
            if (restingTimer <= 0)
            {
                resting = !resting;
                Random ranDelay = new Random();
                restingTimer = ranDelay.Next(chance);
                if (!resting)
                {
                    Jump(jumpForce,  rb);
                    currentTarget = EstablishTempTarget(rb); 
                }
            }
        }
    }
    protected Vector3 EstablishTempTarget(Rigidbody localRb)
    {
        Vector3 targpos = localRb.transform.position;
        Random ran = new Random();
        int change =ran.Next(-temporaryTargetSpawnRadius, temporaryTargetSpawnRadius);
        targpos.x += change;
        change =ran.Next(-temporaryTargetSpawnRadius, temporaryTargetSpawnRadius);
        targpos.z += change;
        return targpos;
    }

    protected void ReplayAnimation()
    {
       AnimatorStateInfo state = walkAnimator.GetCurrentAnimatorStateInfo(0);
       if (state.normalizedTime >=1f)
       {
           walkAnimator.Play("Scene", 0, 0f);
           Debug.Log("something played");
       }
    }


    protected void MoveForward(int speed, Rigidbody localRb)
    {
        Vector3 pos;
        Vector3 forward = -localRb.transform.right;
        pos = forward*(speed*Time.deltaTime);
        localRb.transform.position += pos;
    }

    public void ReduceVelocity(ref Rigidbody localRb)
    {
        if (localRb.linearVelocity.magnitude > 4)
        {
            localRb.linearVelocity *= 0.99f;
        }
    }

    void StartRaycasting(ref Coroutine raycastCoroutine )
    {
        if (raycastCoroutine == null)
        {
            raycastCoroutine = StartCoroutine(RaycastToSee());
        }
    }
    
    IEnumerator RaycastToSee()
    {
        while (playerInRange)
        {
            Vector3 raypos = rb.transform.position;
            raypos.y += 1;
            Ray ray = new Ray(raypos, (player.position - rb.transform.position).normalized);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.name + " was seen by " + rb.gameObject.name);
                if (hit.transform.gameObject.CompareTag("player"))
                {
                    behaviour = BehaviourType.Agressive;
                    CurrentUpdate = AgressiveUpdate;
                    CurrentFixedUpdate = AgressiveFixedUpdate;
                    AgressiveReStart();
                    if(raycastToSee!=null)StopCoroutine(raycastToSee);
                    if(changeBehaviour!=null)StopCoroutine(changeBehaviour);
                    Debug.Log(rb.gameObject.name + " is now agressive");
                }

                //Instantiate(GetComponent<svinDatascript>().testColumn, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Debug.Log("svin saw nothing");
            }

            yield return wait1S;
        }

        raycastToSee = null;
    }
    
    void VisionTriggerOnEnter(Collider other)
    {
        if (behaviour == BehaviourType.Common)
        {
            if (other.GameObject().CompareTag("player"))
            {
                if (player == null)
                {
                    player = other.gameObject.transform;

                }
                StartRaycasting(ref raycastToSee);
                playerInRange = true;
            }
        }
        if (behaviour == BehaviourType.Agressive)
        {
            if (other.GameObject().CompareTag("player"))
            {
                playerInRange = true;
            }
        }
    }
    void VisionTriggerOnExit(Collider other)
    {
        if (behaviour == BehaviourType.Common)
        {
            if (enabled)
            {
                if (other.GameObject().CompareTag("player"))
                {
                    raycastToSee = null;
                    playerInRange = false;
                }
            }
        }
        if (behaviour == BehaviourType.Agressive)
        {
            if (other.GameObject().CompareTag("player"))
            {
                playerInRange = false;
            }
        }
    }
    void JumpTriggerOnEnter(Collider other)
    {
        if (behaviour == BehaviourType.Common)
        {
            if (!resting)
            {
                Jump(jumpForce, rb);
            }
        }
        if (behaviour == BehaviourType.Agressive)
        {
            Jump(jumpForce, rb);
        }
    }
    void StartForgetCoroutine()
    {
        if (forgetCoroutine == null)
        {
            forgetCoroutine = StartCoroutine(ForgetCoroutine());
        }
    }
    private IEnumerator ForgetCoroutine()
    {
        yield return wait1S;
        Debug.Log(rb.name+" started forgetting you");
        timeLeftToForget = timeToForget;
        while (forgetCoroutine != null)
        {
            yield return wait1S;
            Debug.Log("while cycle");
            Vector3 raypos = rb.transform.position;
            raypos.y += 1;
            Ray ray = new Ray(raypos,
                (player.position - rb.transform.position).normalized);
            RaycastHit hit;
            if (playerInRange)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    //Instantiate(GetComponent<svinDatascript>().testColumn, hit.point, Quaternion.LookRotation(hit.normal));
                    if (hit.collider.gameObject.CompareTag("player"))
                    {
                        timeLeftToForget++;
                    }
                }
            }
            timeLeftToForget--;
            if (timeLeftToForget <= 0)
            {
                Debug.Log(name+" forgot about your existence");
                forgetCoroutine = null;
                behaviour = BehaviourType.Common;
                CurrentUpdate = CommonUpdate;
                CurrentFixedUpdate = CommonFixedUpdate;
                CommonReStart();
            }
            Debug.Log(timeLeftToForget+" seckonds is left to forget you for " +rb.name);
        }
    }
    void Update()
    {
        CurrentUpdate.Invoke();
    }
    void FixedUpdate()
    {
        ReduceVelocity(ref rb);
        CurrentFixedUpdate.Invoke();
    }

    void CommonUpdate()
    {
        if (!resting)
        {
            ReplayAnimation();
        }
    }

    void CommonFixedUpdate()
    {
        if (!resting)
        {
            TurnTowardsTarget(currentTarget, rb, rotationSpeed);
            MoveForward(svinSpeed, rb);
        }
    }

    void CommonReStart()
    {
        changeBehaviour = StartCoroutine(ChangeBehaviour(maxbehaveDelay));
    }

    void AgressiveUpdate()
    {
        ReplayAnimation(); 
        StartForgetCoroutine();
    }

    void AgressiveFixedUpdate()
    {
        TurnTowardsTarget(player.position, rb, agroRotationSpeed);
        MoveForward(svinAgroSpeed, rb);
    }
    void AgressiveReStart(){}
    
}


