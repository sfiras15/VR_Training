using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance;

    public event Action<int> onGoldGained;



    public QuestEvents questEvents;

    public TutorialEvents tutorialEvents;
    

    private int goldAmount;
    private void Awake()
    {
        if (instance == null) instance = this;  

        questEvents = new QuestEvents();
        tutorialEvents = new TutorialEvents();
    }

    public void GoldGained(int value)
    {
        if (onGoldGained != null)
        {
            goldAmount += value;
            onGoldGained(value);
        }
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
