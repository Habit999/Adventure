using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CustomGrid : MonoBehaviour
{
	public static CustomGrid Instance;
		
	// Grid Data Variables
	protected static string GridDataPath { get { return Application.dataPath + $"/CustomGridData.json"; } }
	
	[HideInInspector] public Vector2[,] _gridPositions;
	[HideInInspector] public GridCell[,] _gridCells;
	//[HideInInspector] public bool[,] _gridCulling = new bool[Instance._gridLengthX, Instance._gridLengthZ];
	[SerializeField] public GridData gridData;
		
	public int _gridLengthX;
	public int _gridLengthZ;
	public float _gridCellSpacing;
	
	[Space(2)]
	
	public float _gridOffsetX;
	public float _gridOffsetZ;
	
	[Space(5)]
	
	public GameObject _gridCellPrefab;
	
	[HideInInspector] public bool _initialGenerationComplete = false;
	[HideInInspector] public bool _isGridVisible = true;
	
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
	
	public void GenerateGrid()
	{
		/*if(_gridCells.Length > 0 && _gridCells != null)
		{
			foreach(GridCell cell in _gridCells)
			{
				Destroy(cell.gameObject);
			}
		}*/
		
		_gridPositions = new Vector2[_gridLengthX, _gridLengthZ];
		_gridCells = new GridCell[_gridLengthX, _gridLengthZ];
		
		gridData = LoadGridData();
		
		float xAxis;
		float zAxis;
		
		// Grid X Axis
		for(int x = 0; x < _gridLengthX; x++)
		{
			// Grid X Axis
			//if(!gridData._gridCulling[x, 0]) continue;
			
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
				//if(!gridData._gridCulling[x, z]) continue;
				
				xAxis = _gridPositions[x, z - 1].x;
				zAxis = _gridPositions[x, z - 1].y + _gridOffsetZ + _gridCellSpacing;
				
				Vector3 lastCellPosition = new Vector3(xAxis, 0, zAxis);
				_gridPositions[x, z] = new Vector2(lastCellPosition.x, lastCellPosition.z);
			}
			
			_initialGenerationComplete = true;
			
			Debug.Log("Grid Generation Complete");
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
			if(gridData._gridCulling[xPos, yPos])
				_gridCells[xPos, yPos] = Instantiate(_gridCellPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<GridCell>();
			if(xPos < _gridLengthX) xPos++;
			else break;
			if(yPos < _gridLengthZ) yPos++;
			else yPos = 0;
		}
	}
	
	#region Saving & Loading Grid Data
	
	public void SaveGridData()
	{
		if(File.Exists(GridDataPath))
		{
			File.Delete(GridDataPath);
		}
		
		string jsonData = JsonUtility.ToJson(gridData);
		System.IO.File.WriteAllText(GridDataPath, jsonData);
	}
	
	public GridData LoadGridData()
	{
		if(File.Exists(GridDataPath))
		{
			string jsonData = File.ReadAllText(GridDataPath);
			return gridData = JsonUtility.FromJson<GridData>(jsonData);
		}
		else return new GridData();
	}
	
	#endregion
	
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		// DRAW GRID WITH GIZMOS
		if(_isGridVisible)
		{
			int xPos = 0;
			int yPos = 0;
			
			if(!_initialGenerationComplete) GenerateGrid();
			
			foreach(Vector3 gridPos in _gridPositions)
			{
				if(gridData._gridCulling[xPos, yPos]) Gizmos.color = Color.green;
				else Gizmos.color = Color.red;
				Vector3 drawPosition = gridPos;
				drawPosition.z = drawPosition.y;
				drawPosition.y = 0;
				Gizmos.DrawWireCube(drawPosition, _gridCellPrefab.transform.localScale);
				if(xPos < _gridLengthX) xPos++;
				else break;
				if(yPos < _gridLengthZ) yPos++;
				else yPos = 0;
			}
		}
	}
	#endif
}

[System.Serializable]
public class GridData
{
	public bool[,] _gridCulling;
}
