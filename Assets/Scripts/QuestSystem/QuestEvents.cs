using System;


public class QuestEvents
{
    public event Action<string> onStartQuest;

    public void StartQuest(string id)
    {
        if (onStartQuest != null) onStartQuest(id);
    }


    public event Action<string> onAdvanceQuest;

    public void AdvanceQuest(string id)
    {
        if (onAdvanceQuest != null) onAdvanceQuest(id);
    }

    public event Action<string> onFinishQuest;

    public void FinishQuest(string id)
    {
        if (onFinishQuest != null) onFinishQuest(id);
    }

    public event Action<Quest> onStateChangeQuest;

    public void StateChangeQuest(Quest quest)
    {
        if (onStateChangeQuest != null) onStateChangeQuest(quest);
    }
}
