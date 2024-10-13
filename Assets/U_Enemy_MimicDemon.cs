using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Enemy_MimicDemon : A_Enemy
{
	public ENEMYSTATE EnemyState = ENEMYSTATE.Idle;
	
	public EnemyData _currentEnemyData = new EnemyData(100, 30, 0, 20);
	
	public struct MimicData
	{
		public MimicData(float inp_timeTillBlink)
		{
			timeTillBlink = inp_timeTillBlink;
		}
		
		public float timeTillBlink;
	}
	public MimicData _currentMimicData = new MimicData(40);
	
	public int MaxEnemyTypeInScene { get { return maxEnemyTypeInScene; } }
	[SerializeField] int maxEnemyTypeInScene = 3;
	
	public override void EnemyBehaviour()
	{
		switch(EnemyState)
		{
			case ENEMYSTATE.Idle:
				IdleState();
				break;
				
			default:
				EnemyState = ENEMYSTATE.Idle;
				break;
		}
	}
	
	public override void DamagePlayer()
	{
		
	}
	
	public override void DamageEnemy()
	{
		
	}
	
	public override bool TriggerAbility()
	{
		return false;
	}
	
	#region Behaviour States
	
	void IdleState()
	{
		
	}
	
	#endregion
	
	
}
