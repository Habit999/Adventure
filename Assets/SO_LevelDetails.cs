using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelDetails", menuName = "LevelDetails", order = 0)]
public class SO_LevelDetails : ScriptableObject
{
	public int LevelNumber;
	public int LevelBuildIndex;
	
	[Space(5)]
	
	public string LevelName;
	public string LevelBrief;
}
