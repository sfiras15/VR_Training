using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.tutorialEvents.onTeleportLocation += TeleportationOccurred;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.tutorialEvents.onTeleportLocation -= TeleportationOccurred;
    }

    public void TeleportationOccurred()
    {
        FinishQuestStep();
    }
}
