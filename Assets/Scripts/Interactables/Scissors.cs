using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Scissors : Interactables
{
    [SerializeField] private Collider[] scannedObjects;
    [SerializeField] private float sphereRadius = 2f; // Adjustable radius for the sphere
    [SerializeField] private LayerMask cutLayerMask;
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
        scannedObjects = Physics.OverlapSphere(transform.position, sphereRadius,cutLayerMask);
        if (scannedObjects.Length > 0)
        {
            foreach (Collider scannedObject in scannedObjects)
            {
                if (scannedObject.gameObject.TryGetComponent(out ICuttable cuttable))
                {
                    cuttable.Cut();
                }
            }
        }
    }
}
