using System;
using UnityEngine;
using System.Collections;
public class pileAppearScript : MonoBehaviour
{
    public GameObject pile;
    private Coroutine pileEmergeCoroutine;
    float i = Mathf.PI/2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (i < 3 *Mathf.PI/2)
        {
            Vector3 pilePos = pile.transform.position;
            pilePos.y += Mathf.Sin(i) * 0.06f;
            pile.transform.position = pilePos;
        }
        else
        {
            Destroy(this.gameObject);
        }
        i+=1/57f;
    }
}
