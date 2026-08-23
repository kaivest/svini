using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;
using Rando = UnityEngine.Random;



public class svinCommonBehaviour : MonoBehaviour
{
    public Rigidbody svin;
    private Coroutine svinBehavesCoroutine;
    private Coroutine svinGOCoroutine;
    public float rotationSpeed = 30f;
    private float behaveDelay;
    public float SvinSpeed = 0f;
    public int maxbehaveDelay=10;
    private float rotationAngle=0f;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    public static bool svinRotating = false;
    public static bool svinMoving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public bool getSvinRotating()
    {
        return svinRotating;
    }
   private IEnumerator SvinBehaviourCoroutine()
   {
       Random ranDelay = new Random();
       behaveDelay = ranDelay.Next(maxbehaveDelay);
       yield return new WaitForSeconds(behaveDelay);
       rotationAngle = Rando.Range(-360f, 360f);
      // svinMoving = true;
       svinRotating = true;
       svinBehavesCoroutine = null;
   }
    // Update is called once per frame
    void Update()
    {
        if (svinBehavesCoroutine == null && svinMoving==false &&svinRotating==false)
        {
            svinBehavesCoroutine = StartCoroutine((SvinBehaviourCoroutine()));
        }
        
    }

    private void FixedUpdate()
    {
        if (svinRotating)
        {
            svinMoving = true;
            startRotation = transform.rotation;
            targetRotation=Quaternion.Euler(0,rotationAngle,0);
            transform.rotation =
                Quaternion.RotateTowards(startRotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                svinRotating = false;
                svinMoving = false;
            }
        }

        if (svinMoving||svin.position.y>1.8)
        {
            Vector3 svinPosition = svin.position;
            Vector3 svinForward = transform.right;
            svinPosition = svinForward*(SvinSpeed * Time.deltaTime);
            svin.position += svinPosition;
        }
    }
}
