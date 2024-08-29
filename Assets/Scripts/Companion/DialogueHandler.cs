using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    private PlaySoundsFromList soundHandler;

    private void Awake()
    {
        TryGetComponent(out soundHandler);
    }
    public void PlaySound(int index)
    {
        if (soundHandler != null) 
        {
            // this will show subtitles for the read message
            GameEventsManager.instance.MessageEventOccurred(index);
            soundHandler.PlayAtIndex(index);

            
        }

        
    }
    // Update is called once per frame
    void Update()
    {
        // For testing only remove later
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // this will show subtitles for the read message
            GameEventsManager.instance.MessageEventOccurred(soundHandler.CurrentIndex);
            soundHandler.PlaySound();

            
        }
    }
}
