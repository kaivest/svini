using UnityEngine;
using System.Collections;
using Random = System.Random;

public class particleAppearScript : MonoBehaviour
{
    private Coroutine disappearCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        disappearCoroutine = StartCoroutine(Appear());
        Random rand = new Random();
        int a =rand.Next(200)-100;
        int b =  rand.Next(500);
        float c=rand.Next(200)-100;
        Vector3 pushDir = new Vector3(a,b,c);
        pushDir.Normalize();
        Debug.Log("salo spawned");
        gameObject.GetComponent<Rigidbody>().AddForce(pushDir*0.0001f, ForceMode.Impulse);
        gameObject.GetComponent<Rigidbody>().AddTorque(pushDir*0.0001f, ForceMode.Impulse);
    }

    private IEnumerator Appear()
    {
        yield return new WaitForSeconds(5);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
