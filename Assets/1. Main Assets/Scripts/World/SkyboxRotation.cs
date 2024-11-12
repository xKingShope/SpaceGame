using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    void Update()
    {
        float rotation = Time.time * 0.4f;
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }
}



