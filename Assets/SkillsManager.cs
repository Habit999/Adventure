using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
	public PlayerController Controller { get { return gameObject.GetComponent<PlayerController>(); } }
	
	public int _playerLevel = 0;
	public int _skillPoints = 0;
	[HideInInspector] public int _tempSkillPoints;
	
	[HideInInspector] public float _experienceGained;
	[HideInInspector] public float _nextLevelExperience;
	
	private static float firstLevelExperience = 10;
	private static float levelIntervalMultiplier = 0.2f;
	
	// Skill variables
	public enum SKILLTYPE { Vitality, Strength, Intelligence };
	[System.Serializable]
	public struct SkillsList
	{
		public SkillsList(int inp_vitality, int inp_strength, int inp_intelligence)
		{
			vitality = inp_vitality;
			strength = inp_strength;
			intelligence = inp_intelligence;
		}
		
		public int vitality;
		public int strength;
		public int intelligence;
	}
	public SkillsList _currentSkills = new SkillsList(1, 1, 1);
	
	// Temp variables for changing skills before confirmation
	[HideInInspector] public SkillsList _tempSkills = new SkillsList(1, 1, 1);
	[HideInInspector] public bool _tempValuesActive;
	
	bool initialSkillsCheckComplete = false;
	
	void Start()
	{
		StartCoroutine(UpdatePlayerLevel());
	}
	
	public void AddExperience(float amount)
	{
		_experienceGained += amount;
	}
	
	public void ChangeSkillValue(SKILLTYPE type, int amount)
	{
		if(!_tempValuesActive)
		{
			_tempValuesActive = true;
			
			_tempSkillPoints = _skillPoints;
			
			_tempSkills.vitality = _currentSkills.vitality;
			_tempSkills.strength = _currentSkills.strength;
			_tempSkills.intelligence = _currentSkills.intelligence;
		}
		
		switch(type)
		{
			case SKILLTYPE.Vitality:
				_tempSkills.vitality += amount;
				break;
				
			case SKILLTYPE.Strength:
				_tempSkills.strength += amount;
				break;
				
			case SKILLTYPE.Intelligence:
				_tempSkills.intelligence += amount;
				break;
				
			default:
				break;
		}
		
		_tempSkillPoints += -amount;
	}
	
	public void ConfirmSkillChanges()
	{
		_tempValuesActive = false;
		
		_skillPoints = _tempSkillPoints;
		
		_currentSkills.vitality = _tempSkills.vitality;
		_currentSkills.strength = _tempSkills.strength;
		_currentSkills.intelligence = _tempSkills.intelligence;
	}
	
	IEnumerator UpdatePlayerLevel()
	{
		_nextLevelExperience = firstLevelExperience + ((firstLevelExperience * levelIntervalMultiplier) * _playerLevel);
		
		if(_experienceGained / _nextLevelExperience > 1)
		{
			_playerLevel += 1;
			if(initialSkillsCheckComplete) _skillPoints += 1;
			
			_experienceGained -= _nextLevelExperience;
		}
		else initialSkillsCheckComplete = true;
		
		yield return new WaitForSeconds(0.01f);
		StartCoroutine(UpdatePlayerLevel());
	}
}
