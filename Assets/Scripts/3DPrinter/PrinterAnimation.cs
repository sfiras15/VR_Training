using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class PrinterAnimation : MonoBehaviour
{
    // GameObjects that will move
    // zAxis will move along the Y axis
    [SerializeField] private GameObject zAxis;
    // nozzle will move along the X axis
    [SerializeField] private GameObject nozzle;
    // bed will move along the Z axis
    [SerializeField] private GameObject bed;

    [Range(0, 0.75f)][SerializeField] private float moveSpeed = 0.2f;

    [SerializeField] private Printer_SO  printerSo;

 

    private void OnEnable()
    {
        if (printerSo != null) printerSo.onMove += MoveGameObject;
    }

    private void OnDisable()
    {
        if (printerSo != null) printerSo.onMove -= MoveGameObject;
    }
    private void MoveGameObject(Axis axis, float valueInMM)
    {
        if (printerSo != null)
        {
            GameObject obj = GetTargetObject(axis);
            float valueInUnity = valueInMM / 1000.0f; // Convert millimeters to Unity units (meters)
            Vector3 direction = GetDirection(axis) * moveSpeed * valueInUnity;
            StartCoroutine(MoveObjectCoroutine(obj, direction, printerSo.moveDuration));
        }
      
    }


    // Returns the gameObject that will be moved along that axe
    private GameObject GetTargetObject(Axis axis)
    {
        switch (axis)
        {
            case Axis.X:
                return nozzle;
            case Axis.Y:
                return zAxis;
            case Axis.Z:
                return bed;
            default:
                return null;
        }
    }
    private Vector3 GetDirection(Axis axis)
    {
        switch (axis)
        {
            case Axis.X:
                return Vector3.right;
            case Axis.Y:
                return Vector3.up;
            case Axis.Z:
                return Vector3.forward;
            default:
                return Vector3.zero;
        }
    }

    private IEnumerator MoveObjectCoroutine(GameObject obj, Vector3 direction, float duration)
    {
        Vector3 startPosition = obj.transform.localPosition;
        Vector3 targetPosition = startPosition + direction;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.localPosition = targetPosition; // Ensure the object reaches the target position
    }


}
