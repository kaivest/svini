using System;
using UnityEngine;
using Random = System.Random;

public class healthScript : MonoBehaviour
{
    public float currentHP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        
    }

    void Start()
    {
        Debug.Log(this.
            gameObject.
            GetComponent<entityData>()
            .healthpoints);
        Debug.Log("started healthScript");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<entityData>().healthpoints <= 0)
        {
            Debug.Log(gameObject.name + " has been destroyed");
            breakIntoParticles(gameObject.GetComponent<svinDatascript>().salo);
            Destroy(gameObject);
            
        }
        
    }
    void breakIntoParticles(GameObject particle)
    {
        for (int i = 0; i < gameObject.GetComponent<svinDatascript>().saloCount; i++)
        {
            Random rand = new Random();
            int a =rand.Next(4);
            int b =rand.Next(4);
            int c= rand.Next(4);
            Vector3 pushDir = new Vector3();
            pushDir.x = gameObject.transform.position.x+2-a;
            pushDir.z = gameObject.transform.position.z+2-b;
            pushDir.y = gameObject.transform.position.y+2-c;
            
            Instantiate(particle, pushDir, Quaternion.identity);
        }
    }

}
