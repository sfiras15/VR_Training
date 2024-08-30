using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownPrinterQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.mainLevelQuests.onPrinterCooledDown += CooldownCompleted;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.mainLevelQuests.onPrinterCooledDown -= CooldownCompleted;
    }

    private void CooldownCompleted()
    {
        FinishQuestStep();
    }
}
