using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
	public enum Type { Weapon, Potion, Quest, Key };
	public Type ItemType;
	
	public float _usage = 100;
}
