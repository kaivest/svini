using UnityEngine;
using System;


public class shovelScript : wearponDataScript
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        gameObject.AddComponent<customTrigger>();
        gameObject.GetComponent<customTrigger>().OnEntered += OnColliderEntered;

    }

    void OnColliderEntered(Collider other)
    {
        if (other.gameObject.CompareTag("svin"))
        {
            other.gameObject.GetComponent<entityData>().healthpoints-= Convert.ToInt16(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {  
        GetComponent<customTrigger>().Mask =  GameObject.FindGameObjectWithTag("playerHolder").GetComponent<playerMovementScript>().iCanDamage;
    }

    public void use()
    {
        Debug.Log("use");
    }
}
