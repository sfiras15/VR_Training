using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// This script handles the extrusion and curling effect of a filament with smooth elongation.
/// </summary>
public class ExtrusionEffect : MonoBehaviour
{
    [SerializeField] private Transform nozzle;  // Reference to the nozzle position
    private SplineContainer splineContainer;

    [Range(0f, 1f)]
    [SerializeField] private float extrusionSpeed = 0.1f;
    [SerializeField] private float curlHeight = 0.2f;
    [SerializeField] private float curlRadius = 0.2f;
    [SerializeField] private float extrusionLength = 0.5f;
    private float extrusionProgress = 0f;

    // 10mm extruded = 2 seconds
    private float mmToSeconds = 0.2f;
    private bool isExtruding = false;

    private SplineExtrude splineExtrude;

    private Vector3 secondKnotTargetPos;
    private Vector3 thirdKnotTargetPos;
    private Vector3 fourthKnotTargetPos;

    private void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();
        splineExtrude = GetComponent<SplineExtrude>();
        ResetSpline();
        extrusionProgress = 0f;
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.Space)) StartExtrusion(10);
        //if (Input.GetKey(KeyCode.Escape)) StopExtrusion();
    }
    void LateUpdate()
    {
        if (isExtruding)
        {
            ExtrudeAndCurl();
        }
    }

    public void StartExtrusion(float mm)
    {
        var duration = mmToSeconds * mm;
        isExtruding = true;
        StartCoroutine(ResetExtrusionVariable(duration));   
        //
    }
    private IEnumerator ResetExtrusionVariable(float duration)
    {
        yield return new WaitForSeconds(duration);
        isExtruding = false;
    }
    public void StopExtrusion()
    {
        isExtruding = false;
        ResetSpline();
    }

    private void ExtrudeAndCurl()
    {
        if (extrusionProgress >= 0.9f) return;
        extrusionProgress += Time.deltaTime * extrusionSpeed;
        Spline spline = splineContainer.Spline;

        if (spline.Count > 3)
        {
            // First knot stays at the nozzle
            var firstKnot = spline[0];
            firstKnot.Position = splineContainer.transform.InverseTransformPoint(nozzle.position);
            spline.SetKnot(0, firstKnot);

            // Set target positions for second, third, and fourth knots
            secondKnotTargetPos = splineContainer.transform.InverseTransformPoint(nozzle.position - Vector3.up * extrusionLength / 2f);
            thirdKnotTargetPos = splineContainer.transform.InverseTransformPoint(nozzle.position - Vector3.up * extrusionLength) + new Vector3(0, Mathf.Sin(extrusionProgress) * curlHeight, Mathf.Cos(extrusionProgress) * curlRadius);
            fourthKnotTargetPos = splineContainer.transform.InverseTransformPoint(nozzle.position - Vector3.up * extrusionLength * 1.5f) + new Vector3(0, Mathf.Sin(extrusionProgress + Mathf.PI / 2) * curlHeight, Mathf.Cos(extrusionProgress + Mathf.PI / 2) * curlRadius);

            // Lerp positions of each knot to smoothly move from the nozzle
            var secondKnot = spline[1];
            secondKnot.Position = Vector3.Lerp(splineContainer.transform.InverseTransformPoint(nozzle.position), secondKnotTargetPos, extrusionProgress);
            spline.SetKnot(1, secondKnot);

            var thirdKnot = spline[2];
            thirdKnot.Position = Vector3.Lerp(splineContainer.transform.InverseTransformPoint(nozzle.position), thirdKnotTargetPos, extrusionProgress);
            spline.SetKnot(2, thirdKnot);

            var fourthKnot = spline[3];
            fourthKnot.Position = Vector3.Lerp(splineContainer.transform.InverseTransformPoint(nozzle.position), fourthKnotTargetPos, extrusionProgress);
            spline.SetKnot(3, fourthKnot);
        }

        if (splineExtrude != null)
        {
            splineExtrude.Rebuild();
        }
    }

    // Method used if we cut the extruded materials
    public void ResetSpline()
    {
        Spline spline = splineContainer.Spline;

        // Reset all knots to the nozzle's position
        for (int i = 0; i < spline.Count; i++)
        {
            var knot = spline[i];
            knot.Position = splineContainer.transform.InverseTransformPoint(nozzle.position);
            spline.SetKnot(i, knot);
        }
        extrusionProgress = 0f;
        if (splineExtrude != null)
        {
            splineExtrude.Rebuild();
        }
    }
}