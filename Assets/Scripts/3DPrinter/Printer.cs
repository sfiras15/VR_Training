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
    public float babyStepIncrement;
    public float currentBabyStep;
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

        // For the position Add a function that reads and initiates the value from the printer model 's position in the editor on start up
        nozzlePosition = new Vector3(0,0f,40f);// check z value later
        homePosition = new Vector3(0f, 0f, -180f); // this is just an example check real values with the 3d printer model 
        incrementPosition = 1f;

        currentBabyStep = 0f;
        babyStepIncrement = 0.01f;
        probeOffset = 0f;


        incrementExtrusion = 10.00f;
        extrudedValue = 0f;
    }
}
