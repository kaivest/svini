using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class svinJumpScript : MonoBehaviour
{
    public Rigidbody svin;
    public GameObject frontLegs;
    public GameObject rearLegs;
    public GameObject frontAxis;
    public GameObject rearAxis;
    /// <summary>
    /// 
    /// </summary>
    public static float legRotation=30f;
    public static float legRotationSpeed = 2f;/// <summary>
                                              
                                              /// </summary>
    public float jumpStrength = 10f;
    public bool isJumping = false;
    public Quaternion frontLegSpartRotation;
    public Quaternion frontLegTargetRotation;
    public Quaternion rearLegSpartRotation;
    public Quaternion rearLegTargetRotation;
    public Coroutine legMovementcoroutine;
    private WaitForSeconds wait001 = new WaitForSeconds(1/60f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

   public IEnumerator LegMovementCoroutine()
   {
       bool legsAway = true;
       bool legsBack = false;
       if (legsAway)
       {
           for (int i = 0; i < legRotation/2; i++)
           {
               yield return wait001;
               Vector3 frontLegRot =frontLegs.transform.localEulerAngles;
               frontLegRot.z += legRotationSpeed;
               frontLegs.transform.localEulerAngles = frontLegRot;
               Vector3 rearLegRot =rearLegs.transform.localEulerAngles;
               rearLegRot.z -= legRotationSpeed;
               rearLegs.transform.localEulerAngles = rearLegRot;
           }

           legsAway = false;
           legsBack = true;
       }
       if (legsBack)
       {
           for (float i = legRotation/2; i >0 ; i--)
           {
               yield return wait001;
               Vector3 frontLegRot =frontLegs.transform.localEulerAngles;
               frontLegRot.z -= legRotationSpeed;
               frontLegs.transform.localEulerAngles = frontLegRot;
               Vector3 rearLegRot =rearLegs.transform.localEulerAngles;
               rearLegRot.z += legRotationSpeed;
               rearLegs.transform.localEulerAngles = rearLegRot;
           }

           legsBack = false;
       }
       legMovementcoroutine = null;
   }
    // Update is called once per frame
    void Update()
    {
        if (svinCommonBehaviour.svinRotating || svinCommonBehaviour.svinMoving||svinAttack.svinCharge||svinAttack.svinRotate)
        {
            isJumping = true;
        }

        if (svinCommonBehaviour.svinRotating == false && svinCommonBehaviour.svinMoving == false&&svinAttack.svinCharge == false&&svinAttack.svinRotate ==false )
        {
            isJumping = false;
        }

        if (svin.position.y > 1.5f)
        {
            if (legMovementcoroutine == null)
            {
                legMovementcoroutine = StartCoroutine(LegMovementCoroutine());
            }
        }

    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor") && (isJumping))
        {
            Jump();
        }
    }

    void Jump()
    {

            svin.AddForce(0, jumpStrength , 0, ForceMode.Impulse);
        
    }
}
