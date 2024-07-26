using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    [field: SerializeField] public string questStepDescription { get; private set; }
    public  bool isFinished { get; private set; }

    protected string questId;

    public void InitializeQuestId(string id)
    {
        questId = id;
    }

    public string Id
    {
        get { return questId; }
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
