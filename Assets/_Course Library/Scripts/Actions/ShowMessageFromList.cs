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

    [Header("For Dialogue Options")]
    // With some testing these values give the best result
    [Tooltip("The maximum length of characters that can be shown at once")]
    [SerializeField] private int maxLengthPerMessage = 175;

    [Tooltip("The delay between showing parts of a message")]
    [SerializeField] private float messageDelay = 10.5f;

    private void OnEnable()
    {
        if (GameEventsManager.instance != null) GameEventsManager.instance.mainLevelQuests.onMessageRead += ShowMessageWithParts;
    }

    private void OnDisable()
    {
        if (GameEventsManager.instance != null) GameEventsManager.instance.mainLevelQuests.onMessageRead -= ShowMessageWithParts;
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
    // New method to show a message with parts
    public void ShowMessageWithParts(int messageIndex)
    {
        StartCoroutine(DisplayMessageWithParts(messageIndex));
    }

    private IEnumerator DisplayMessageWithParts(int messageIndex)
    {
        if (messageIndex < 0 || messageIndex >= messages.Count)
        {
            yield break; // Invalid index, exit the coroutine
        }

        string message = messages[messageIndex];
        int totalLength = message.Length;
        int currentIndex = 0;

        while (currentIndex < totalLength)
        {
            // Determine the maximum length for this part
            int length = Mathf.Min(maxLengthPerMessage, totalLength - currentIndex);

            // Ensure we don't cut off in the middle of a word
            if (currentIndex + length < totalLength && !char.IsWhiteSpace(message[currentIndex + length]))
            {
                // Move the length back to the last space
                int lastSpace = message.LastIndexOf(' ', currentIndex + length);
                if (lastSpace > currentIndex)
                {
                    length = lastSpace - currentIndex;
                }
            }

            // Extract and show this part of the message
            string partMessage = message.Substring(currentIndex, length).TrimStart(); // Trim leading whitespace
            messageOutput.text = partMessage;

            // Update the current index
            currentIndex += length;

            // Wait for the specified delay if there are more parts
            if (currentIndex < totalLength)
            {
                yield return new WaitForSeconds(messageDelay);
            }
        }
    }

}
