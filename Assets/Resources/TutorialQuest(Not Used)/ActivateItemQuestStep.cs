using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateItemQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.tutorialEvents.onItemActivated += ItemActivated;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.tutorialEvents.onItemActivated -= ItemActivated;
    }

    public void ItemActivated()
    {
        FinishQuestStep();
    }
}
