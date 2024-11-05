using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelDetails", menuName = "LevelDetails", order = 0)]
public class SO_LevelDetails : ScriptableObject
{
	public int LevelBuildIndex;
	public string LevelName;
	public string LevelBrief;
}
