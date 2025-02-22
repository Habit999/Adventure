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

    public List<bool> IsCellActive;
    public List<GameObject> OccupantPrefabs;
    public List<Vector3> OccupantPositions;
    public List<Vector3> OccupantEulerAngles;
}
