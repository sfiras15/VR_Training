using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Can_Start , CAN_Finish can be used if we plan to add NPCS in the game where we might talk to them
// to get/finish the quest.
public enum QuestState
{
    REQUIREMENT_NOT_MET,
    CAN_START,
    IN_PROGRESS,
    CAN_FINISH,
    FINISHED
}
