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

    public InputActionProperty joystickMovementAction;

    private void OnEnable()
    {
        if (locomotionSo != null) { locomotionSo.onGrabbing += ToggleSnapTurn; }
        joystickMovementAction.action.Enable();
        joystickMovementAction.action.performed += JoystickMoved;
    }

    private void OnDisable()
    {
        if (locomotionSo != null) { locomotionSo.onGrabbing -= ToggleSnapTurn; }
        joystickMovementAction.action.Disable();
        joystickMovementAction.action.performed -= JoystickMoved;
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

    private void JoystickMoved(InputAction.CallbackContext context)
    {
        Debug.Log("testing");
        var joystickValue = context.ReadValue<Vector2>();
        // Add so that when a certain threshhold is met we activate the action of teleporting
        Debug.Log(joystickValue);
    }
}
