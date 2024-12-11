using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObjects : MonoBehaviour
{
    public int ObjectDensity = 200;
    public GameObject[] spawnItems;
    public float Xspread = 500;
    public float Yspread = 500;
    public float Zspread = 200;


    private void SpreadItems ()
    {
        float randomRotation = Random.Range(0f, 180f);
        int randomIndex = Random.Range(0, spawnItems.Length);
        Vector3 RandomPosition = new Vector3(Random.Range(-Xspread, Xspread),Random.Range(-Yspread, Yspread),Random.Range(10, Zspread));
        GameObject clone = Instantiate(spawnItems[randomIndex], RandomPosition, Quaternion.Euler(new Vector3(randomRotation, randomRotation, randomRotation)));
       
        if (randomIndex > 0 && randomIndex < 3)
        {
            float size = Random.Range(60f, 65f);
            clone.transform.localScale = new Vector3(size, size, size);
        } 
        else
        {
            float size = Random.Range(.5f, .65f);
            clone.transform.localScale = new Vector3(size, size, size);
        }

    
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ObjectDensity; i++)
        {
            SpreadItems();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
