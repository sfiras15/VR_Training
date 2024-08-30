using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will activate the objectVisualizer which will make it visible at all times on the bed
public class PurgingLine : MonoBehaviour
{
    // the one on the nozzle
    [SerializeField] private GameObject mainObjectVisualizer;
    // the one on the purgingLine
    [SerializeField] private GameObject childObjectVisualizer;


    private void OnEnable()
    {
        if (GameEventsManager.instance != null) GameEventsManager.instance.onPurgeCompleted += ActivateVisualizer;
    }
    private void OnDisable()
    {
        if (GameEventsManager.instance != null) GameEventsManager.instance.onPurgeCompleted -= ActivateVisualizer;
    }
    private void Start()
    {
        if (childObjectVisualizer != null) childObjectVisualizer.SetActive(false);
        if (mainObjectVisualizer != null) mainObjectVisualizer.SetActive(true);
    }
    private void ActivateVisualizer()
    {
        if (childObjectVisualizer != null) childObjectVisualizer.SetActive(true);
        if (mainObjectVisualizer != null) mainObjectVisualizer.SetActive(false);
    }
}
