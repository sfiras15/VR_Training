using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MaterialSocket : XRSocketInteractor
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("hello");
        if (GameEventsManager.instance != null) GameEventsManager.instance.MaterialLoadingEventOccurred(true);

    }
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        if (GameEventsManager.instance != null) GameEventsManager.instance.MaterialLoadingEventOccurred(false);

    }


}
