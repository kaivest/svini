using Unity.VisualScripting;
using UnityEngine;

public class svinAgroBehaviourScript : svinDatascript
{
    void Awake()
    {
        walkAnimator = GetComponent<svinDatascript>().walkAnimator;
        GetComponent<svinDatascript>().jumpTrigger = GetComponent<svinDatascript>().jumpTrigger.gameObject.AddComponent<customTrigger>();
        GetComponent<svinDatascript>().jumpTrigger.OnStayed += JumpTriggerOnStay;
        Debug.Log(GetComponent<svinDatascript>().jumpTrigger.enabled + " is datascript jumptrigger");
        
    }

    void OnJumpTriggerEnter(Collider other)
    {
        
    }
    void JumpTriggerOnStay(Collider other)
    {
        if ((GetComponent<svinDatascript>().jumpMask.value&(1<< other.gameObject.layer))!=0)
        {
            Jump(GetComponent<svinDatascript>().jumpStrength, GetComponent<svinDatascript>().rb);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       replayAnimation(); 
    }
}
