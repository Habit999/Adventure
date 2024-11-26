using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SE_ItemData
{
	public enum TYPE {Weapon, Healing, Experience}
	public TYPE Type;
	
	public string Name;
	
	public Sprite Image;
	
	public int MaxItemStack;
	
	public string AnimatorTriggerName;
}
