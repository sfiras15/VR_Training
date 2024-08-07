using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class that stores all the relevant printer parameters that will be used in the ui.
/// </summary>
public class Printer
{
    public int currentNozzleTemperature;
    public int targetNozzleTemperature;
    public int currentBedTemperature;
    public int targetBedTemperature;
    public int incrementTemperature;   
    public Vector3 nozzlePosition;
    public float probeOffset;


    // default values change them if necessary
    public Printer()
    {
        currentNozzleTemperature = 32;
        targetNozzleTemperature = 0;
        currentBedTemperature = 35;
        targetBedTemperature = 0;
        incrementTemperature = 10;
        nozzlePosition = new Vector3(0f,0f,-2.15f);// check z value later
        probeOffset = 0f;
    }
}
