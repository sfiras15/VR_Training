using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance;

    public QuestEvents questEvents;

    // this was for testing  the quest system mostly, it's not in the tutorial anymore
    public TutorialEvents tutorialEvents;
    private void Awake()
    {
        if (instance == null) instance = this;  

        questEvents = new QuestEvents();
        tutorialEvents = new TutorialEvents();
    }
    public void TeleportationOccurred()
    {
        tutorialEvents.LocationChanged();
    }
    public void ItemGrabbed()
    {
        tutorialEvents.ItemGrabbed();
    }
    public void ItemActivated()
    {
        tutorialEvents.ItemActivated();
    }
}
