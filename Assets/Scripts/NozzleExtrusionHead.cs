using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script responsible for enabling the extrusion effect when the materiel extrusion event gets triggered
/// </summary>
public class NozzleExtrusionHead : MonoBehaviour,ICuttable
{
    private ExtrusionEffect extrusionEffectSpline;
    private Collider nozzleCollider;

    private void Awake()
    {
        extrusionEffectSpline = GetComponentInChildren<ExtrusionEffect>();
        nozzleCollider = GetComponent<Collider>();
        nozzleCollider.enabled = false;
        extrusionEffectSpline.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (GameEventsManager.instance != null)
        {
            if (extrusionEffectSpline != null)
            {
                GameEventsManager.instance.onExtrudingMaterial += EnableExtrusionEffect;
                GameEventsManager.instance.onPurgeCompleted += DisableExtrusionEffect;
            }
        }
    }
    private void OnDisable()
    {
        if (GameEventsManager.instance != null)
        {
            if (extrusionEffectSpline != null)
            {
                GameEventsManager.instance.onExtrudingMaterial -= EnableExtrusionEffect;
                GameEventsManager.instance.onPurgeCompleted -= DisableExtrusionEffect;
            }
        }
    }
    private void EnableExtrusionEffect(float mm)
    {
        extrusionEffectSpline.gameObject.SetActive(true);
        nozzleCollider.enabled = true;
        extrusionEffectSpline.StartExtrusion(mm);
    }
    private void DisableExtrusionEffect()
    {
        extrusionEffectSpline.ResetSpline();
        nozzleCollider.enabled = true;
        extrusionEffectSpline.gameObject.SetActive(false);
        
    }

    public void Cut()
    {
        DisableExtrusionEffect();
    }
}
