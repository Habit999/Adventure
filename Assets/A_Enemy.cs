using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_Enemy : MonoBehaviour
{
	public struct EnemyData
	{
		public EnemyData(float inp_health, float inp_damage, float inp_walkSpeed, float inp_sprintSpeed)
		{
			health = inp_health;
			damage = inp_damage;
			walkSpeed = inp_walkSpeed;
			sprintSpeed = inp_sprintSpeed;
		}
		
		public float health;
		public float damage;
		public float walkSpeed;
		public float sprintSpeed;
	}
	
	public struct LocalGridVariables
	{
		public LocalGridVariables(Vector2 inp_gridCellPosition, GridCell inp_targetGridCell, GridCell inp_nextPathCell)
		{
			gridCellPosition = inp_gridCellPosition;
			targetGridCell = inp_targetGridCell;
			nextPathCell = inp_nextPathCell;
		}
		
		public Vector2 gridCellPosition;
		public GridCell targetGridCell;
		public GridCell nextPathCell;
	}
	
	public enum ENEMYSTATE { Idle, Roaming, Attacking };
	
	public virtual void DamagePlayer(float attackDamage)
	{
		PlayerController.Instance.DamagePlayer(attackDamage);
	}
	
	protected abstract void EnemyBehaviour();
	
	public abstract void DamageEnemy(float incomingDamage);
}
