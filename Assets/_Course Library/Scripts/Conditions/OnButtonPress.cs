using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Checks for button input on an input action
/// </summary>
public class OnButtonPress : MonoBehaviour
{
    [Tooltip("Actions to check")]
    public InputAction action = null;

    // When the button is pressed
    public UnityEvent OnPress = new UnityEvent();

    // When the button is released
    public UnityEvent OnRelease = new UnityEvent();

    private bool allowPress;

    private void Awake()
    {
        allowPress = false;
        action.started += Pressed;
        action.canceled += Released;
    }

    private void OnDestroy()
    {
        action.started -= Pressed;
        action.canceled -= Released;
    }

    private void OnEnable()
    {
        action.Enable();
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.tutorialEvents.onWelcomeCanvasFinished += AllowPressStatus;
        }
    }

    private void OnDisable()
    {
        action.Disable();
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.tutorialEvents.onWelcomeCanvasFinished -= AllowPressStatus;
        }
    }


    private void AllowPressStatus()
    {
        allowPress = true;
    }
    private void Pressed(InputAction.CallbackContext context)
    {
        if (allowPress)
            OnPress.Invoke();
    }

    private void Released(InputAction.CallbackContext context)
    {
        if (allowPress)
            OnRelease.Invoke();
    }
}
