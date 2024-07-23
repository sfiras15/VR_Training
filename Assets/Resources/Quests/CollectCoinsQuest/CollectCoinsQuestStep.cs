using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoinsQuestStep : QuestStep
{
    private int currentCoinsCollected = 0;
    [SerializeField] private int coinsToComplete = 5;

    private void OnEnable()
    {
        GameEventsManager.instance.onGoldGained += CoinsCollected;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.onGoldGained -= CoinsCollected;
    }

    private void CoinsCollected(int value)
    {
        if (currentCoinsCollected < coinsToComplete)
        {
            currentCoinsCollected++;
        }
        
        if (currentCoinsCollected >= coinsToComplete)
        {
            FinishQuestStep();
        }
        
    }
}
