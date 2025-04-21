using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Prefab library for saving and loading assets
/// </summary>

public static class PrefabLibrary
{
	public static IDictionary<int, GameObject> ItemPrefabID = new Dictionary<int, GameObject>()
	{
		{ 1, Resources.Load<GameObject>("Item_Bandage") },
		{ 2, Resources.Load<GameObject>("Item_HealthPotion") },
		{ 3, Resources.Load<GameObject>("Item_Scythe") }
	};
}
