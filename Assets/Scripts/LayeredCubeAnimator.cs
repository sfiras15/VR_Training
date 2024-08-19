using System.Collections;
using UnityEngine;

public class LayeredCubeAnimator : MonoBehaviour
{
    public float layerVisibilityDuration = 2.0f; // Time for each layer to appear
    public GameObject[] layers; // Array to hold references to each layer

    private void Start()
    {
        StartCoroutine(AnimateLayers());
    }

    private IEnumerator AnimateLayers()
    {
        // Loop through layers and enable them one by one
        for (int i = 0; i < layers.Length; i++)
        {
            // Set the stencil reference value to correspond to the current layer
            SetStencilRef(i + 1);

            // Show current layer
            //layers[i].SetActive(true);

            // Wait for the specified duration before showing the next layer
            yield return new WaitForSeconds(layerVisibilityDuration);
        }
    }

    private void SetStencilRef(int refValue)
    {
        // Set the stencil reference value in the shader
        foreach (var layer in layers)
        {
            Debug.Log(refValue);
            var material = layer.GetComponent<Renderer>().material;
            material.SetInteger("_StencilRef", refValue);
        }
    }
}
