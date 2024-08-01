using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Shows an ordered list of messages via a text mesh
/// </summary>
public class ShowMessageFromList : MonoBehaviour
{
    [Tooltip("The text mesh the message is output to")]
    public TextMeshProUGUI messageOutput = null;

    // What happens once the list is completed
    public UnityEvent OnComplete = new UnityEvent();

    [Tooltip("The list of messages that are shown")]
    [TextArea] public List<string> messages = new List<string>();

    private int index = -1;

    private bool isClicked = false;

    private void Start()
    {
        //ShowMessage();
    }

    public void NextMessage()
    {
        int newIndex = ++index % messages.Count;

        if (newIndex < index)
        {
            OnComplete.Invoke();
        }
        else
        {
            ShowMessage();
        }
    }

    // this method is only called for the tablet in the tutorial as a quick fix
    public void ShowContinuousText()
    {
        if (!isClicked)
        {
            isClicked = true;
            StartCoroutine(ShowMessagesRepeatedly());
        }
        
    }

    private IEnumerator ShowMessagesRepeatedly()
    {
        while (true)
        {
            int newIndex = ++index % messages.Count;

            if (newIndex < index)
            {
                OnComplete.Invoke();
                yield break; // Stop the coroutine if OnComplete is invoked
            }
            else
            {
                ShowMessage();
            }

            yield return new WaitForSeconds(5.5f); // the time it takes for the tts to read each paragraph
        }
    }

    public void PreviousMessage()
    {
        index = --index % messages.Count;
        ShowMessage();
    }

    private void ShowMessage()
    {
        messageOutput.text = messages[Mathf.Abs(index)];
    }

    public void ShowMessageAtIndex(int value)
    {
        index = value;
        ShowMessage();
    }
}
