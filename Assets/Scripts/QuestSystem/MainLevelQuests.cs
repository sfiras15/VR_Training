using System;
using UnityEngine;

public class MainLevelQuests
{
    public event Action onHeatLevelAchieved;
    public void HeatLevelAchieved()
    {
        if (onHeatLevelAchieved != null) onHeatLevelAchieved();
    }


    public event Action onMaterialExtruded;
    public void MaterialExtruded()
    {
        if (onMaterialExtruded != null) onMaterialExtruded();
    }


    public event Action onHomePositionAchieved;
    public void HomePositionAchieved()
    {
        if (onHomePositionAchieved != null) onHomePositionAchieved();
    }
}
