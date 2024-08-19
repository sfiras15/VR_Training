using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/Printer", order = 2)]
public class Printer_SO : ScriptableObject
{
    // the time that an object takes to move
    // duration based on the value of movement
    public float moveDuration = 2f;

    // Event for moving the different components of the printer
    public event Action<Axis, float> onMove;

    public void Move(Axis axis,float value)
    {
        onMove?.Invoke(axis, value);
    }
}
