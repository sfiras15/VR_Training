using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BabyStep : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI babyStepText;
    [SerializeField] private TextMeshProUGUI probeOffsetText;
    public virtual void UpdateText(float babyStepValue,float offsetValue)
    {
        babyStepText.text = $"{babyStepValue:F2}";
        probeOffsetText.text = $"{offsetValue:F2}";
    }
}
