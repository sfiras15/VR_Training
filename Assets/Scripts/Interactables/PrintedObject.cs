using UnityEngine;

public class PrintedObject : MonoBehaviour
{
    private Interactables interactable;
    // Material with the printing shader
    [SerializeField] private Material material;
    private void Awake()
    {
        interactable = GetComponent<Interactables>();
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
        if (material != null) material.SetFloat("_ConstructY", 15f);

    }
}
