using System.Collections;
using UnityEngine;

public class rotationScript : MonoBehaviour
{
    public float rotationSpeed = 0.01f;
    private Coroutine rotationCoroutine;
    public GameObject rotator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        rotator.transform.rotation *= Quaternion.Euler(0, rotationSpeed, 0);
    }
}
