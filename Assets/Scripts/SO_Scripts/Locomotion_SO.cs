using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;


[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/Locomotion", order = 2)]
public class Locomotion_SO : ScriptableObject
{
    private bool held;
    private XRBaseController controller;

    public event Action<XRBaseController, bool> onGrabbing;

    public XRBaseController Controller
    {
        get { return controller; }
        set
        {
            controller = value;
        }
    }
    public bool Held
    {
        get { return held; }
        set
        {
            held = value;
            onGrabbing?.Invoke(controller,held);

        }
    }
}

