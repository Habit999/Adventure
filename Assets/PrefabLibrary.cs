using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class PrefabLibrary
{
	public static IDictionary<int, GameObject> PrefabID = new Dictionary<int, GameObject>()
	{
		{ 1, Resources.Load<GameObject>("StartPoint") },
		{ 2, Resources.Load<GameObject>("MovePoint") }
	};
}
