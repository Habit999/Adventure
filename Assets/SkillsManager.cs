using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
	public PlayerController Controller { get { return gameObject.GetComponent<PlayerController>(); } }
	
	public delegate void PlayerLevelUp();
	public event PlayerLevelUp LevelUp;

    public event Action<float, float> OnExperienceChange;

    public int PlayerLevel = 0;
	public int SkillPoints = 0;
	[HideInInspector] public int TempSkillPoints;
	
	[HideInInspector] public float ExperienceGained;
	[HideInInspector] public float NextLevelExperience;
	
	private static float firstLevelExperience = 10;
	private static float levelIntervalMultiplier = 0.2f;
	
	// Skill variables
	public enum SKILLTYPE { Vitality, Strength };
	[System.Serializable]
	public struct SkillsList
	{
		public SkillsList(int inp_vitality, int inp_strength)
		{
			vitality = inp_vitality;
			strength = inp_strength;
		}
		
		public int vitality;
		public int strength;
	}
	public SkillsList CurrentSkills = new SkillsList(1, 1);
	
	// Temp variables for changing skills before confirmation
	[HideInInspector] public SkillsList TempSkills = new SkillsList(1, 1);
	[HideInInspector] public bool TempValuesActive;
	
	bool initialSkillsCheckComplete = false;
	
	void Start()
	{
		StartCoroutine(UpdatePlayerLevel());
	}
	
	public void AddExperience(float amount)
	{
		ExperienceGained += amount;
		OnExperienceChange(ExperienceGained, NextLevelExperience);
    }
	
	public void ChangeSkillValue(SKILLTYPE type, int amount)
	{
		if(!TempValuesActive)
		{
			TempValuesActive = true;
			
			TempSkillPoints = SkillPoints;
			
			TempSkills.vitality = CurrentSkills.vitality;
			TempSkills.strength = CurrentSkills.strength;
		}
		
		switch(type)
		{
			case SKILLTYPE.Vitality:
				TempSkills.vitality += amount;
				break;
				
			case SKILLTYPE.Strength:
				TempSkills.strength += amount;
				break;
				
			default:
				break;
		}
		
		TempSkillPoints += -amount;
	}
	
	public void ConfirmSkillChanges()
	{
		TempValuesActive = false;
		
		SkillPoints = TempSkillPoints;
		
		CurrentSkills.vitality = TempSkills.vitality;
		CurrentSkills.strength = TempSkills.strength;
	}
	
	private IEnumerator UpdatePlayerLevel()
	{
		// Experience required for next level
		NextLevelExperience = firstLevelExperience + ((firstLevelExperience * levelIntervalMultiplier) * PlayerLevel);
		
		// Checks if player has enough xp for next level
		if(ExperienceGained / NextLevelExperience > 1)
		{
			PlayerLevel += 1;
			if(initialSkillsCheckComplete) SkillPoints += 1;
			
			ExperienceGained -= NextLevelExperience;
			
			// Call level up event
			LevelUp();
		}
		else initialSkillsCheckComplete = true;
		
		yield return new WaitForSeconds(0.01f);
		StartCoroutine(UpdatePlayerLevel());
	}
}
