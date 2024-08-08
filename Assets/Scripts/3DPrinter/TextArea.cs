using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextArea : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI textUI;
    public void UpdateText(string value)
    {
        textUI.text = value;
    }
}
