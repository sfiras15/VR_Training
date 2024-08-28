using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMaterialQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.mainLevelQuests.onMaterialLoaded += MaterialLoadingAchieved;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.mainLevelQuests.onMaterialLoaded -= MaterialLoadingAchieved;
    }

    private void MaterialLoadingAchieved(bool value)
    {
        FinishQuestStep();
    }
}
