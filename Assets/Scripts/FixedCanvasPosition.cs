using UnityEngine;

public class SmoothFixedCanvasPosition : MonoBehaviour
{
    [Tooltip("Offset from the camera in world space coordinates.")]
    [SerializeField] private Vector3 positionOffset = new Vector3(0, 0f, 1f);
    private Transform cameraTransform;

    [Tooltip("How quickly the canvas follows the camera. Higher values = quicker.")]
    public float followSpeed = 10f;

    private void Start()
    {
        if (cameraTransform == null)
        {
            // Find the camera tagged as "MainCamera" if not set in the inspector
            cameraTransform = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform != null)
        {
            // Smoothly interpolate the position of the canvas
            Vector3 targetPosition = cameraTransform.position + cameraTransform.rotation * positionOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            // Ensure the canvas faces the camera directly
            transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
        }
    }
}
