using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceSpawnPoint : MonoBehaviour
{
	/*[HideInInspector] public bool _willSpawn;
	
	[SerializeField] float minRandomAmountRange;
	[SerializeField] float maxRandomAmountRange;
	
	GameObject body;
	SphereCollider sphereCollider;
	
	void Awake()
	{
		body = transform.GetChild(0).gameObject;
		sphereCollider = GetComponent<SphereCollider>();
		
		body.SetActive(false);
		sphereCollider.enabled = false;
	}
	
	void OnEnable()
	{
		SpawnManager.SpawnLevelLoot += SpawnXP;
	}
	
	void OnDisable()
	{
		SpawnManager.SpawnLevelLoot -= SpawnXP;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<SkillsManager>().AddExperience(Random.Range(minRandomAmountRange, maxRandomAmountRange));
			gameObject.SetActive(false);
		}
	}
	
	void SpawnXP()
	{
		if(!_willSpawn) return;
		body.SetActive(true);
		sphereCollider.enabled = true;
	}*/
}
