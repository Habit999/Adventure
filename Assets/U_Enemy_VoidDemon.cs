using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Enemy_VoidDemon : A_Enemy
{
	public ENEMYSTATE EnemyState = ENEMYSTATE.Idle;
	
	public EnemyData _currentEnemyData = new EnemyData(100, 30, 10, 20);
	
	public LocalGridVariables _currentLocalGridVriables = new LocalGridVariables(Vector2.zero, null, null);
	
	public struct VoidData
	{
		public VoidData(float inp_idleTime, float inp_maxMovedSpacesTillIdle, float inp_maxLightDimmingDistance)
		{
			idleTime = inp_idleTime;
			maxMovedSpacesTillIdle = inp_maxMovedSpacesTillIdle;
			maxLightDimmingDistance = inp_maxLightDimmingDistance;
		}
		
		public float idleTime;
		public float maxMovedSpacesTillIdle;
		public float maxLightDimmingDistance;
	}
	public VoidData _currentVoidData = new VoidData(3, 40, 30);
	
	public float GetBodyHalfHeight { get { return body.localScale.y / 2; } }
	
	[SerializeField] Transform body;
	
	IEnumerator idleRoutine;
	bool isIdling;
	bool isRoaming;
	
	void Awake()
	{
		isIdling = false;
		isRoaming = false;
		
		CheckClosestCell();
	}
	
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
				
			case ENEMYSTATE.Roaming:
				RoamingState();
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
		if(!isIdling)
		{
			isIdling = true;
			idleRoutine = IdleToRoamTime(_currentVoidData.idleTime);
		}
	}
	
	void RoamingState()
	{
		if(!isRoaming) GetRoamingTarget();
		
		if(_currentLocalGridVriables.gridCellPosition != _currentLocalGridVriables.targetGridCell._cellIndex)
		{
			isRoaming = true;
			
			Vector3 moveTarget = _currentLocalGridVriables.targetGridCell.transform.position;
			moveTarget.y += GetBodyHalfHeight;
			transform.position += ((moveTarget - transform.position).normalized * _currentEnemyData.walkSpeed) * Time.deltaTime;
		}
	}
	
	#endregion
	
	#region Checks & Routines
	
	void CheckClosestCell()
	{
		GridCell closestCell = null;
		foreach(CustomGrid grid in CustomGrid.ActiveGrids)
		{
			foreach(GridCell cell in grid._gridCells)
			{
				if(closestCell == null) closestCell = cell;
				else
				{
					if(Vector3.Distance(cell.transform.position, transform.position) < Vector3.Distance(closestCell.transform.position, transform.position))
					{
						closestCell = cell;
					}
				}
			}
		}
		
		_currentLocalGridVriables.gridCellPosition = closestCell._cellIndex;
		_currentLocalGridVriables.targetGridCell = closestCell;
	}
	
	void GetRoamingTarget()
	{
		CustomGrid currentGrid = _currentLocalGridVriables.targetGridCell._connectedGrid;
		Vector2 currentCellPos = _currentLocalGridVriables.gridCellPosition;
		
		GridCell[] checkSpaces = new GridCell[4];
		GridCell[] availableSpaces;
		
		// Check Spaces
		int spacesFree = 0;
		if(currentCellPos.x + 1 < currentGrid._gridLengthX)
		{
			checkSpaces[0] = currentGrid._gridCells[(int) currentCellPos.x + 1, (int) currentCellPos.y];
			if(currentGrid._gridCulling[(int) checkSpaces[0]._cellIndex.x, (int) checkSpaces[0]._cellIndex.y])
			{
				spacesFree++;
			}
		}
		if(currentCellPos.x - 1 >= 0)
		{
			checkSpaces[1] = currentGrid._gridCells[(int) currentCellPos.x - 1, (int) currentCellPos.y];
			if(currentGrid._gridCulling[(int) checkSpaces[1]._cellIndex.x, (int) checkSpaces[1]._cellIndex.y])
			{
				spacesFree++;
			}
		}
		if(currentCellPos.y + 1 < currentGrid._gridLengthZ)
		{
			checkSpaces[2] = currentGrid._gridCells[(int) currentCellPos.x, (int) currentCellPos.y + 1];
			if(currentGrid._gridCulling[(int) checkSpaces[2]._cellIndex.x, (int) checkSpaces[2]._cellIndex.y])
			{
				spacesFree++;
			}
		}
		if(currentCellPos.y - 1 >= 0)
		{
			checkSpaces[3] = currentGrid._gridCells[(int) currentCellPos.x, (int) currentCellPos.y - 1];
			if(currentGrid._gridCulling[(int) checkSpaces[3]._cellIndex.x, (int) checkSpaces[3]._cellIndex.y])
			{
				spacesFree++;
			}
		}
		
		// Decide From Free Spaces
		availableSpaces = new GridCell[spacesFree];
		int spaceIndex = 0;
		foreach(GridCell checkCell in checkSpaces)
		{
			if(currentGrid._gridCulling[(int) checkCell._cellIndex.x, (int) checkCell._cellIndex.y])
			{
				availableSpaces[spaceIndex] = checkCell;
				spaceIndex++;
			}
		}
		
		_currentLocalGridVriables.targetGridCell = availableSpaces[Random.Range(0, spacesFree)];
		
		/*int xOrY = Random.Range(0, 1);
		bool directionFree1X = false;
		bool directionFree2X = false;
		bool directionFree1Y = false;
		bool directionFree2Y = false;
		
		Vector2 currentCellPos = _currentLocalGridVriables.gridCellPosition;
		
		// Direction free?
		if(currentCellPos.x + 1 < currentGrid._gridLengthX)
		{
			directionFree1 = currentGrid._gridCulling[currentCellPos.x + 1, currentCellPos.y];
		}
		if(currentCellPos.x - 1 >= 0)
		{
			directionFree1 = currentGrid._gridCulling[currentCellPos.x - 1, currentCellPos.y];
		}
		if(currentCellPos.y + 1 < currentGrid._gridLengthY)
		{
			directionFree1 = currentGrid._gridCulling[currentCellPos.y + 1, currentCellPos.y];
		}
		if(currentCellPos.y - 1 >= 0)
		{
			directionFree1 = currentGrid._gridCulling[currentCellPos.y - 1, currentCellPos.y];
		}
		
		// Trapped?
		if(!directionFree1X && !directionFree2X && !directionFree1Y && !directionFree2Y)
		{
			EnemyState = EnemyState.Idle;
			return;
		}
		
		// Switch?
		if(xOrY == 1 && (!directionFree1X && !directionFree2X))
		{
			
		}
		
		// Decide
		if(xOrY == 1)
		{
			if(directionFree1X && directionFree2X)
			{
				int direction = Random.Range(0, 1);
			}
			else if(!directionFree1X && !directionFree2X)
			{
				
			}
			else
			{
				
			}
			
			if(directionFree1X)
			{
				
			}
			else if(directionFree2X)
			{
				
			}
		}
		else
		{
			if(directionFree1Y)
			{
				
			}
			else if(directionFree2Y)
			{
				
			}
		}*/
	}
	
	IEnumerator IdleToRoamTime(float incomingWaitTime)
	{
		yield return new WaitForSeconds(incomingWaitTime);
		isIdling = false;
		EnemyState = ENEMYSTATE.Roaming;
	}
	
	#endregion
}
