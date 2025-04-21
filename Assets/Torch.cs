using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls intensity of torch lighting
/// </summary>

public class Torch : MonoBehaviour
{
    [SerializeField] private Light lightSource;

    private float startLightIntensity;

    private void Start()
    {
        startLightIntensity = lightSource.intensity;
    }

    public void AdjustLightIntensity(float dimPercentage)
    {
        lightSource.intensity = startLightIntensity * dimPercentage;
    }
}
