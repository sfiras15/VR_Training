using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum TemperatureType
{
    NOZZLE,
    BED
}

public class Temperature : MonoBehaviour
{

    public TemperatureType type;
    [SerializeField] private TextMeshProUGUI textUI;

    public void UpdateText(string value)
    {
        textUI.text = value;
    }
}
