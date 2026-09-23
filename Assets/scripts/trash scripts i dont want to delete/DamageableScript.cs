using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float damage;
    public Coroutine flyCoroutine;
    public float knockback;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

  void  OnCollisionEnter(Collision other)
    {

            if (other.gameObject.GetComponent<healthScript>() != null &&
                other.gameObject.tag != gameObject.tag)
            {
                    other.gameObject.GetComponent<healthScript>().currentHP -= damage;
                    if (other.gameObject.tag != "floor")
                    {
                        Vector3 pushDirection =
                            (other.gameObject.transform.position - gameObject.transform.position).normalized;
                        if (other.gameObject.GetComponent<Rigidbody>() != null)
                        {
                            other.gameObject.GetComponent<Rigidbody>().AddForce(pushDirection.x * knockback,
                                pushDirection.y * knockback, pushDirection.z * knockback, ForceMode.Impulse);
                        }
                    }
            }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
