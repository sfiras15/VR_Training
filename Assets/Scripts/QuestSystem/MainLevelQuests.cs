using System;
using UnityEngine;

public class MainLevelQuests : MonoBehaviour
{
    public event Action onHeatLevelAchieved;
    
    public void HeatLevelAchieved()
    {
        if (onHeatLevelAchieved != null) onHeatLevelAchieved();
    }
}
