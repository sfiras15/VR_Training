using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;

    private string questId;

    public void InitializeQuestId(string id)
    {
        questId = id;
    }
    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;

            //Event to advance the quest 

            GameEventsManager.instance.questEvents.AdvanceQuest(questId);

            Destroy(gameObject);    
        }
    }
}
