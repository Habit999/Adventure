using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ItemPrefabs", menuName = "CustomGrid/ItemPrefabs")]
public class SO_ItemPrefabs : ScriptableObject
{
	public List<GameObject> GameItems = new List<GameObject>();
}
