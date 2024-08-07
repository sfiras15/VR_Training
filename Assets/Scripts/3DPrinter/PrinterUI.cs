using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterUI : MonoBehaviour
{
    private Printer printer;

    private Temperature[] temperatures;
    private Increment[] increments;

    private float updateInterval = 2f; // Time in seconds between updates
    private int incrementValue = 5;

    // Events for the quest system
    private bool heatLevelEventInvoked = false;

    private void Awake()
    {
        // true to get even the inactive ones in hierarchy
        temperatures = GetComponentsInChildren<Temperature>(true);
        increments = GetComponentsInChildren<Increment>(true);
        printer = new Printer();
    }

    private void OnEnable()
    {
        if (IncrementExists())
        {
            Increment.onIncrementChanged += ChangeIncrement;
            Debug.Log("Subscribed to Increment.onIncrementChanged");
        }
        StartCoroutine(UpdateTemperaturesRoutine());
    }

    private void OnDisable()
    {
        if (IncrementExists())
        {
            Increment.onIncrementChanged -= ChangeIncrement;
            Debug.Log("Unsubscribed from Increment.onIncrementChanged");
        }
        StopCoroutine(UpdateTemperaturesRoutine());
    }

    private bool IncrementExists()
    {
        return FindObjectsOfType<Increment>(true).Length > 0;
    }

    private void ChangeIncrement(IncrementType type, int value)
    {
        if (type == IncrementType.TEMPERATURE)
        {
            printer.incrementTemperature = value;
            InitializeIncrements();
            Debug.Log("Printer increment: " + printer.incrementTemperature);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTemperatureUI();
        InitializeIncrements();
    }

    private void Update()
    {
        HeatingQuest();
    }

    private void HeatingQuest()
    {
        if (printer.currentBedTemperature == 50 && printer.currentNozzleTemperature == 50 && !heatLevelEventInvoked)
        {
            heatLevelEventInvoked = true;
            GameEventsManager.instance.HeatEventOccurred();
        }
    }
    private void UpdateTemperatureUI()
    {
        foreach (var temperature in temperatures)
        {
            if (temperature.type == TemperatureType.NOZZLE)
                temperature.UpdateText($"{printer.currentNozzleTemperature} / {printer.targetNozzleTemperature}");
            else
                temperature.UpdateText($"{printer.currentBedTemperature} / {printer.targetBedTemperature}");
        }
    }

    private void InitializeIncrements()
    {
        foreach (var increment in increments)
        {
            if (increment.type == IncrementType.TEMPERATURE)
                increment.UpdateText($"{printer.incrementTemperature}");
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
        UpdateTemperatureUI();
    }
    public void DecreaseBedTemperature()
    {
        printer.targetBedTemperature -= printer.incrementTemperature;
        UpdateTemperatureUI();
    }

    public void IncreaseNozzleTemperature()
    {
        printer.targetNozzleTemperature += printer.incrementTemperature;
        UpdateTemperatureUI();
    }
    public void DecreaseNozzleTemperature()
    {
        printer.targetNozzleTemperature -= printer.incrementTemperature;
        UpdateTemperatureUI();
    }
}
