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

    public event Action onTabletUsed;

    public event Action onPurgeCompleted;
    public event Action onPrintStarted;
    public event Action onPrintCompleted;
    private void Awake()
    {
        if (instance == null) instance = this;  

        questEvents = new QuestEvents();
        tutorialEvents = new TutorialEvents();
        mainLevelQuests = new MainLevelQuests();
    }
    // Main level Events

    public void PurgeCompleted()
    {
        onPurgeCompleted?.Invoke();
    }
    public void PrintStarted()
    {
        onPrintStarted?.Invoke();
    }
    public void PrintCompleted()
    {
        onPrintCompleted?.Invoke();
    }

    // 3D printer Events
    public void MaterialLoadingEventOccurred(bool value)
    {
        mainLevelQuests.MaterialLoaded(value);
    }
    public void HeatEventOccurred()
    {
        mainLevelQuests.HeatLevelAchieved();
    }
    public void MaterialExtrusionEventOccurred()
    {
        mainLevelQuests.MaterialExtruded();
    }
    public void HomeEventOccurred()
    {
        mainLevelQuests.HomePositionAchieved();
    }
    public void PrintPreparationEventOccurred()
    {
        mainLevelQuests.PrintPreparationStarted();
    }
    public void BabyStepEventOccurred()
    {
        mainLevelQuests.BabyStepLevelAchieved();
    }
    public void CooldownEventOccurred()
    {
        mainLevelQuests.CooldownAchieved();
    }

    // Companion Events
    public event Action<int> onMessageRead;
    public void MessageEventOccurred(int index)
    {
        onMessageRead?.Invoke(index);
    }

    // Tablet event

    public void TabletEventOccurred()
    {
        onTabletUsed?.Invoke();
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
