using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_Item : MonoBehaviour
{
	public SE_ItemData _itemData = new SE_ItemData();
	
	public float GetRandomExperience { get { return Random.Range(5, 10); } }
	
	[System.Serializable]
	public struct SpecificData
	{
		public int levelRequired;
		public float damage;
		public float healthRecovery;
		public float manaRecovery;
	}
	
	[Space(5)]
	public SkillsManager.SkillsList minimumStatRequirements = new SkillsManager.SkillsList(1, 1, 1);
	
	public abstract bool CollectItem();
}
