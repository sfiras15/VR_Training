using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrinterUI : MonoBehaviour
{
    [SerializeField] private Printer_SO printerSo;
    private Printer printer;

    // Components found in the UI menus to update their info
    private Temperature[] temperatures;
    private Increment[] increments;
    private ExtrudedMaterial extrudedMaterial;
    private Position[] positions;
    private BabyStep[] babySteps;

    private float updateInterval = 2f; // Time in seconds between updates
    private int incrementValue = 5; // temperature increment

    // bools for the quest system
    private bool heatLevelEventInvoked = false;
    private bool materialEventInvoked = false;
    private bool homeEventInvoked = false;

    // variables for cooling down temperature 
    private bool isBedCoolingDown = false;
    private bool isNozzleCoolingDown = false;

    // bools for tracking the movement of the axis
    private bool isMovingOnX = false;
    private bool isMovingOnY = false;
    private bool isMovingOnZ = false;

    private void Awake()
    {
        // true to get even the inactive ones in hierarchy
        temperatures = GetComponentsInChildren<Temperature>(true);
        increments = GetComponentsInChildren<Increment>(true);
        extrudedMaterial = GetComponentInChildren<ExtrudedMaterial>(true);
        positions = GetComponentsInChildren<Position>(true);
        babySteps = GetComponentsInChildren<BabyStep>(true);
        printer = new Printer();
    }

    private void OnEnable()
    {
        if (IncrementExists())
        {
            Increment.onIncrementChanged += ChangeIncrement;
            // Debug.Log("Subscribed to Increment.onIncrementChanged");
        }
        StartCoroutine(UpdateTemperaturesRoutine());
    }

    private void OnDisable()
    {
        if (IncrementExists())
        {
            Increment.onIncrementChanged -= ChangeIncrement;
            // Debug.Log("Unsubscribed from Increment.onIncrementChanged");
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
            printer.incrementTemperature = (int)value;
            InitializeIncrements();
            //Debug.Log("Printer increment: " + printer.incrementTemperature);
        }
        if (type == IncrementType.EXTRUDED_MATERIAL)
        {
            printer.incrementExtrusion = value;
            InitializeIncrements();
            //Debug.Log("Printer increment: " + printer.incrementExtrusion);
        }
        if (type == IncrementType.POSITION)
        {
            printer.incrementPosition = value;
            InitializeIncrements();
            //Debug.Log("Printer increment: " + printer.incrementPosition);
        }
        if (type == IncrementType.BABYSTEP)
        {
            printer.babyStepIncrement = value;
            InitializeIncrements();
            //Debug.Log("Printer increment: " + printer.incrementPosition);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize UI elements
        UpdateTemperatureUI();
        InitializeIncrements();
        UpdateExtrudedMaterialUI(0.00f);
        UpdateNozzlePositionUI();
        UpdateBabyStepUI();
    }

    private void Update()
    {
        HeatingQuest();
        ExtrusionQuest();
        HomeQuest();
        if (Input.GetKey(KeyCode.Space))
        {
            Print();
        }
    }

    // Events for the quest system
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
            GameEventsManager.instance.MaterialExtrusionEventOccurred();
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
    private void UpdateBabyStepUI()
    {
        if (babySteps.Length == 0) return;

        foreach (var babyStep in babySteps)
        {
            babyStep.UpdateText(printer.currentBabyStep, printer.probeOffset);
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
            else increment.UpdateText($"{printer.babyStepIncrement.ToString("F2")}");
        }
    }

    private IEnumerator UpdateTemperaturesRoutine()
    {
        while (true)
        {
            if (!isNozzleCoolingDown)
            {
                // Increment current nozzle temperature towards target nozzle temperature
                if (printer.currentNozzleTemperature < printer.targetNozzleTemperature)
                {
                    printer.currentNozzleTemperature += Mathf.Min(incrementValue, printer.targetNozzleTemperature - printer.currentNozzleTemperature);
                }
            }
            else
            {
                if (printer.currentNozzleTemperature > printer.initialNozzleTemperature)
                {
                    printer.currentNozzleTemperature -= Mathf.Min(incrementValue, printer.currentNozzleTemperature - printer.initialNozzleTemperature);
                }
            }

            if (!isBedCoolingDown)
            {
                // Increment current bed temperature towards target bed temperature
                if (printer.currentBedTemperature < printer.targetBedTemperature)
                {
                    printer.currentBedTemperature += Mathf.Min(incrementValue, printer.targetBedTemperature - printer.currentBedTemperature);
                }
            }
            else
            {
                if (printer.currentBedTemperature > printer.initialBedTemperature)
                {
                    printer.currentBedTemperature -= Mathf.Min(incrementValue, printer.currentBedTemperature - printer.initialBedTemperature);
                }
            }
            // Update the temperature UI to reflect the new values
            UpdateTemperatureUI();

            // Wait for the specified interval before updating again
            yield return new WaitForSeconds(updateInterval);

            // Add here decrease in the temperature when a certain button has been clicked, same logic
        }
    }
    // Changes the position UI and the printer coordinates
    private IEnumerator moveNozzlePosition(Axis axis, float startValue, float endValue,float duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            SetAxisValue(ref printer.nozzlePosition, axis, Mathf.Lerp(startValue, endValue, timeElapsed / duration));

            timeElapsed += Time.deltaTime;

            UpdateNozzlePositionUI();
            yield return null;
        }

        SetAxisValue(ref printer.nozzlePosition, axis, endValue);
        UpdateNozzlePositionUI();
    }
    //private float GetAxisValue(Vector3 position, Axis axis)
    //{
    //    switch (axis)
    //    {
    //        case Axis.X: return position.x;
    //        case Axis.Y: return position.y;
    //        case Axis.Z: return position.z;
    //        default: throw new ArgumentException("Invalid axis");
    //    }
    //}

    private void SetAxisValue(ref Vector3 position, Axis axis, float value)
    {
        switch (axis)
        {
            case Axis.X: position.x = value; break;
            case Axis.Y: position.y = value; break;
            case Axis.Z: position.z = value; break;
            default: throw new ArgumentException("Invalid axis");
        }
    }
    private IEnumerator ReturnHomeSequence()
    {
        // Move Y axis home
        yield return StartCoroutine(ReturnYHomeSequence());

        // Move X axis home
        yield return StartCoroutine(ReturnXHomeSequence());

        // Move Z axis home
        yield return StartCoroutine(ReturnZHomeSequence());
    }
    private IEnumerator ReturnYHomeSequence()
    {
        if (isMovingOnY) yield break;

        isMovingOnY = true;
        float startValue = printer.nozzlePosition.y;
        float endValue = printer.homePosition.y;
        yield return StartCoroutine(MoveToDestination(startValue, endValue, Axis.Y, isMovingOnY, printerSo.moveDuration));
    }

    private IEnumerator ReturnXHomeSequence()
    {
        if (isMovingOnX) yield break;

        isMovingOnX = true;
        float startValue = printer.nozzlePosition.x;
        float endValue = printer.homePosition.x;
        yield return StartCoroutine(MoveToDestination(startValue, endValue, Axis.X, isMovingOnX, printerSo.moveDuration));
    }

    private IEnumerator ReturnZHomeSequence()
    {
        if (isMovingOnZ) yield break;

        isMovingOnZ = true;
        float startValue = printer.nozzlePosition.z;
        float endValue = printer.homePosition.z;
        yield return StartCoroutine(MoveToDestination(startValue, endValue, Axis.Z, isMovingOnZ, printerSo.moveDuration));

    }
    private IEnumerator ResetBool(bool value)
    {
        //Debug.Log("Reset");
        yield return new WaitForSeconds(printerSo.moveDuration);
        if (value == isMovingOnX) isMovingOnX = false;
        else if (value == isMovingOnY) isMovingOnY = false;
        else isMovingOnZ = false;
    }

    private IEnumerator MoveToDestination(float start, float end, Axis axis, bool isMovingOnAxis,float duration)
    {
        float incrementValue = MathF.Abs(end - start);
        float sign = end - start < 0 ? -1 : 1;

        if (axis == Axis.X) printerSo.Move(Axis.X, sign * incrementValue, duration);
        else if (axis == Axis.Y) printerSo.Move(Axis.Z, sign * incrementValue, duration); // The Z axis moves along the y coordinates in the printer animation
        else printerSo.Move(Axis.Y, sign * incrementValue, duration); // The bed moves along the Z coordinates in the printer animation
        StartCoroutine(ResetBool(isMovingOnAxis));
        yield return StartCoroutine(moveNozzlePosition(axis, start, end, duration));
        
    }

    // Methods called by the buttons in UI
    public void IncreaseBedTemperature()
    {
        printer.targetBedTemperature += printer.incrementTemperature;
        if (printer.targetBedTemperature > 150) printer.targetBedTemperature = 150;
        isBedCoolingDown = false;
        UpdateTemperatureUI();
    }

    public void DecreaseBedTemperature()
    {
        printer.targetBedTemperature -= printer.incrementTemperature;
        if (printer.targetBedTemperature < 0) printer.targetBedTemperature = 0;
        isBedCoolingDown = false;
        UpdateTemperatureUI();
    }

    public void CooldownBedTemperature()
    {
        printer.targetBedTemperature = 0;
        isBedCoolingDown = true;
        UpdateTemperatureUI();
    }

    public void IncreaseNozzleTemperature()
    {
        printer.targetNozzleTemperature += printer.incrementTemperature;
        if (printer.targetNozzleTemperature > 275) printer.targetNozzleTemperature = 275;
        isNozzleCoolingDown = false;
        UpdateTemperatureUI();
    }

    public void DecreaseNozzleTemperature()
    {
        printer.targetNozzleTemperature -= printer.incrementTemperature;
        if (printer.targetNozzleTemperature < 0) printer.targetNozzleTemperature = 0;
        isNozzleCoolingDown = false;
        UpdateTemperatureUI();
    }

    public void CooldownNozzleTemperature()
    {
        printer.targetNozzleTemperature = 0;
        isNozzleCoolingDown = true;
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
        if (printer.extrudedValue < 0f) printer.extrudedValue = 0f;
        UpdateExtrudedMaterialUI(printer.extrudedValue);
    }

    public void IncreaseXPosition()
    {
        //Debug.Log("test");
        if (printerSo != null)
        {
            if (isMovingOnX) return;
            //Debug.Log("test inside");
            isMovingOnX = true;
            float startValue = printer.nozzlePosition.x;
            float endValue = printer.nozzlePosition.x + printer.incrementPosition;
            // add position limit based on the 3D printer
            if (endValue > 300f) endValue = 300f;
            StartCoroutine(MoveToDestination(startValue, endValue, Axis.X, isMovingOnX, printerSo.moveDuration));
        }
    }

    public void IncreaseYPosition()
    {
        if (printerSo != null)
        {
            if (isMovingOnY) return;

            isMovingOnY = true;
            float startValue = printer.nozzlePosition.y;
            float endValue = printer.nozzlePosition.y + printer.incrementPosition;
            if (endValue > 460f) endValue = 460f;

            StartCoroutine(MoveToDestination(startValue, endValue, Axis.Y, isMovingOnY, printerSo.moveDuration));   
        }
    }
    public void IncreaseZPosition()
    {
        if (printerSo != null)
        {
            if (isMovingOnZ) return;

            isMovingOnZ = true;
            float startValue = printer.nozzlePosition.z;
            float endValue = printer.nozzlePosition.z + printer.incrementPosition;
            if (endValue > 810f) endValue = 810f;
            StartCoroutine(MoveToDestination(startValue, endValue, Axis.Z, isMovingOnZ, printerSo.moveDuration));
        }
    }
    public void DecreaseXPosition()
    {
        if (printerSo != null)
        {
            if (isMovingOnX) return;

            isMovingOnX = true;
            float startValue = printer.nozzlePosition.x;
            float endValue = printer.nozzlePosition.x - printer.incrementPosition;
            if (endValue < -320f) endValue = -320f;
            StartCoroutine(MoveToDestination(startValue, endValue, Axis.X, isMovingOnX, printerSo.moveDuration));
        }
    }
    public void DecreaseYPosition()
    {
        if (printerSo != null)
        {
            if (isMovingOnY) return;

            isMovingOnY = true;
            float startValue = printer.nozzlePosition.y;
            float endValue = printer.nozzlePosition.y - printer.incrementPosition;
            if (endValue < -350f) endValue = -350f;
            StartCoroutine(MoveToDestination(startValue, endValue, Axis.Y, isMovingOnY, printerSo.moveDuration));
        }
    }
    public void DecreaseZPosition()
    {
        if (printerSo != null)
        {
            if (isMovingOnZ) return;

            isMovingOnZ = true;
            float startValue = printer.nozzlePosition.z;
            float endValue = printer.nozzlePosition.z - printer.incrementPosition;
            if (endValue < -203f) endValue = -203f;
            StartCoroutine(MoveToDestination(startValue, endValue, Axis.Z, isMovingOnZ, printerSo.moveDuration));
        }
    }
    public void ReturnHome()
    {
        if (printerSo != null) StartCoroutine(ReturnHomeSequence());
    }

    public void ReturnXHome()
    {
        if (printerSo != null) StartCoroutine(ReturnXHomeSequence());
    }
    public void ReturnYHome()
    {
        if (printerSo != null) StartCoroutine(ReturnYHomeSequence());
    }
    public void ReturnZHome()
    {
        if (printerSo != null) StartCoroutine(ReturnZHomeSequence());
    }

    public void UpOffset()
    {
        
        if (printerSo != null)
        {
            if (isMovingOnZ) return;

            isMovingOnZ = true;

            //update babyStep parameters
            printer.currentBabyStep += printer.babyStepIncrement;
            UpdateBabyStepUI();

            float startValue = printer.nozzlePosition.z;
            float endValue = printer.nozzlePosition.z + printer.babyStepIncrement;

            StartCoroutine(MoveToDestination(startValue, endValue, Axis.Z, isMovingOnZ, 0.5f));
        }

    }
    public void DownOffset()
    {
        if (printerSo != null)
        {
            if (isMovingOnZ) return;

            isMovingOnZ = true;

            //update babyStep parameters
            printer.currentBabyStep -= printer.babyStepIncrement;

            UpdateBabyStepUI();

            float startValue = printer.nozzlePosition.z;
            float endValue = printer.nozzlePosition.z - printer.babyStepIncrement;
            if (endValue < -203f) endValue = -203f;
            StartCoroutine(MoveToDestination(startValue, endValue, Axis.Z, isMovingOnZ, 0.5f));
        }
    }

    public void SaveOffset()
    {
        printer.probeOffset += printer.currentBabyStep;
        printer.currentBabyStep = 0;
        // Add a condition here that checks the right thickness of the material before triggering the event
        GameEventsManager.instance.mainLevelQuests.BabyStepLevelAchieved();
        UpdateBabyStepUI();
    }
    // Used on the reset button and the back button of the BabyStep menu
    public void ResetOffset()
    {
        if (printerSo != null)
        {
            if (isMovingOnZ) return;

            
            if (printer.currentBabyStep != 0)
            {
                isMovingOnZ = true;
                float startValue = printer.nozzlePosition.z;
                float endValue = printer.nozzlePosition.z - printer.currentBabyStep;
                if (endValue < -203f) endValue = -203f;

                //update babyStep parameters
                printer.currentBabyStep = 0;
                UpdateBabyStepUI();

                StartCoroutine(MoveToDestination(startValue, endValue, Axis.Z, isMovingOnZ, 0.5f));
            }
            
        }
    }
    public void Print()
    {
        StartCoroutine(PrintSequence());
        GameEventsManager.instance.PrintEventOccurred();
    }
    // The sequence movement is based on the 3D printer in real life
    private IEnumerator PrintSequence()
    {
        // Maybe add home sequence to assure it starts from there

        // Move the nozzle to bottom left part of the bed
        isMovingOnX = true;
        StartCoroutine(MoveToDestination(printer.nozzlePosition.x, -320f, Axis.X, isMovingOnX, 1.5f));//Change duration values later
        isMovingOnY = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.y, 460f, Axis.Y, isMovingOnY, 1.5f));//Change duration values later
        // Also add time yield return new waitForSeconds to pause the process
        yield return new WaitForSeconds(0.25f);
        // Move the nozzle to the middle part of the bed
        isMovingOnX = true;
        StartCoroutine(MoveToDestination(printer.nozzlePosition.x, 0f, Axis.X, isMovingOnX, 1.5f));//Change duration values later
        isMovingOnY = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.y, 200f, Axis.Y, isMovingOnY, 1.5f));//Change duration values later
        yield return new WaitForSeconds(0.25f);
        // Move the nozzle up and down twice
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -200f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -180f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -200f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -180f, Axis.Z, isMovingOnZ, 0.5f));

        // Move the nozzle to the left middle part of the bed
        isMovingOnX = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.x, -320f, Axis.X, isMovingOnX, 1.5f));//Change duration values later
        yield return new WaitForSeconds(0.25f);
        // Move the nozzle up and down thrice
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -200f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -180f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -200f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -180f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -200f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -180f, Axis.Z, isMovingOnZ, 0.5f));

        // Move the nozzle to the right middle part of the bed
        isMovingOnX = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.x, 300f, Axis.X, isMovingOnX, 2f));//Change duration values later
        yield return new WaitForSeconds(0.25f);
        // Move the nozzle up and down thrice
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -200f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -180f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -200f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -180f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -200f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -180f, Axis.Z, isMovingOnZ, 0.5f));

        // Move the nozzle to the middle part of the bed
        isMovingOnX = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.x, 0f, Axis.X, isMovingOnX, 1f));//Change duration values later
        yield return new WaitForSeconds(0.25f);
        // Move the nozzle up and down twice
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -200f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -180f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -200f, Axis.Z, isMovingOnZ, 0.5f));
        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -180f, Axis.Z, isMovingOnZ, 0.5f));

        // Move the nozzle to the bottom right part of the bed
        isMovingOnX = true;
        StartCoroutine(MoveToDestination(printer.nozzlePosition.x, 300f, Axis.X, isMovingOnX, 1.25f));//Change duration values later
        isMovingOnY = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.y, 460f, Axis.Y, isMovingOnY, 1.25f));//Change duration values later

        isMovingOnZ = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.z, -200f, Axis.Z, isMovingOnZ, 0.5f)); // from here on out add the Probe offset value on the Z axis

        // start ejecting material
        // Add a pause so for the UI to explain babySteps and then go back to ejecting materials
        isMovingOnX = true;
        yield return StartCoroutine(MoveToDestination(printer.nozzlePosition.x, -280f, Axis.X, isMovingOnX, 15f)); // from here on out add the Probe offset value on the Z axis
    }

}
