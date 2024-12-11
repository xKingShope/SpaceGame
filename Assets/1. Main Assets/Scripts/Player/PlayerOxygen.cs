using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerOxygen : MonoBehaviour
{
    public TextMeshProUGUI oxygenText;
    public double oxygenStatus;
    public int oxygenGained;
    public Boolean DEBUG = true;
    float count;
    private int newOxygenStatus;

    void Start()
    {
        StartCoroutine("decrease");
        oxygenText = GameObject.Find("Oxygen Status").GetComponent<TextMeshProUGUI>();
        oxygenText.GetComponent<TextMeshProUGUI>().text = "Oxygen: " + oxygenStatus;
        if (DEBUG) Debug.Log("Oxygen text set");
    }
    void Update()
    {
       //using a while loop freezes the whole program
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && Input.GetKey("space"))
        {
            oxygenStatus -= .01;
            oxygenText.GetComponent<TextMeshProUGUI>().text = "Oxygen: " + ((int)oxygenStatus);
            if (DEBUG) Debug.Log("Decreased oxygen");
        }
        if (OxygenTankCollision.gainOxygen)
        {
            oxygenText.GetComponent<TextMeshProUGUI>().text = "Oxygen: " + ((int)oxygenStatus + oxygenGained);
        }
    }

}
