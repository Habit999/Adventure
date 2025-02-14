using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "PrefabLibrary", menuName = "CustomGrid/PrefabLibrary")]
public class SO_PrefabLibrary : ScriptableObject
{
	public List<GameObject> GridPresets = new List<GameObject>();
	
	[Space(10)]
	
	public List<GameObject> GameItems = new List<GameObject>();
}
