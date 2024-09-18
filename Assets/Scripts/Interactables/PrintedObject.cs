using UnityEngine;

public class PrintedObject : MonoBehaviour
{
    private Interactables interactable;
    // Material with the printing shader
    [SerializeField] private Material material;
    private Rigidbody rb;
    private void Awake()
    {
        interactable = GetComponent<Interactables>();
        rb = GetComponent<Rigidbody>();
        interactable.enabled = false;
    }

    private void OnEnable()
    {
        if (GameEventsManager.instance != null)
        {
            if (interactable != null) GameEventsManager.instance.mainLevelQuests.onPrinterCooledDown += EnableInteractable;
        }
    }

    private void OnDisable()
    {
        if (GameEventsManager.instance != null)
        {
            if (interactable != null) GameEventsManager.instance.mainLevelQuests.onPrinterCooledDown -= EnableInteractable;
        }
    }
    private void EnableInteractable()
    {
        
        if (interactable != null) interactable.enabled = true;
        if (material != null) material.SetFloat("_ConstructY", 50f);
        if (rb != null) rb.isKinematic = false;

    }
}
