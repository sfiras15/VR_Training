using UnityEngine;


/// <summary>
/// Links each quest to it's questInfoSO and holds the state of the quest.
/// Also have various qol functions for the questManager
/// </summary>
public class Quest
{
    public QuestInfoSO info;

    public QuestState state;

    public int currentQuestIndex { get; private set; }

    public Quest(QuestInfoSO questInfo)
    {
        info = questInfo;
        state = QuestState.REQUIREMENT_NOT_MET;
        currentQuestIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentQuestIndex++;
    }

    public bool CurrentStepExists()
    {
        return currentQuestIndex < info.questStepsPrefabs.Length;
    }

    public void InstantiateCurrentQuestStepPrefab(Transform parentTransform)
    {
        GameObject currentQuestStepPrefab = CurrentQuestStepPrefab();

        if (currentQuestStepPrefab != null )
        {
            QuestStep questStep = Object.Instantiate<GameObject>(currentQuestStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestId(info.id);
        }
    }

    public GameObject CurrentQuestStepPrefab()
    {
        GameObject currentQuestStepPrefab = null;
        if (CurrentStepExists())
        {
            currentQuestStepPrefab = info.questStepsPrefabs[currentQuestIndex];
        }
        else
        {
            Debug.LogWarning("No questStep Found");
        }
        return currentQuestStepPrefab;
    }

}
