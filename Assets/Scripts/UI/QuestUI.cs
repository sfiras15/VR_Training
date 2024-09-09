using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Updates the UI relatives to the currentQuest in questInfoSO.
/// </summary>

public class QuestUI : MonoBehaviour
{
    [SerializeField] private QuestStepUI questStepUiPrefab;
    [SerializeField] private TextMeshProUGUI questName;

    [Header("Where we will instantiate our questSteps")]
    [SerializeField] private Transform questStepsTransform;

    [SerializeField] private QuestUiSo questUISo;

    private Quest quest;

    private void OnEnable()
    {
        if (questUISo != null)
        {
            questUISo.onQuestStarted += loadUI;
            questUISo.onQuestAdvanced += UpdateUI;
            
        }
        GameEventsManager.instance.questEvents.onFinishQuest += ClearUI;

    }

    private void OnDisable()
    {
        if (questUISo != null)
        {
            questUISo.onQuestStarted -= loadUI;
            questUISo.onQuestAdvanced -= UpdateUI;
        }
        GameEventsManager.instance.questEvents.onFinishQuest += ClearUI;
    }
    private void loadUI()
    {
        if (questUISo != null)
        {
            quest = questUISo.currentQuest;
            var currentIndex = 1;
            questName.text = quest.info.displayName;
            foreach (var questStepPrefab in quest.info.questStepsPrefabs)
            {
                QuestStep questStep = questStepPrefab.GetComponent<QuestStep>();

                QuestStepUI questUiObj = Object.Instantiate<QuestStepUI>(questStepUiPrefab, questStepsTransform);
                questUiObj.Init(questStep.questStepDescription, currentIndex-1);
                currentIndex++;


            }
        }
    }
    private void UpdateUI(int index)
    {
        if (questUISo != null)
        {
            // Get all existing QuestStepUI instances
            var questStepUis = questStepsTransform.GetComponentsInChildren<QuestStepUI>();


            foreach (var questStepUi in questStepUis)
            {
                // Find the corresponding QuestStepUI instance
                if (index == questStepUi.QuestIndex)
                {
                    string description = questStepUi.DescriptionText;
                    questStepUi.UpdateStepUI($"<s>" + description + "</s>");
                }
            }
        }
    }
    public void ClearUI(string id)
    {
        for (int i = questStepsTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(questStepsTransform.GetChild(i).gameObject);
        }
        questName.text = "Quest name";
    }
}