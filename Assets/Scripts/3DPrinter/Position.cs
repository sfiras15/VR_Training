using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Position : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI xCoordinates;
    [SerializeField] private TextMeshProUGUI yCoordinates;
    [SerializeField] private TextMeshProUGUI zCoordinates;
    public virtual void UpdateText(Vector3 value)
    {
        xCoordinates.text = $"X: {value.x:F2}";
        yCoordinates.text = $"Y: {value.y:F2}";
        zCoordinates.text = $"Z: {value.z:F2}";
    }
}
