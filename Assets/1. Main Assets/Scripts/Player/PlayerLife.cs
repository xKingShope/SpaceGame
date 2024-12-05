using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLife : MonoBehaviour
{
    public TextMeshProUGUI oxygenText;
    public int oxygenStatus;
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
    void FixedUpdate()
    {
       //WHY DOES THIS NOT UPDATE MORE THAN ONCE ??
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            oxygenText.GetComponent<TextMeshProUGUI>().text = "Oxygen: " + (oxygenStatus - 1);
            if (DEBUG) Debug.Log("Decreased oxygen");
        }
        if (OxygenTankCollision.gainOxygen)
        {
            oxygenText.GetComponent<TextMeshProUGUI>().text = "Oxygen: " + (oxygenStatus + oxygenGained);
        }
    }

}
