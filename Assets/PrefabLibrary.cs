using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class PrefabLibrary
{
	public static IDictionary<int, GameObject> GridPrefabID = new Dictionary<int, GameObject>()
	{
		{ 1, Resources.Load<GameObject>("StartPoint") },
		{ 2, Resources.Load<GameObject>("MovePoint") }
	};
	
	public static IDictionary<int, GameObject> ItemPrefabID = new Dictionary<int, GameObject>()
	{
		{ 1, Resources.Load<GameObject>("Item_Bandage") },
		{ 2, Resources.Load<GameObject>("Item_HealthPotion") },
		{ 3, Resources.Load<GameObject>("Item_ManaPotion") },
		{ 4, Resources.Load<GameObject>("Item_Scythe") }
	};
}
