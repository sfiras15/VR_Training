using System;
using System.Collections;
using UnityEngine;

public class PrinterUI : MonoBehaviour
{
    [SerializeField] private Printer_SO printerSo;
    private Printer printer;

    private Temperature[] temperatures;
    private Increment[] increments;
    private ExtrudedMaterial extrudedMaterial;
    private Position[] positions;

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
    private IEnumerator moveNozzlePosition(Axis axis, float startValue, float endValue)
    {
        float timeElapsed = 0f;
        while (timeElapsed < printerSo.moveDuration)
        {
            SetAxisValue(ref printer.nozzlePosition, axis, Mathf.Lerp(startValue, endValue, timeElapsed / printerSo.moveDuration));

            timeElapsed += Time.deltaTime;

            UpdateNozzlePositionUI();
            yield return null;
        }

        SetAxisValue(ref printer.nozzlePosition, axis, endValue);
        UpdateNozzlePositionUI();
    }
    private float GetAxisValue(Vector3 position, Axis axis)
    {
        switch (axis)
        {
            case Axis.X: return position.x;
            case Axis.Y: return position.y;
            case Axis.Z: return position.z;
            default: throw new ArgumentException("Invalid axis");
        }
    }

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
        float startValue = GetAxisValue(printer.nozzlePosition, Axis.Y);
        float endValue = printer.homePosition.y;

        printerSo.Move(Axis.Z, endValue - startValue);
        // add position limit based on the 3D printer
        yield return StartCoroutine(moveNozzlePosition(Axis.Y, startValue, endValue));
        isMovingOnY = false;
    }

    private IEnumerator ReturnXHomeSequence()
    {
        if (isMovingOnX) yield break;

        isMovingOnX = true;
        float startValue = GetAxisValue(printer.nozzlePosition, Axis.X);
        float endValue = printer.homePosition.x;

        printerSo.Move(Axis.X, endValue - startValue);
        // add position limit based on the 3D printer
        yield return StartCoroutine(moveNozzlePosition(Axis.X, startValue, endValue));
        isMovingOnX = false;
    }

    private IEnumerator ReturnZHomeSequence()
    {
        if (isMovingOnZ) yield break;

        isMovingOnZ = true;
        float startValue = GetAxisValue(printer.nozzlePosition, Axis.Z);
        float endValue = printer.homePosition.z;

        printerSo.Move(Axis.Y, endValue - startValue);
        // add position limit based on the 3D printer
        yield return StartCoroutine(moveNozzlePosition(Axis.Z, startValue, endValue));
        isMovingOnZ = false;
    }
    private IEnumerator ResetBool(bool value)
    {
        //Debug.Log("Reset");
        yield return new WaitForSeconds(printerSo.moveDuration);
        if (value == isMovingOnX) isMovingOnX = false;
        else if (value == isMovingOnY) isMovingOnY = false;
        else isMovingOnZ = false;

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
            float startValue = GetAxisValue(printer.nozzlePosition, Axis.X);
            float endValue = GetAxisValue(printer.nozzlePosition, Axis.X) + printer.incrementPosition;
            // add position limit based on the 3D printer
            if (endValue > 400f) endValue = 400f;

            float incrementValue = MathF.Abs(endValue - startValue);
            float sign = endValue - startValue < 0 ? -1 : 1;
            printerSo.Move(Axis.X, sign * incrementValue); // Move value
            StartCoroutine(moveNozzlePosition(Axis.X, startValue, endValue));
            StartCoroutine(ResetBool(isMovingOnX));
        }
    }

    public void IncreaseYPosition()
    {
        if (printerSo != null)
        {
            if (isMovingOnY) return;

            isMovingOnY = true;
            float startValue = GetAxisValue(printer.nozzlePosition, Axis.Y);
            float endValue = GetAxisValue(printer.nozzlePosition, Axis.Y) + printer.incrementPosition;
            if (endValue > 550f) endValue = 550f;
            float incrementValue = MathF.Abs(endValue - startValue);
            float sign = endValue - startValue < 0 ? -1 : 1;


            printerSo.Move(Axis.Z, sign * incrementValue); // Move value
            StartCoroutine(moveNozzlePosition(Axis.Y, startValue, endValue));

            // add position limit based on the 3D printer
            StartCoroutine(ResetBool(isMovingOnY));
        }
    }
    public void IncreaseZPosition()
    {
        if (printerSo != null)
        {
            if (isMovingOnZ) return;

            isMovingOnZ = true;
            float startValue = GetAxisValue(printer.nozzlePosition, Axis.Z);
            float endValue = GetAxisValue(printer.nozzlePosition, Axis.Z) + printer.incrementPosition;
            if (endValue > 810f) endValue = 810f;
            float incrementValue = MathF.Abs(endValue - startValue);
            float sign = endValue - startValue < 0 ? -1 : 1;

            printerSo.Move(Axis.Y, sign * incrementValue); // Move value
            StartCoroutine(moveNozzlePosition(Axis.Z, startValue, endValue));

            // add position limit based on the 3D printer
            StartCoroutine(ResetBool(isMovingOnZ));
        }
    }
    public void DecreaseXPosition()
    {
        if (printerSo != null)
        {
            if (isMovingOnX) return;

            isMovingOnX = true;
            float startValue = GetAxisValue(printer.nozzlePosition, Axis.X);
            float endValue = GetAxisValue(printer.nozzlePosition, Axis.X) - printer.incrementPosition;
            if (endValue < -320f) endValue = -320f;
            float incrementValue = MathF.Abs(endValue - startValue);
            float sign = endValue - startValue < 0 ? -1 : 1;

            printerSo.Move(Axis.X, sign * incrementValue); // Move value
            StartCoroutine(moveNozzlePosition(Axis.X, startValue, endValue));

            // add position limit based on the 3D printer
            StartCoroutine(ResetBool(isMovingOnX));
        }
    }
    public void DecreaseYPosition()
    {
        if (printerSo != null)
        {
            if (isMovingOnY) return;

            isMovingOnY = true;
            float startValue = GetAxisValue(printer.nozzlePosition, Axis.Y);
            float endValue = GetAxisValue(printer.nozzlePosition, Axis.Y) - printer.incrementPosition;
            if (endValue < -350f) endValue = -350f;
            float incrementValue = MathF.Abs(endValue - startValue);
            float sign = endValue - startValue < 0 ? -1 : 1;

            printerSo.Move(Axis.Z, sign * incrementValue); // Move value
            StartCoroutine(moveNozzlePosition(Axis.Y, startValue, endValue));

            // add position limit based on the 3D printer
            StartCoroutine(ResetBool(isMovingOnY));
        }
    }
    public void DecreaseZPosition()
    {
        if (printerSo != null)
        {
            if (isMovingOnZ) return;

            isMovingOnZ = true;
            float startValue = GetAxisValue(printer.nozzlePosition, Axis.Z);
            float endValue = GetAxisValue(printer.nozzlePosition, Axis.Z) - printer.incrementPosition;
            if (endValue < -203f) endValue = -203f;
            float incrementValue = MathF.Abs(endValue - startValue);
            float sign = endValue - startValue < 0 ? -1 : 1;
            
            printerSo.Move(Axis.Y, sign * incrementValue); // Move value
            StartCoroutine(moveNozzlePosition(Axis.Z, startValue, endValue));

            // add position limit based on the 3D printer
            StartCoroutine(ResetBool(isMovingOnZ));
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
}
