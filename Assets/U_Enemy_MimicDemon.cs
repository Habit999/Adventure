using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Enemy_MimicDemon : A_Enemy
{
	public static int MaxEnemyTypeInScene = 3;
	
	public ENEMYSTATE EnemyState = ENEMYSTATE.Idle;
	
	public EnemyData _currentEnemyData = new EnemyData(100, 30, 0, 0);
	
	public struct MimicData
	{
		public MimicData(float inp_timeTillBlink)
		{
			timeTillBlink = inp_timeTillBlink;
		}
		
		public float timeTillBlink;
	}
	public MimicData _currentMimicData = new MimicData(20);
	
	void Update()
	{
		EnemyBehaviour();
	}
	
	protected override void EnemyBehaviour()
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
	
	public override void DamageEnemy(float incomingDamage)
	{
		_currentEnemyData.health -= incomingDamage;
		
		if(_currentEnemyData.health <= 0) Destroy(this.gameObject);
	}
	
	#region Behaviour States
	
	void IdleState()
	{
		
	}
	
	#endregion
	
	
}
