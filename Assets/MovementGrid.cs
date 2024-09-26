using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGrid : MonoBehaviour
{
	public static MovementGrid Instance;
	
	[HideInInspector] public Vector2[,] _gridPositions;
	[HideInInspector] public GridCell[,] _gridCells;
		
	public int _gridLengthX;
	public int _gridLengthZ;
	public float _gridCellSpacing;
	
	[Space(2)]
	
	public float _gridOffsetX;
	public float _gridOffsetZ;
	
	[Space(5)]
	
	public GameObject _gridCellPrefab;
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
	}
	
	void Start()
	{
		GenerateGrid();
		SpawnGrid();
	}
	
	void GenerateGrid()
	{
		_gridPositions = new Vector2[_gridLengthX, _gridLengthZ];
		
		float xAxis;
		float zAxis;
		
		// Grid X Axis
		for(int x = 0; x < _gridLengthX; x++)
		{
			if(x == 0)
			{
				Vector3 lastCellPosition = new Vector3(_gridOffsetX, 0, _gridOffsetZ);
				_gridPositions[x, 0] = new Vector2(lastCellPosition.x, lastCellPosition.z);
			}
			else
			{
				xAxis = _gridPositions[x - 1, 0].x + _gridOffsetX + _gridCellSpacing;
				
				Vector3 lastCellPosition = new Vector3(xAxis, 0, _gridOffsetZ);
				_gridPositions[x, 0] = new Vector2(lastCellPosition.x, lastCellPosition.z);
			}
			
			// Grid Z Axis
			for(int z = 1; z < _gridLengthZ; z++)
			{
				xAxis = _gridPositions[x, z - 1].x;
				zAxis = _gridPositions[x, z - 1].y + _gridOffsetZ + _gridCellSpacing;
				
				Vector3 lastCellPosition = new Vector3(xAxis, 0, zAxis);
				_gridPositions[x, z] = new Vector2(lastCellPosition.x, lastCellPosition.z);
			}
		}
	}
	
	void SpawnGrid()
	{
		int xPos = 0;
		int yPos = 0;
		foreach(Vector3 gridPos in _gridPositions)
		{
			Vector3 spawnPosition = gridPos;
			spawnPosition.z = spawnPosition.y;
			spawnPosition.y = 0;
			_gridCells[xPos, yPos] = Instantiate(_gridCellPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<GridCell>();
			if(xPos < _gridLengthX) xPos++;
			else break;
			if(yPos < _gridLengthZ) yPos++;
			else yPos = 0;
		}
	}
	
	#if UNITY_EDITOR
	void OnDrawGizmosSelected()
	{
		// DRAW GRID WITH GIZMOS

		/*Vector3 lastPositionX = Vector3.zero;
		Vector3 lastPositionZ = Vector3.zero;
		for(int x = 0; x < _gridLengthX; x++)
		{
			if(x == 0)
			{
				Gizmos.color = Color.green;
				lastPositionX = transform.position + new Vector3(_gridOffsetX, 0, _gridOffsetZ);
				Gizmos.DrawWireCube(lastPositionX, _gridCellPrefab.transform.localScale);
			}
			else
			{
				Gizmos.color = Color.green;
				lastPositionX = lastPositionX + new Vector3(_gridOffsetX + _gridCellSpacing, 0, _gridOffsetZ + _gridCellSpacing);
				Gizmos.DrawWireCube(lastPositionX, _gridCellPrefab.transform.localScale);
			}
			
			for(int y = 0; y < _gridLengthZ; y++)
			{
				if(y == 0)
				{
					lastPositionZ = lastPositionX;
				}
				else
				{
					Gizmos.color = Color.green;
					lastPositionZ = lastPositionZ + new Vector3(_gridOffsetX + _gridCellSpacing, 0, _gridOffsetZ + _gridCellSpacing);
					Gizmos.DrawWireCube(lastPositionZ, _gridCellPrefab.transform.localScale);
				}
			}*/
			
		GenerateGrid();
		Gizmos.color = Color.green;
		foreach(Vector3 gridPos in _gridPositions)
		{
			Vector3 drawPosition = gridPos;
			drawPosition.z = drawPosition.y;
			drawPosition.y = 0;
			Gizmos.DrawWireCube(drawPosition, _gridCellPrefab.transform.localScale);
		}
	}
	#endif
}
