using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class svinAttack : MonoBehaviour
{
    public static bool svinCharge = false;
    public Rigidbody svin;
    public float chargeSpeed= 7f;
    public static Transform targe;
    private Coroutine turnToTargetCoroutine;
    public static bool svinRotate = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if (targe != null)
        {
            svinRotate = true;
            svinCharge = true;
        }
    }

    private void FixedUpdate()
    {
        if ((svinRotate)&&targe!=null)
        {
            Vector3 direction = (targe.position - svin.transform.position).normalized;
            if (Vector3.Angle(svin.transform.right, direction) > 5f)
            {
                svin.transform.right =
                    Vector3.RotateTowards(svin.transform.right, direction, 0.02f, 0.02f * Time.deltaTime);
            }
        }
        if (svinCharge)
        {
            Vector3 movement = svin.position;
            Vector3 forward = transform.right;
            movement = forward * (chargeSpeed * Time.deltaTime);
            svin.position += movement;
        }
    }
}
