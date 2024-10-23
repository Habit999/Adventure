using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelDetails", menuName = "LevelDetails", order = 0)]
public class SO_LevelDetails : ScriptableObject
{
	public int levelBuildIndex;
	public string levelName;
	public string levelBrief;
}
