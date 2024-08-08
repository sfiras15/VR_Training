using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PrinterUI : MonoBehaviour
{
    private Printer printer;

    private Temperature[] temperatures;
    private Increment[] increments;
    private ExtrudedMaterial extrudedMaterial;
    private Position[] positions;

    private float updateInterval = 2f; // Time in seconds between updates
    private int incrementValue = 5;

    // bools  for the quest system
    private bool heatLevelEventInvoked = false;
    private bool materialEventInvoked = false;
    private bool homeEventInvoked = false;

    private void Awake()
    {
        // true to get even the inactive ones in hierarchy
        temperatures = GetComponentsInChildren<Temperature>(true);
        increments = GetComponentsInChildren<Increment>(true);
        extrudedMaterial = GetComponentInChildren<ExtrudedMaterial>(true);
        positions = GetComponentsInChildren<Position>(true);
        printer = new Printer();
    }

    private void OnEnable()
    {
        if (IncrementExists())
        {
            Increment.onIncrementChanged += ChangeIncrement;
            //Debug.Log("Subscribed to Increment.onIncrementChanged");
        }
        StartCoroutine(UpdateTemperaturesRoutine());
    }

    private void OnDisable()
    {
        if (IncrementExists())
        {
            Increment.onIncrementChanged -= ChangeIncrement;
            //Debug.Log("Unsubscribed from Increment.onIncrementChanged");
        }
        StopCoroutine(UpdateTemperaturesRoutine());
    }

    private bool IncrementExists()
    {
        return FindObjectsOfType<Increment>(true).Length > 0;
    }

    private void ChangeIncrement(IncrementType type, float value)
    {
        
        if (type == IncrementType.TEMPERATURE)
        {
            printer.incrementTemperature = (int) value;
            InitializeIncrements();
            Debug.Log("Printer increment: " + printer.incrementTemperature);
        }
        if (type == IncrementType.EXTRUDED_MATERIAL)
        {
            printer.incrementExtrusion = value;
            InitializeIncrements();
            Debug.Log("Printer increment: " + printer.incrementExtrusion);
        }
        if (type == IncrementType.POSITION)
        {
            printer.incrementPosition = value;
            InitializeIncrements();
            Debug.Log("Printer increment: " + printer.incrementPosition);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTemperatureUI();
        InitializeIncrements();
        UpdateExtrudedMaterialUI(0.00f);
        UpdateNozzlePositionUI();
    }

    private void Update()
    {
        HeatingQuest();
        ExtrusionQuest();
        HomeQuest();
    }

    // Events for the quest System
    private void HeatingQuest()
    {
        if (printer.currentBedTemperature == 50 && printer.currentNozzleTemperature == 50 && !heatLevelEventInvoked)
        {
            heatLevelEventInvoked = true;
            GameEventsManager.instance.HeatEventOccurred();
        }
    }
    private void ExtrusionQuest()
    {
        if (printer.extrudedValue >= 20 && !materialEventInvoked)
        {
            materialEventInvoked = true;
            GameEventsManager.instance.MaterialEventOccurred();
        }
    }
    private void HomeQuest()
    {
        if (printer.nozzlePosition == printer.homePosition && !homeEventInvoked)
        {
            homeEventInvoked = true;
            GameEventsManager.instance.HomeEventOccurred();
        }
    }



    private void UpdateExtrudedMaterialUI(float value)
    {
        extrudedMaterial.UpdateText(value.ToString("F2"));
    }
    private void UpdateTemperatureUI()
    {
        if (temperatures.Length == 0) return;

        foreach (var temperature in temperatures)
        {
            if (temperature.type == TemperatureType.NOZZLE)
                temperature.UpdateText($"{printer.currentNozzleTemperature} / {printer.targetNozzleTemperature}");
            else
                temperature.UpdateText($"{printer.currentBedTemperature} / {printer.targetBedTemperature}");
        }
  
    }
    private void UpdateNozzlePositionUI()
    {
        if (positions.Length == 0) return;

        foreach (var position in positions)
        {
            position.UpdateText(printer.nozzlePosition);
        }
    }

    private void InitializeIncrements()
    {
        if (increments.Length == 0) return;

        foreach (var increment in increments)
        {
            if (increment.type == IncrementType.TEMPERATURE)
                increment.UpdateText($"{printer.incrementTemperature}");
            else if (increment.type == IncrementType.EXTRUDED_MATERIAL)
                increment.UpdateText($"{printer.incrementExtrusion}");
            else if (increment.type == IncrementType.POSITION)
                increment.UpdateText($"{printer.incrementPosition}");
        }
    }

    private IEnumerator UpdateTemperaturesRoutine()
    {
        while (true)
        {
            // Increment current nozzle temperature towards target nozzle temperature
            if (printer.currentNozzleTemperature < printer.targetNozzleTemperature)
            {
                printer.currentNozzleTemperature += Mathf.Min(incrementValue, printer.targetNozzleTemperature - printer.currentNozzleTemperature);
            }

            // Increment current bed temperature towards target bed temperature
            if (printer.currentBedTemperature < printer.targetBedTemperature)
            {
                printer.currentBedTemperature += Mathf.Min(incrementValue, printer.targetBedTemperature - printer.currentBedTemperature);
            }

            // Update the temperature UI to reflect the new values
            UpdateTemperatureUI();

            // Wait for the specified interval before updating again
            yield return new WaitForSeconds(updateInterval);

            // Add here decrease in the temperature when a certain button has been clicked , same logic

        }
    }
    // Methods called by the buttons in UI
    public void IncreaseBedTemperature()
    {
        printer.targetBedTemperature += printer.incrementTemperature;
        if (printer.targetBedTemperature > 150) printer.targetBedTemperature = 150;
        UpdateTemperatureUI();
    }
    public void DecreaseBedTemperature()
    {
        printer.targetBedTemperature -= printer.incrementTemperature;
        if (printer.targetBedTemperature < 0) printer.targetBedTemperature = 0;
        UpdateTemperatureUI();
    }

    public void IncreaseNozzleTemperature()
    {
        printer.targetNozzleTemperature += printer.incrementTemperature;
        if (printer.targetNozzleTemperature > 275) printer.targetNozzleTemperature = 275;
        UpdateTemperatureUI();
    }
    public void DecreaseNozzleTemperature()
    {
        printer.targetNozzleTemperature -= printer.incrementTemperature;
        if (printer.targetNozzleTemperature < 0) printer.targetNozzleTemperature = 0;
        UpdateTemperatureUI();
    }

    public void IncreaseExtrudedMaterial()
    {
        printer.extrudedValue += printer.incrementExtrusion;
        if (printer.extrudedValue > 30f) printer.extrudedValue = 30f;
        UpdateExtrudedMaterialUI(printer.extrudedValue);
        
    }
    public void DecreaseExtrudedMaterial()
    {
        printer.extrudedValue -= printer.incrementExtrusion;
        if (printer.extrudedValue < -10f) printer.extrudedValue = -10f;
        UpdateExtrudedMaterialUI(printer.extrudedValue);
    }

    public void IncreaseXPosition()
    {
        printer.nozzlePosition.x += printer.incrementPosition;
        // add position limit based on the 3D printer
        UpdateNozzlePositionUI();
    }
    public void IncreaseYPosition()
    {
        printer.nozzlePosition.y += printer.incrementPosition;
        // add position limit based on the 3D printer
        UpdateNozzlePositionUI();
    }
    public void IncreaseZPosition()
    {
        printer.nozzlePosition.z += printer.incrementPosition;
        // add position limit based on the 3D printer
        UpdateNozzlePositionUI();
    }
    public void DecreaseXPosition()
    {
        printer.nozzlePosition.x -= printer.incrementPosition;
        // add position limit based on the 3D printer
        UpdateNozzlePositionUI();
    }
    public void DecreaseYPosition()
    {
        printer.nozzlePosition.y -= printer.incrementPosition;
        // add position limit based on the 3D printer
        UpdateNozzlePositionUI();
    }
    public void DecreaseZPosition()
    {
        printer.nozzlePosition.z -= printer.incrementPosition;
        // add position limit based on the 3D printer
        UpdateNozzlePositionUI();
    }
    public void ReturnHome()
    {
        StartCoroutine(LerpToHome("x"));
        StartCoroutine(LerpToHome("y"));
        StartCoroutine(LerpToHome("z"));
    }
    public void ReturnXHome()
    {
        StartCoroutine(LerpToHome("x"));
    }
    public void ReturnYHome()
    {
        StartCoroutine(LerpToHome("y"));
    }
    public void ReturnZHome()
    {
        StartCoroutine(LerpToHome("z"));
    }

    private IEnumerator LerpToHome(string axis)
    {
        float duration = 2f; // Duration of the animation of the 3D printer to move
        float timeElapsed = 0f;

        float startValue = GetAxisValue(printer.nozzlePosition, axis);
        float targetValue = GetAxisValue(printer.homePosition, axis);

        while (timeElapsed < duration)
        {
            // Lerp the value based on the elapsed time
            SetAxisValue(ref printer.nozzlePosition, axis, Mathf.Lerp(startValue, targetValue, timeElapsed / duration));

            // Update the UI to reflect the current nozzle position
            UpdateNozzlePositionUI();

            // Increase the elapsed time
            timeElapsed += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Ensure the final position is exactly the home position
        SetAxisValue(ref printer.nozzlePosition, axis, targetValue);
        UpdateNozzlePositionUI();
    }

    private float GetAxisValue(Vector3 position, string axis)
    {
        switch (axis.ToLower())
        {
            case "x": return position.x;
            case "y": return position.y;
            case "z": return position.z;
            default: throw new System.ArgumentException("Invalid axis");
        }
    }

    private void SetAxisValue(ref Vector3 position, string axis, float value)
    {
        switch (axis.ToLower())
        {
            case "x": position.x = value; break;
            case "y": position.y = value; break;
            case "z": position.z = value; break;
            default: throw new System.ArgumentException("Invalid axis");
        }
    }
}
