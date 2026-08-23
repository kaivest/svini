using UnityEngine;

public class healthScript : MonoBehaviour
{
    public float maxHealthpoints;
    public float currentHP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHealthpoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            spawnSvin.currentSvinCount--;
            infoScript.killCoutt++;
            Destroy(gameObject);
            
        }

        if (currentHP < maxHealthpoints)
        {
            svinVision.svinSeesYou = true;
        }
    }
}
