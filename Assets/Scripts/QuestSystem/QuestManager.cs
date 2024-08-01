using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;


/// <summary>
/// This class is responsible for holding reference to all the quests that are in the game.
/// Also responsible for starting,updating,finishing the quests all while changing their appropriate info
/// </summary>
public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;

    [SerializeField] private QuestInfoSO[] questInfoSOs;

    [SerializeField] private QuestUiSo questUiSO;

    private int currentPlayerLevel;
    private void Awake()
    {
        questMap = CreateQuestMap();
        //var quest = GetQuestById("TutorialQuest");
        //Debug.Log(quest.info.displayName);
        //Debug.Log(quest.info.goldReward);
    }


    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
    }
    public void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);

        if (questUiSO != null)
        {
            questUiSO.currentQuest = quest;
            questUiSO.StartQuestUI();
        }
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
        quest.InstantiateCurrentQuestStepPrefab(this.transform);
    }
    public void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);

        
        if (questUiSO != null)
        {
            questUiSO.currentQuest = quest;
            questUiSO.UpdateQuestUI(quest.currentQuestIndex);
        }
        quest.MoveToNextStep();
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStepPrefab(transform);
        }
        else
        {
            //ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
            //FinishQuest(id);

            // trigger this event to update the UI and update the state of the quests
            GameEventsManager.instance.questEvents.FinishQuest(quest.info.id);
        }
        Debug.Log("advance quest : " + id);
    }
    public void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
        ClaimReward(quest.info.xpReward);
        //Debug.Log("finish quest : " + id+ " QuestState : " + quest.state);
    }

    // add rewards layer , maybe score or something 
    private void ClaimReward(int value)
    {
        
    }
    private void Start()
    {
        //Broadcast the state of all the quest on startup
        foreach(var quest in questMap.Values)
        {
            GameEventsManager.instance.questEvents.StateChangeQuest(quest);
        }
    }
    private Dictionary<string, Quest> CreateQuestMap()
    {
        questInfoSOs = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        foreach(QuestInfoSO quest in questInfoSOs)
        {
            if (!idToQuestMap.ContainsKey(quest.id))
            {
                idToQuestMap.Add(quest.id, new Quest(quest));
            }
            else
            {
                Debug.LogWarning("Found duplicate Quest id in the dictionary : " +  quest.id);
            }
        }
        //if (idToQuestMap != null) Debug.Log("Map successfull");
        return idToQuestMap;

    }
    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("No quest found with this id :" + id);
        }
        return quest;
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id); 
        quest.state = state;
        GameEventsManager.instance.questEvents.StateChangeQuest(quest);
    }
    private void PlayerLevelChange(int level)
    {
        currentPlayerLevel = level;
    }

    private bool CheckRequirements(Quest quest)
    {
        bool playerMeetsRequirement = true;

        // Check if the player's level higher than the levelRequirement 
        if (quest.info.playerLevelRequirement > currentPlayerLevel)
        {
            playerMeetsRequirement = false;
        }

        // Check if the pre requisite quests are finished 
        foreach(QuestInfoSO questInfo in quest.info.preRequisiteQuests)
        {
            Quest preReqQuest = GetQuestById(questInfo.id);

            if (preReqQuest.state != QuestState.FINISHED)
            {
                playerMeetsRequirement = false;
            }
        }

        // Add any other prerequisites you like in the questInfoSO
        return playerMeetsRequirement;
    }
    private void Update()
    {
        foreach(Quest quest in questMap.Values)
        {
            if ((quest.state == QuestState.REQUIREMENT_NOT_MET) && CheckRequirements(quest))
            {
                //ChangeQuestState(quest.info.id, QuestState.CAN_START);
                // so that the UI loads the current active quest;
                if (questUiSO != null) questUiSO.currentQuest = quest;
                StartQuest(quest.info.id);
                
                //Debug.Log("Quest : " + quest.info.displayName + " Status : " + quest.state);
            }
        }
    }
}
