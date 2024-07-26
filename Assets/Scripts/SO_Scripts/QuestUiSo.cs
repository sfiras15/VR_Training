using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/QuestUI", order = 2)]
public class QuestUiSo : ScriptableObject
{
    [field:SerializeField] public Quest currentQuest {  get; set; }

    public event Action onQuestStarted;
    public event Action<int> onQuestAdvanced;

    public event Action onQuestFinished;
    


    public void StartQuestUI()
    {
        if (onQuestStarted != null) onQuestStarted();
    }
    public void UpdateQuestUI(int index)
    {
        if (onQuestAdvanced != null) onQuestAdvanced(index);
    }

    public void FinishQuestUI()
    {
        if (onQuestFinished != null) onQuestFinished();
    }



}
