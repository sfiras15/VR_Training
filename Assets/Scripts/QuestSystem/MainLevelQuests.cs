using System;
using UnityEngine;

public class MainLevelQuests
{
    // Event for loading the material in the socket of the printer
    public event Action<bool> onMaterialLoaded;
    public void MaterialLoaded(bool value)
    {
        if (onMaterialLoaded != null) onMaterialLoaded(value);
    }

    // Event for heating the bed / nozzle to the right temperature
    public event Action onHeatLevelAchieved;
    public void HeatLevelAchieved()
    {
        if (onHeatLevelAchieved != null) onHeatLevelAchieved();
    }

    // Event for extruding material from the printer
    public event Action onMaterialExtruded;
    public void MaterialExtruded()
    {
        if (onMaterialExtruded != null) onMaterialExtruded();
    }

    // Event for checking the home position of the printer

    public event Action onHomePositionAchieved;
    public void HomePositionAchieved()
    {
        if (onHomePositionAchieved != null) onHomePositionAchieved();
    }

    // Event for starting to print
    public event Action onPrintStarted;
    public void PrintStarted()
    {
        if (onPrintStarted != null) onPrintStarted();
    }

    // Event for adjusting the zAxis during print
    public event Action onBabyStepLevelAchieved;
    public void BabyStepLevelAchieved()
    {
        if (onBabyStepLevelAchieved != null) onBabyStepLevelAchieved();
    }

    // Event for broadcasting the current read message by the companion
    public event Action<int> onMessageRead;
    public void MessageRead(int index)
    {
        if (onMessageRead != null) onMessageRead(index);
    }
}
