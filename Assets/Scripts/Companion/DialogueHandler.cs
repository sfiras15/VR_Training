using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private PlaySoundsFromList soundHandler;

    private void Awake()
    {
        TryGetComponent(out soundHandler);
    }
    public void PlaySound()
    {
        if (soundHandler != null) soundHandler.PlaySound();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
