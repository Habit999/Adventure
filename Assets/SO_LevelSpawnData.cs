using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject to hold level spawn data
/// </summary>

[CreateAssetMenu(fileName = "New LevelSpawnData", menuName = "LevelSpawnData", order = 1)]
public class SO_LevelSpawnData : ScriptableObject
{
	[Header("Loot")]
	public int MaxChestsInLevel;
	public int MaxExperienceInLevel;
	
	[Header("Enemies")]
	public int MaxMimicsInLevel;
	public int MaxVoidsInLevel;
	public int MaxThievesInLevel;
}
