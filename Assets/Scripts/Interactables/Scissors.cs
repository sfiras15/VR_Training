using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Scissors : Interactables
{
    [SerializeField] private Collider[] scannedObjects;
    [SerializeField] private float sphereRadius = 2f; // Adjustable radius for the sphere
    [SerializeField] private LayerMask cutLayerMask;
    // Reference to the half of the scissors that will rotate
    [SerializeField] private GameObject scissorsHalf;
    [SerializeField] private float cutDuration = 0.5f;
    private bool isCutting = false;

    // Visualize the sphere in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Set the gizmo color
        Gizmos.DrawWireSphere(transform.position, sphereRadius); // Draw the sphere at the object's position
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        Cut();
    }

    private void Cut()
    {
        if (!isCutting)
        {
            if (scissorsHalf != null) StartCoroutine(CutAnimation());
            scannedObjects = Physics.OverlapSphere(transform.position, sphereRadius, cutLayerMask);
            if (scannedObjects.Length > 0)
            {
                foreach (Collider scannedObject in scannedObjects)
                {
                    if (scannedObject.gameObject.TryGetComponent(out ICuttable cuttable)) cuttable.Cut();
                }
            }
        }
    }

    private IEnumerator CutAnimation()
    {
        isCutting = true;
        float elapsed = 0;

        // Save the initial rotation (only concerned with Y-axis)
        float initialYRotation = -15f;
        float targetYRotation = 0f;

        // First part: Rotate Y from -15 to 0
        while (elapsed < cutDuration)
        {
            float t = elapsed / cutDuration;
            float newYRotation = Mathf.Lerp(initialYRotation, targetYRotation, t);
            scissorsHalf.transform.localRotation = Quaternion.Euler(0, newYRotation, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Wait before the second part
        yield return new WaitForSeconds(0.1f); // Optional pause

        // Second part: Rotate Y from 0 back to -15
        elapsed = 0;
        while (elapsed < cutDuration)
        {
            float t = elapsed / cutDuration;
            float newYRotation = Mathf.Lerp(targetYRotation, initialYRotation, t);
            scissorsHalf.transform.localRotation = Quaternion.Euler(0, newYRotation, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isCutting = false;
    }
}
