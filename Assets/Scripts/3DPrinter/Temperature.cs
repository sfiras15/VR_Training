using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum TemperatureType
{
    NOZZLE,
    BED
}

public class Temperature : TextArea
{

    public TemperatureType type;
}
