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
    private Ending ending;
    public CanvasGroup LoseBackgroundImageCanvasGroup;

    void Start()
    {
        oxygenText = GameObject.Find("Oxygen Status").GetComponent<TextMeshProUGUI>();
        oxygenText.GetComponent<TextMeshProUGUI>().text = "Oxygen: " + oxygenStatus + "%";
        if (DEBUG) Debug.Log("Oxygen text set");

        //find ending script
        ending = FindObjectOfType<Ending>();
        if(ending == null){
            Debug.LogError("Ending script not found!");
        }
    }
    void Update()
    {
        //if WASD + SPACE, decrease oxygen
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if( Input.GetKey("space")){
                oxygenStatus -= .01;
                oxygenText.GetComponent<TextMeshProUGUI>().text = "Oxygen: " + ((int)oxygenStatus);
                if (DEBUG) Debug.Log("Decreased oxygen");
            }
        }
        // if oxygen tank collected, gain oxygen
        if (OxygenTankCollision.gainOxygen)
        {
            oxygenText.GetComponent<TextMeshProUGUI>().text = "Oxygen: " + ((int)oxygenStatus + oxygenGained);
        }
        //if oxygen depleted, DIE
        if(oxygenStatus <= 0){
            if(DEBUG) Debug.Log("Oxygen empty!");
            ending.EndLevel(LoseBackgroundImageCanvasGroup, true);
        }
    }

}
