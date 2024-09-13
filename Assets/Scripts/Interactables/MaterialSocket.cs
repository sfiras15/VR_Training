using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MaterialSocket : XRSocketInteractor
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (GameEventsManager.instance != null) GameEventsManager.instance.MaterialLoadingEventOccurred(true);
        IXRSelectInteractable interactableObject = args.interactableObject;

        if (interactableObject != null)
        {
            Interactables interactables = interactableObject.transform.GetComponent<Interactables>();
            if (interactables != null) interactables.SocketEnteredMethod(true);
        }
    }
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        if (GameEventsManager.instance != null) GameEventsManager.instance.MaterialLoadingEventOccurred(false);
        IXRSelectInteractable interactableObject = args.interactableObject;

        if (interactableObject != null)
        {
            Interactables interactables = interactableObject.transform.GetComponent<Interactables>();
            if (interactables != null) interactables.SocketEnteredMethod(false);
        }
    }
}
