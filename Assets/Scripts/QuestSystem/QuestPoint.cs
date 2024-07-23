using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour
{
    private bool isNearPlayer = false;

    [SerializeField] private KeyCode interactKey;
    [Header("Quest")]

    [SerializeField] QuestInfoSO questInfo;

    private QuestState currentQuestState;
    private string questId;

    [Header("Config")]

    [SerializeField] private bool startPoint;
    [SerializeField] private bool finishPoint;




    private void Awake()
    {
        questId = questInfo.id;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearPlayer = false;
        }
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
            Debug.Log("quest with id: " + questId + " updated to state : " + currentQuestState);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //interact();
        if (Input.GetKeyDown(interactKey))
        {
            StartQuest();
        }
        
    }

    public void interact()
    {
        if (!isNearPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(interactKey))
        {
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                GameEventsManager.instance.questEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                GameEventsManager.instance.questEvents.FinishQuest(questId);
            }

           //GameEventsManager.instance.questEvents.AdvanceQuest(questId);

        } 
    }

    // If called on scene load it will not work because of the order of execution (scene load happens before the update method of the questManager)
    // you have to force start it in order to work
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
