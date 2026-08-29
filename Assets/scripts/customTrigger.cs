using UnityEngine;

public class customTrigger : MonoBehaviour
{
    public event System.Action<Collider> OnEntered;
    public event System.Action<Collider> OnExited;
    public event System.Action<Collider> OnStayed;
    public bool isStay = false;
    void OnTriggerEnter(Collider other)
    {
            OnEntered?.Invoke(other);
            isStay = true;
    }

    void OnTriggerExit(Collider other)
    {
        OnExited?.Invoke(other);
        isStay = false;
    }

    void OnTriggerStay(Collider other)
    {
        OnStayed?.Invoke(other);
        Debug.Log(name + " stayed in contact with " + other.name);
    }
}
