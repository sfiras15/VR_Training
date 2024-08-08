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

    public MainLevelQuests mainLevelQuests;
    private void Awake()
    {
        if (instance == null) instance = this;  

        questEvents = new QuestEvents();
        tutorialEvents = new TutorialEvents();
        mainLevelQuests = new MainLevelQuests();
    }
    // Main level Events
    public void HeatEventOccurred()
    {
        mainLevelQuests.HeatLevelAchieved();
    }
    public void MaterialEventOccurred()
    {
        mainLevelQuests.MaterialExtruded();
    }
    public void HomeEventOccurred()
    {
        mainLevelQuests.HomePositionAchieved();
    }


    // Tutorial level Events
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
    public void WelcomeCanvas()
    {
        tutorialEvents.WelcomeCanvasFinished();
    }
}
