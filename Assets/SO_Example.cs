using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Example", menuName = "CustomAssets/Example", order = 0)]
public class SO_Example : ScriptableObject
{
	public new string name;
	public Sprite image;
	[Space(3)]
	public string description;
	[Space(3)]
	public int amount;
}
