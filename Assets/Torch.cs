using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private Light lightSource;

    private float startLightIntensity;

    public void AdjustLightIntensity(float dimPercentage)
    {
        lightSource.intensity = startLightIntensity * dimPercentage;
    }
}
