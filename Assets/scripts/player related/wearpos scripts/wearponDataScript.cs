using UnityEngine;

public class wearponDataScript : MonoBehaviour
{
    public int damage;
    public int impulse;
    public int rotationImpulse;
    public Collider hitbox;
    public int speed;
    public const float baseSpeed = 0.001f; 
    public int comboCount;
    public KeyCode combo1;
    public KeyCode combo2;
    public KeyCode combo3;
    public KeyCode combo4;
    public KeyCode combo5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal virtual void Use(ref Transform wearponPivo)
    {
        Debug.Log("Use");
    }

    internal virtual void Ability(ref Transform wearponPivot)
    {
      Debug.Log("Ability");  
    }
    
    
}
