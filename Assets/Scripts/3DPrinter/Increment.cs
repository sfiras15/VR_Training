using System;
using TMPro;
using UnityEngine;

public enum IncrementType
{
    TEMPERATURE,
    // add the other increment types later
}

public class Increment : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;
    public IncrementType type { get; private set; }
    public static event Action<IncrementType, int> onIncrementChanged;
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

            Debug.Log($"Invoking onIncrementChanged with type: {type} and newValue: {newValue}");
            onIncrementChanged?.Invoke(type, newValue);
        }
    }
}
