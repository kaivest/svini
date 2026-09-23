using System.Collections;
using UnityEngine;
using Random = System.Random;
public class spawnSvin : MonoBehaviour
{
    private Coroutine svinSpawnCoroutine;
    public static int currentSvinCount=0;
    public int maxSvin=40;
    public GameObject svinToSpawn;
    public float delay =5f;
    public WaitForSeconds spawnDelay;
    private Vector3 spawnPosition;
    IEnumerator SvinSpawnCoroutine()
    {
        while (currentSvinCount < maxSvin)
        {
            Random randX = new Random();
            spawnPosition.x = (this.transform.position.x + this.transform.localScale.x *5 - randX.Next((int)(this.transform.localScale.x*10)));
            Random randZ = new Random();
            spawnPosition.z = (this.transform.position.z + this.transform.localScale.z *5 - randX.Next((int)(this.transform.localScale.z*10)));
            spawnPosition.y = this.transform.position.x+2;
            Instantiate(svinToSpawn, spawnPosition, transform.rotation);
            currentSvinCount++;
            yield return spawnDelay;
        }
        svinSpawnCoroutine = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnDelay = new WaitForSeconds(5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (svinSpawnCoroutine == null)
        {
            svinSpawnCoroutine = StartCoroutine((SvinSpawnCoroutine()));
            if (delay != 5f)
            {
                spawnDelay = new WaitForSeconds(delay);
            }
        }
    }
}
