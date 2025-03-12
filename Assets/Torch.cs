using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
