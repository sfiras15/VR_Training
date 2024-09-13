using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Filament : Interactables
{
    [SerializeField] private GameObject cableGameObject;

    private void Start()
    {
        ToggleCable(false);
    }
    private void ToggleCable(bool value)
    {
        cableGameObject.SetActive(value);
    }
    public override void SocketEnteredMethod(bool isEntered) 
    {
        ToggleCable(isEntered);
    }
}
