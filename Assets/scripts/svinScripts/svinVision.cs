using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class svinVision : MonoBehaviour
{
    [SerializeField] public Transform target;
    public static  bool svinSeesYou = false;
    public static bool svinSees = false;
    public  bool rotationFixed = true;
    private Coroutine flyCoroutine;
    public  GameObject visionField;
    public GameObject svin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    IEnumerator FlyCoroutine()
    {
        rotationFixed = false;
        yield return new WaitForSeconds(4f);
        rotationFixed = true;
        flyCoroutine = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            svinSeesYou = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        svinSees = svinSeesYou;
        if (svinSeesYou)
        {
                if (svin != null)
                {
                    svin.GetComponent<svinAttack>().enabled = true;
                    svinCommonBehaviour.svinRotating = false;
                    svinCommonBehaviour.svinMoving = false;
                    svin.GetComponent<svinCommonBehaviour>().enabled = false;
                }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "wearpon")
        {
            if (flyCoroutine == null)
            {
                flyCoroutine = StartCoroutine(FlyCoroutine());
            }
        }
    }
    private void FixedUpdate()
    {
        if (rotationFixed)
        {
            Quaternion noRot = transform.rotation;
            noRot.x = 0;
            noRot.z = 0;
            transform.rotation = noRot;
        }
    }
}
