using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Interactables : XRGrabInteractable
{
    [SerializeField] private bool _held = false;
    [SerializeField] private Locomotion_SO locomotionSo;

    private XRBaseController _controller;
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        var interactor = args.interactorObject as XRBaseControllerInteractor;
        _controller = interactor.xrController;
        locomotionSo.Controller = _controller;
        locomotionSo.Held = true;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        var interactor = args.interactorObject as XRBaseControllerInteractor;
        _controller = interactor.xrController;
        locomotionSo.Controller = _controller;
        locomotionSo.Held = false;
    }


}
