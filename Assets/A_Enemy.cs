using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_Enemy : MonoBehaviour
{
	public struct EnemyData
	{
		public EnemyData(float inp_health, float inp_damage, float inp_speed, float inp_damageRange)
		{
			health = inp_health;
			damage = inp_damage;
			speed = inp_speed;
			damageRange = inp_damageRange;
		}
		
		public float health;
		public float damage;
		public float speed;
		public float damageRange;
	}
	
	public enum ENEMYSTATE { Idle, Walking, Attacking };
	
	public abstract void EnemyBehaviour();
	
	public abstract void DamagePlayer();
	
	public abstract void DamageEnemy();
	
	public abstract bool TriggerAbility();
}
