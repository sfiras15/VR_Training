using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Tablet : Interactables
{
    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        if (locomotionSo != null)
        {
            if (locomotionSo.Held)
            {
                if (GameEventsManager.instance != null) GameEventsManager.instance.TabletEventOccurred();
            }
        }

    }
}
