using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;
public class svinDatascript : MonoBehaviour
{
    public Animator walkAnimator;
    public Rigidbody rb;
    public int jumpStrength = 1;
    private WaitForSeconds wait001 = new WaitForSeconds(0.01f);
    public Transform player;
    public float rotationSpeed = 0.01f;
    public int svinSpeed = 5;
    public int SvinAgroSpeed = 7;
    public int maxbehaveDelay=15;
    public bool resting = true;
    public bool svinSeesYou = false;
    public int restingTimer = 5;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;
        
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "floor"&&!resting)
        {
            Jump(jumpStrength);
        }
    }
   
    protected void Jump(int js)
    {
       rb.AddForce(0,js,0, ForceMode.Impulse); 
    }

    protected void turnTowardsTarget(Vector3 targetPos)
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
    protected void fixRotation()
    {
        Quaternion rot = rb.transform.rotation;
        rot.x = 0;
        rot.z = 0;
        rb.transform.rotation = rot;
    }

    protected void MoveForward()
    {
        Vector3 pos = rb.transform.position;
        Vector3 forward = -rb.transform.right;
        pos = forward*(svinSpeed*Time.deltaTime);
        rb.transform.position += pos;
    }
}


