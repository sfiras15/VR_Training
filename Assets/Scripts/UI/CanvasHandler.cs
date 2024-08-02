using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    private bool activated = false;
    public void Activate()
    {
        if (!activated)
        {
            activated = true;
            gameObject.SetActive(true);
        }
    }
}
