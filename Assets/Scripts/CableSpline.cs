using UnityEngine;
using UnityEngine.Splines;


/// <summary>
/// This script will make the last knot (index 2) to always follow the target in our case it's the nozzleHole
/// </summary>
public class CableSpline : MonoBehaviour
{
    [SerializeField] private Transform followTarget;  // Reference to the nozzle hole gameObject that will be followed
    private SplineContainer splineContainer;

    [Range(0f, 1f)]
    [SerializeField] private float curveValue = 0.1f;

    private void Awake()
    {
        // Get the SplineContainer component attached to the GameObject
        splineContainer = GetComponent<SplineContainer>();
    }

    void LateUpdate()
    {
        NozzleFollower();
    }
    private void NozzleFollower()
    {
        if (splineContainer != null && followTarget != null)
        {
            // Access the first spline
            Spline spline = splineContainer.Spline;

            // Check if the spline has at least 3 knots
            if (spline.Count > 2)
            {
                // Get the third knot
                var thirdKnot = spline[2];

                // Move the third knot to match the position of the followTarget
                thirdKnot.Position = splineContainer.transform.InverseTransformPoint(followTarget.position);

                // Adjust tangents to control the curve (straighten or slightly curve depending on your need)
                Vector3 tangentDirection = Vector3.up * curveValue;  // Small upward tangent adjustment, modify this to your desired curve
                thirdKnot.TangentIn = -tangentDirection; // Control the in-tangent (curve coming towards the knot)
                thirdKnot.TangentOut = tangentDirection; // Control the out-tangent (curve leaving the knot)
                // Update the spline with the modified third knot
                spline.SetKnot(2, thirdKnot);
            }
        }
    }
}
