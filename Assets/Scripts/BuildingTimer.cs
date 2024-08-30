using System.Collections;
using System.Timers;
using UnityEngine;

public class BuildingTimer : MonoBehaviour
{
    // Material with the printing shader
    [SerializeField] private Material material;

    // from testing these are the values that yield the best result
    [SerializeField] private float minY = 0.83f;
    [SerializeField] private float maxY = 1.03f;

    // duration of the print
    [SerializeField] private float duration = 8f;

    private void OnEnable()
    {
        if (GameEventsManager.instance != null) GameEventsManager.instance.onPrintStarted += StartPrint;
    }
    private void OnDisable()
    {
        if (GameEventsManager.instance != null) GameEventsManager.instance.onPrintStarted -= StartPrint;
    }
    private void Start()
    {
        material.SetFloat("_ConstructY", minY);
    }

    private void StartPrint()
    {
        StartCoroutine(PrintingSequence());
    }
    private IEnumerator PrintingSequence()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float y = Mathf.Lerp(minY, maxY, elapsed / duration);
            material.SetFloat("_ConstructY", y);
            elapsed += Time.deltaTime;
            yield return null;
        }
        material.SetFloat("_ConstructY", maxY);
    }
}
