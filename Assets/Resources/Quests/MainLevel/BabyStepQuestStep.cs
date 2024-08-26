using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyStepQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.mainLevelQuests.onBabyStepLevelAchieved += BabyStepAchieved;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.mainLevelQuests.onBabyStepLevelAchieved -= BabyStepAchieved;
    }

    private void BabyStepAchieved()
    {
        FinishQuestStep();
    }
}
