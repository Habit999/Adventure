using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridData", menuName = "CustomGrid/GridData")]
public class SO_GridData : ScriptableObject
{
    public int LengthX;
    public int LengthZ;

    public float Spacing;

    public float OffsetX;
    public float OffsetZ;

    public bool[,] IsCellActive;
    public GameObject[,] OccupantPrefabs;
    public Vector3[,] OccupantPositions;
    public Vector3[,] OccupantEulerAngles;
}
