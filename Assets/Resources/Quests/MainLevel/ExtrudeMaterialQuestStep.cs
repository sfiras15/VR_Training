using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeMaterialQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.mainLevelQuests.onMaterialExtruded += MaterialExtruded;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.mainLevelQuests.onMaterialExtruded -= MaterialExtruded;
    }

    private void MaterialExtruded()
    {
        FinishQuestStep();
    }
}
