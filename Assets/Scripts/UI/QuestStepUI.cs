using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestStepUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stepText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private int questIndex;

    public string DescriptionText
    {
        get { return descriptionText.text; }
    }
    public int QuestIndex
    {
        get { return questIndex; }
    }
    public void Init(string step,string description,int index)
    {
        stepText.text = step;
        descriptionText.text = description;
        questIndex = index;
    }

    public void UpdateStepUI(string description)
    {
        descriptionText.text = description;
    }


}
