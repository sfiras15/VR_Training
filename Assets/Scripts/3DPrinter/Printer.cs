using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class that stores all the relevant printer parameters that will be used in the ui.
/// </summary>
public class Printer
{
    // Temperature
    public int currentNozzleTemperature;
    public int targetNozzleTemperature;
    public int initialNozzleTemperature;

    public int currentBedTemperature;
    public int targetBedTemperature;
    public int initialBedTemperature;

    public int incrementTemperature; 
    
    // Position
    public Vector3 nozzlePosition;
    public Vector3 homePosition;
    public float incrementPosition;

    // BabySteps
    public float probeOffset;

    // Extrusion
    public float incrementExtrusion;
    public float extrudedValue;

    // default values change them if necessary
    public Printer()
    {
        currentNozzleTemperature = 32;
        initialNozzleTemperature = 32;
        targetNozzleTemperature = 0;
        currentBedTemperature = 35;
        initialBedTemperature = 35;
        targetBedTemperature = 0;
        incrementTemperature = 10;


        nozzlePosition = new Vector3(2f,10f,-2f);// check z value later
        homePosition = new Vector3(0f, 3f, 5f); // this is just an example check real values with the 3d printer model 
        incrementPosition = 0.1f;

        probeOffset = 0f;


        incrementExtrusion = 10.00f;
        extrudedValue = 0f;
    }
}
