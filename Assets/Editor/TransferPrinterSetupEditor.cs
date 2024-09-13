using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(TransferPrinterSetup))]
public class TransferPrinterSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TransferPrinterSetup script = (TransferPrinterSetup)target;

        if (GUILayout.Button("Sync Printers"))
        {
            script.SyncPrinters();
        }
    }
}
