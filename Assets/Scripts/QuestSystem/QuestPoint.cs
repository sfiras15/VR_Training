using UnityEngine;

/// <summary>
/// Starts the current questInfo quest, 
/// </summary>
public class QuestPoint : MonoBehaviour
{

    [SerializeField] private KeyCode interactKey;
    [Header("Quest")]

    [SerializeField] QuestInfoSO questInfo;

    private QuestState currentQuestState;
    private string questId;


    // In case we want to add npc for starting/finishing the quest
    [Header("Config")]

    [SerializeField] private bool startPoint;
    [SerializeField] private bool finishPoint;

    private void Awake()
    {
        questId = questInfo.id;
    }
    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onStateChangeQuest += ChangeQuestState;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStateChangeQuest -= ChangeQuestState;
    }

    public void ChangeQuestState(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
           // Debug.Log("quest with id: " + questId + " updated to state : " + currentQuestState);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // this was done for testing
        //if (Input.GetKeyDown(interactKey))
        //{
        //    StartQuest();
        //}
        
    }

    // If called on scene load it will not work because of the order of execution (scene load happens before the update method of the questManager)
    // you have to force start it in order to work

    // this function is called from the welcomeCanvas onSceneLoad, might change it later
    public void StartQuest()
    {
        //Debug.Log("starting");
        
        if (currentQuestState.Equals(QuestState.CAN_START))
        {
            GameEventsManager.instance.questEvents.StartQuest(questId);
        }
        //GameEventsManager.instance.questEvents.StartQuest(questId);

    }
}
