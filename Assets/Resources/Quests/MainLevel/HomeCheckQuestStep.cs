using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCheckQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.mainLevelQuests.onHomePositionAchieved += HomePositionAchieved;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.mainLevelQuests.onHomePositionAchieved -= HomePositionAchieved;
    }

    private void HomePositionAchieved()
    {
        FinishQuestStep();
    }
}
