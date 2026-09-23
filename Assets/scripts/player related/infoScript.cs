using UnityEngine;
using TMPro;
public class infoScript : MonoBehaviour
{
    public GameObject player;
    public static int killCoutt;
    public TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "hp:"+ player.GetComponent<healthScript>().currentHP.ToString()+"\n"+"kill count: "+killCoutt.ToString();

    }
}
