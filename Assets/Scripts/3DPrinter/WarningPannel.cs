using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPanel : MonoBehaviour
{
    private ShowMessageFromList showMessageFromList;

    private void Awake()
    {
        showMessageFromList = GetComponentInChildren<ShowMessageFromList>(true);
    }
    public void ShowWarning(int index)
    {
        gameObject.SetActive(true);
        if (showMessageFromList != null) showMessageFromList.ShowMessageAtIndex(index);
    }
}
