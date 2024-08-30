using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintingQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.mainLevelQuests.onPrintPreparationStarting += PrintingAchieved;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.mainLevelQuests.onPrintPreparationStarting -= PrintingAchieved;
    }

    private void PrintingAchieved()
    {
        FinishQuestStep();
    }
}
