using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GridData", menuName = "CustomGrid/GridData")]
public class SO_GridData : ScriptableObject
{
    public int LengthX;
    public int LengthZ;

    public float Spacing;

    public float OffsetX;
    public float OffsetZ;

    [SerializeField]
    public List<bool> IsCellActive;
    [SerializeField]
    public List<GameObject> OccupantPrefabs;
    [SerializeField]
    public List<Vector3> OccupantPositions;
    [SerializeField]
    public List<Vector3> OccupantEulerAngles;
    [SerializeField]
    public List<bool> OccupantActiveOnSpawn;
}
