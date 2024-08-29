using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Interactables : XRGrabInteractable
{
    [SerializeField] protected Locomotion_SO locomotionSo;

    private XRBaseController _controller;


    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        var interactor = args.interactorObject as XRBaseControllerInteractor;
        _controller = interactor.xrController;
        if (locomotionSo != null )
        {
            locomotionSo.Controller = _controller;
            locomotionSo.Held = true;
        }

        
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        var interactor = args.interactorObject as XRBaseControllerInteractor;
        _controller = interactor.xrController;
        if (locomotionSo != null)
        {
            locomotionSo.Controller = _controller;
            locomotionSo.Held = false;
        }
    }

    // This method will be changed based on the class that inherits it
    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        //if (locomotionSo != null)
        //{
        //    if (locomotionSo.Held) Debug.Log("true");
        //}

    }


}
