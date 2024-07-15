using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionManager : MonoBehaviour
{
    [SerializeField] private ActionBasedSnapTurnProvider snapTurnProvide;
    [SerializeField] private Locomotion_SO locomotionSo;
    [SerializeField] private XRBaseController leftController;
    [SerializeField] private XRBaseController rightController;



    private void OnEnable()
    {
        if (locomotionSo != null) { locomotionSo.onGrabbing += ToggleSnapTurn; }
    }

    private void OnDisable()
    {
        if (locomotionSo != null) { locomotionSo.onGrabbing -= ToggleSnapTurn; }
    }

    public void ToggleSnapTurn(XRBaseController controller,bool value)
    {
        if (controller == leftController)
        {
            InputAction action = snapTurnProvide.leftHandSnapTurnAction.action;
            if (!value) action.Enable();
            else action.Disable();
        }
        if (controller == rightController)
        {
            InputAction action = snapTurnProvide.rightHandSnapTurnAction.action;
            if (!value) action.Enable();
            else action.Disable();
        }

    }
}
