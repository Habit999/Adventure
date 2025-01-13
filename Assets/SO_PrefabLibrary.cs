using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "PrefabLibrary", menuName = "CustomGrid/PrefabLibrary")]
public class SO_PrefabLibrary : ScriptableObject
{
	public List<GameObject> _gridPresets = new List<GameObject>();
	
	[Space(10)]
	
	public List<GameObject> _gameItems = new List<GameObject>();
}
