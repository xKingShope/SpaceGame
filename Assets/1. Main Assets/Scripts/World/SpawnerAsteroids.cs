using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAsteroids : MonoBehaviour
{
    public int AsteroidDensity = 200;
    public GameObject asteroid;
    public float Xspread = 500;
    public float Yspread = 500;
    public float Zspread = 200;


    private void SpreadItems ()
    {
        float randomRotation = Random.Range(0f, 180f);
        Vector3 RandomPosition = new Vector3(Random.Range(-Xspread, Xspread),Random.Range(-Yspread, Yspread),Random.Range(10, Zspread));
        GameObject clone = Instantiate(asteroid, RandomPosition, Quaternion.Euler(new Vector3(randomRotation, randomRotation, randomRotation)));
       
        float size = Random.Range(0.4f, 6f);
       
        clone.transform.localScale = new Vector3(size, size, size);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < AsteroidDensity; i++)
        {
            SpreadItems();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
