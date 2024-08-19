using System;
using TMPro;
using UnityEngine;

public enum IncrementType
{
    TEMPERATURE,
    EXTRUDED_MATERIAL,
    POSITION
    // add the other increment types later
}

public class Increment : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;
    [field:SerializeField] public IncrementType type { get; private set; }
    public static event Action<IncrementType, float> onIncrementChanged;
    public void UpdateText(string value)
    {
        textUI.text = value;
    }

    public void ChangeIncrement()
    {
        
        if (type == IncrementType.TEMPERATURE)
        {
            int currentValue = int.Parse(textUI.text);
            int newValue;
            switch (currentValue)
            {
                case 1:
                    newValue = 5;
                    break;
                case 5:
                    newValue = 10;
                    break;
                case 10:
                    newValue = 1;
                    break;
                default:
                    newValue = 1;
                    break;
            }

            //Debug.Log($"Invoking onIncrementChanged with type: {type} and newValue: {newValue}");
            onIncrementChanged?.Invoke(type, newValue);
        }
        else if (type == IncrementType.EXTRUDED_MATERIAL)
        {
            float currentValue = float.Parse(textUI.text);

            Debug.Log(currentValue);
            float newValue;
            switch (currentValue)
            {
                case 1.00f:
                    newValue = 5.00f;
                    break;
                case 5.00f:
                    newValue = 10.00f;
                    break;
                case 10.00f:
                    newValue = 1.00f;
                    break;
                default:
                    newValue = 1.00f;
                    break;
            }

            //Debug.Log($"Invoking onIncrementChanged with type: {type} and newValue: {newValue}");
            onIncrementChanged?.Invoke(type, newValue);
        }
        else if (type == IncrementType.POSITION)
        {
            float currentValue = float.Parse(textUI.text);
            float newValue;
            switch (currentValue)
            {
                case 1f:
                    newValue = 10f;
                    break;
                case 10f:
                    newValue = 100f;
                    break;
                case 100f:
                    newValue = 1f;
                    break;
                default:
                    newValue = 1f;
                    break;
            }

            //Debug.Log($"Invoking onIncrementChanged with type: {type} and newValue: {newValue}");
            onIncrementChanged?.Invoke(type, newValue);
        }

    }
}
