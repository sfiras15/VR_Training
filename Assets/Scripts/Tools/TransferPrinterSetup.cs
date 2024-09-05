using UnityEngine;
using System.Collections.Generic;

public class TransferPrinterSetup: MonoBehaviour
{
    public GameObject sourcePrinter;
    public GameObject targetPrinter;

    public void SyncPrinters()
    {
        if (sourcePrinter == null || targetPrinter == null)
        {
            Debug.LogError("Source or Target Printer is not assigned.");
            return;
        }

        // Collect component names from both printers
        HashSet<string> sourceNames = CollectComponentNames(sourcePrinter.transform);
        HashSet<string> targetNames = CollectComponentNames(targetPrinter.transform);

        // Find components in the target printer that are not in the source printer
        HashSet<string> toRemove = new HashSet<string>(targetNames);
        toRemove.ExceptWith(sourceNames);

        // Remove components from the target printer
        RemoveComponents(targetPrinter.transform, toRemove);

        Debug.Log($"Removed {toRemove.Count} facets from the target printer.");
    }

    HashSet<string> CollectComponentNames(Transform parent)
    {
        HashSet<string> names = new HashSet<string>();

        foreach (Transform child in parent)
        {
            string name = child.gameObject.name;
            if (name.StartsWith("Shape_IndexedFaceSet"))
            {
                names.Add(name);
            }

            // Recursively check children
            names.UnionWith(CollectComponentNames(child));
        }

        return names;
    }

    void RemoveComponents(Transform parent, HashSet<string> namesToRemove)
    {
        foreach (Transform child in parent)
        {
            string name = child.gameObject.name;
            if (namesToRemove.Contains(name))
            {
                DestroyImmediate(child.gameObject); // Use DestroyImmediate for editor mode
                continue;
            }

            // Recursively check children
            RemoveComponents(child, namesToRemove);
        }
    }
}
