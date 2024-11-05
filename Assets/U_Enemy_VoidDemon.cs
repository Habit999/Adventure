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
	
	[Space(3)]
	
	public CustomGrid _startGrid;
	
	[Space(3)]
	
	[SerializeField] Transform body;
	
	IEnumerator idleRoutine;
	bool isIdling;
	bool isRoaming;
	
	bool gridFound;
	
	void Start()
	{
		isIdling = false;
		isRoaming = false;
		
		gridFound = (_startGrid != null)? true : false;
		
		StartCoroutine(CheckClosestCell());
	}
	
	void Update()
	{
		EnemyBehaviour();
	}
	
	protected override void EnemyBehaviour()
	{
		BehaviourCheck();
		
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
		if(!isIdling && gridFound)
		{
			isIdling = true;
			idleRoutine = IdleToRoamTime(_currentVoidData.idleTime);
			StartCoroutine(idleRoutine);
		}
	}
	
	void RoamingState()
	{
		if(gridFound)
		{
			if(!isRoaming) GetRoamingTarget();
			
			if(isRoaming && _currentLocalGridVriables.targetGridCell != null)
			{
				if(_currentLocalGridVriables.gridCellPosition != _currentLocalGridVriables.targetGridCell._cellIndex)
				{
					isRoaming = true;
					
					Vector3 moveTarget = _currentLocalGridVriables.targetGridCell.transform.position;
					moveTarget.y += GetBodyHalfHeight;
					transform.position += ((moveTarget - transform.position).normalized * _currentEnemyData.walkSpeed) * Time.deltaTime;
				}
			}
		}
	}
	
	#endregion
	
	#region Checks & Routines
	
	void BehaviourCheck()
	{
		if(EnemyState != ENEMYSTATE.Idle && isIdling)
		{
			StopCoroutine(idleRoutine);
			isIdling = false;
		}
	}
	
	IEnumerator CheckClosestCell()
	{
		GridCell closestCell = null;
		
		if(_startGrid == null)
		{
			foreach(CustomGrid grid in CustomGrid.ActiveGrids)
			{
				yield return new WaitWhile(() => grid._gridCells == null);
				foreach(GridCell cell in grid._gridCells)
				{
					if(cell != null)
					{
						if(closestCell == null) closestCell = cell;
					
						if(Vector3.Distance(cell.transform.position, transform.position) < Vector3.Distance(closestCell.transform.position, transform.position))
						{
							closestCell = cell;
						}
					}
				}
			}
		}
		else
		{
			yield return new WaitWhile(() => _startGrid._gridCells == null);
			yield return new WaitWhile(() => _startGrid._gridCells.GetLength(0) != _startGrid._gridLengthX && _startGrid._gridCells.GetLength(1) != _startGrid._gridLengthZ);
			foreach(GridCell cell in _startGrid._gridCells)
			{
				if(cell != null)
				{
					if(closestCell == null) closestCell = cell;
				
					if(Vector3.Distance(cell.transform.position, transform.position) < Vector3.Distance(closestCell.transform.position, transform.position))
					{
						closestCell = cell;
					}
				}
			}
		}
		
		_currentLocalGridVriables.gridCellPosition = closestCell._cellIndex;
		_currentLocalGridVriables.targetGridCell = closestCell;
		gridFound = true;
	}
	
	void GetRoamingTarget()
	{
		CustomGrid currentGrid = (_startGrid == null)? _currentLocalGridVriables.targetGridCell._connectedGrid : _startGrid;
		Vector2 currentCellPos = _currentLocalGridVriables.gridCellPosition;
		
		Vector2[] checkSpaces = new Vector2[4];
		GridCell[] availableSpaces;
		
		// Check Spaces
		int spacesFree = 0;
		
		if(currentCellPos.x + 1 < currentGrid._gridLengthX)
		{
			Vector2 assignVector = currentCellPos;
			assignVector.x += 1;
			if(currentGrid._gridCulling[(int) assignVector.x, (int) assignVector.y])
			{
				checkSpaces[0] = currentCellPos;
				spacesFree++;
			}
		}
		if(currentCellPos.x - 1 >= 0)
		{
			Vector2 assignVector = currentCellPos;
			assignVector.x -= 1;
			if(currentGrid._gridCulling[(int) assignVector.x, (int) assignVector.y])
			{
				checkSpaces[1] = currentCellPos;
				spacesFree++;
			}
		}
		if(currentCellPos.y + 1 < currentGrid._gridLengthZ)
		{
			Vector2 assignVector = currentCellPos;
			assignVector.y += 1;
			if(currentGrid._gridCulling[(int) assignVector.x, (int) assignVector.y])
			{
				checkSpaces[2] = currentCellPos;
				spacesFree++;
			}
		}
		if(currentCellPos.y - 1 >= 0)
		{
			Vector2 assignVector = currentCellPos;
			assignVector.y -= 1;
			if(currentGrid._gridCulling[(int) assignVector.x, (int) assignVector.y])
			{
				checkSpaces[3] = currentCellPos;
				spacesFree++;
			}
		}
		
		// Decide From Free Spaces
		availableSpaces = new GridCell[spacesFree];
		int availableIndex = 0;
		foreach(Vector2 checkCell in checkSpaces)
		{
			availableSpaces[availableIndex] = currentGrid._gridCells[(int) checkCell.x, (int) checkCell.y];
		}
		
		_currentLocalGridVriables.targetGridCell = availableSpaces[Random.Range(0, spacesFree)];
		
		/*if(currentCellPos.x + 1 < currentGrid._gridCells.GetLength(0) && currentCellPos.x + 1 < currentGrid._gridLengthX)
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
		if(currentCellPos.y + 1 < currentGrid._gridCells.GetLength(1) && currentCellPos.y + 1 < currentGrid._gridLengthZ)
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
		}*/
		
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
