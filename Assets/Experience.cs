using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Experience collection script
/// </summary>

public class Experience : MonoBehaviour
{
    [SerializeField] private float minRandomAmountRange;
    [SerializeField] private float maxRandomAmountRange;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<SkillsManager>().AddExperience(Random.Range(minRandomAmountRange, maxRandomAmountRange));
            Destroy(gameObject);
        }
    }
}
