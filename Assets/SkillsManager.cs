using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
	public PlayerController Controller { get { return gameObject.GetComponent<PlayerController>(); } }
	
	public float _experienceGained = 0;
	public int _playerLevel = 0;
	public int _skillPoints = 0;
	
	private static float firstLevelExperience = 10;
	private static float levelIntervalMultiplier = 0.2f;
	private static float nextLevelExperience = 10;
	
	public struct SkillsList
	{
		public int vitality;
		public int strength;
		public int intelligence;
	}
	public SkillsList _currentSkills = new SkillsList();
	
	bool initialSkillsCheckComplete = false;
	
	void Start()
	{
		_playerLevel = 0;
		_experienceGained = 0;
		StartCoroutine(UpdatePlayerLevel());
	}
	
	public void AddExperience(float amount)
	{
		_experienceGained += amount;
	}
	
	IEnumerator UpdatePlayerLevel()
	{
		nextLevelExperience = firstLevelExperience + ((firstLevelExperience * levelIntervalMultiplier) * _playerLevel);
		
		if(_experienceGained / nextLevelExperience > 1)
		{
			print(nextLevelExperience);
			_playerLevel += 1;
			if(initialSkillsCheckComplete) _skillPoints += 1;
		}
		else initialSkillsCheckComplete = true;
		
		yield return new WaitForSeconds(0.01f);
		StartCoroutine(UpdatePlayerLevel());
	}
}
