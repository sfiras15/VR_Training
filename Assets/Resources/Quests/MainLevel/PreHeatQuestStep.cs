using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreHeatQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.mainLevelQuests.onHeatLevelAchieved += HeatAchieved;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.mainLevelQuests.onHeatLevelAchieved -= HeatAchieved;
    }

    private void HeatAchieved()
    {
        FinishQuestStep();
    }
}
