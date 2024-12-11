using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 offset;


    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        offset = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position - offset;


        Vector3 rotate = player.transform.eulerAngles;
        rotate.y = 0;
        transform.eulerAngles = rotate;
    }
}
