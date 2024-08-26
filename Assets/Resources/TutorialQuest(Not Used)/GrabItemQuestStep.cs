using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItemQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.tutorialEvents.onItemGrabbed += ItemGrabbed;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.tutorialEvents.onItemGrabbed -= ItemGrabbed;
    }

    public void ItemGrabbed()
    {
        FinishQuestStep();
    }
}
