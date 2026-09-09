using System;
using UnityEngine;


public class customTrigger : MonoBehaviour
{
    public event System.Action<Collider> OnEntered;
    public event System.Action<Collider> OnExited;
    public event System.Action<Collider> OnStayed;
    public bool isStay = false;
    [SerializeField] public LayerMask Mask;
    void OnTriggerEnter(Collider other)
    {
        if ((Mask.value & (1 << other.gameObject.layer)) != 0)
        {
            OnEntered?.Invoke(other);
            isStay = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((Mask.value & (1 << other.gameObject.layer)) != 0)
        {
            OnExited?.Invoke(other);
            isStay = false;
        }
    }

     void OnTriggerStay(Collider other)
     {
         if ((Mask.value & (1 << other.gameObject.layer)) != 0)
         {
             OnStayed?.Invoke(other);
         }
     }
}
