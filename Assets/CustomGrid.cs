using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class CustomGrid : MonoBehaviour
{
	public static List<CustomGrid> ActiveGrids = new List<CustomGrid>();
	
	// Grid Data Variables
	string GridDataPath { get { return Application.dataPath + $"/{_gridName}_CustomGridData.json"; } }
	
	[HideInInspector] public Vector2[,] _gridPositions;
	[HideInInspector] public GridCell[,] _gridCells;
	[HideInInspector] public bool[,] _gridCulling;
	[HideInInspector] public GameObject[,] _gridCellOccupants;
	
	[HideInInspector] public GridData gridData;
	
	public string _gridName;
	[Space(3)]
	public int _gridLengthX;
	public int _gridLengthZ;
	public float _gridCellSpacing;
	
	[Space(2)]
	//
	public float _gridOffsetX;
	public float _gridOffsetZ;
	
	[Space(5)]
	
	public GameObject _gridCellPrefab;
	
	[HideInInspector] public bool _initialGenerationComplete = false;
	[HideInInspector] public bool _isGridVisible = true;
	[HideInInspector] public bool _activeGridPreview = false;
	
	[Space(10)]
	
	public bool _enableEditorTools = false;
	
	void Awake()
	{
		ActiveGrids.Add(this);
	}
	
	void Start()
	{
		GenerateGrid();
		SpawnGrid();
	}
	
	public void GenerateGrid()
	{
		if(_initialGenerationComplete && _gridCells != null)
		{
			foreach(GridCell cell in _gridCells)
			{
				if(cell != null) Destroy(cell.gameObject);
			}
		}
		
		gridData = new GridData();
		gridData.cellCullingData = new bool[_gridLengthX * _gridLengthZ];
		
		_gridPositions = new Vector2[_gridLengthX, _gridLengthZ];
		_gridCells = new GridCell[_gridLengthX, _gridLengthZ];
		_gridCulling = new bool[_gridLengthX, _gridLengthZ];
		
		LoadGridData();
		
		float xAxis;
		float zAxis;
		
		// Grid X Axis
		for(int x = 0; x < _gridLengthX; x++)
		{
			// Grid X Axis			
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
			if(_gridCulling[xPos, yPos])
			{
				_gridCells[xPos, yPos] = Instantiate(_gridCellPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<GridCell>();
				_gridCells[xPos, yPos]._cellIndex = new Vector2(xPos, yPos);
				_gridCells[xPos, yPos]._connectedGrid = this;
			}
				
			if(xPos < _gridLengthX) xPos++;
			else break;
			if(yPos < _gridLengthZ) yPos++;
			else yPos = 0;
		}
	}
	
	#region Saving & Loading Grid Data
	
	// Use the public ones to store data in the correct format
	public void SaveGridData()
	{
		gridData = new GridData();
		gridData.cellCullingData = new bool[_gridLengthX * _gridLengthZ];
		gridData.cellOccupantIDData = new int[_gridLengthX * _gridLengthZ];
		
		// Converting Data:
		
		// grid culling (bool[,] => bool[])
		int dataIndex = 0;
		for(int x = 0; x < _gridLengthX; x++)
		{
			for(int y = 0; y < _gridLengthZ; y++)
			{
				gridData.cellCullingData[dataIndex] = _gridCulling[x, y];
				dataIndex++;
			}
		}
		
		// Cell Occupant ID
		dataIndex = 0;
		foreach(GameObject occupant in _gridCellOccupants)
		{
			int id = 1;
			foreach(GameObject prefab in PrefabLibrary.PrefabID.Values)
			{
				if(occupant == prefab)
				{
					gridData.cellOccupantIDData[dataIndex] = id;
					break;
				}
				id++;
			}
			dataIndex++;
		}
		
		SaveData(gridData);
	}
	
	public bool LoadGridData()
	{
		gridData = new GridData();
		gridData.cellCullingData = new bool[_gridLengthX * _gridLengthZ];
		gridData.cellOccupantIDData = new int[_gridLengthX * _gridLengthZ];
		
		var loadedData = LoadData();
		
		if(loadedData == null) return false;
		else
		{
			gridData = loadedData;
			
			// Converting Data:
			
			// grid culling (bool[] => bool[,])
			int dataIndex = 0;
			for(int x = 0; x < _gridLengthX; x++)
			{
				for(int y = 0; y < _gridLengthZ; y++)
				{
					_gridCulling[x, y] = gridData.cellCullingData[dataIndex];
					if(gridData.cellOccupantIDData[dataIndex] != 0)
						_gridCellOccupants[x, y] = PrefabLibrary.PrefabID[gridData.cellOccupantIDData[dataIndex]];
					else _gridCellOccupants[x, y] = null;
					
					dataIndex++;
				}
			}
			
			return true;
		}
	}
	
	void SaveData(GridData gData)
	{
		if(File.Exists(GridDataPath))
		{
			File.Delete(GridDataPath);
		}
		
		string jsonData = JsonUtility.ToJson(gData);
		System.IO.File.WriteAllText(GridDataPath, jsonData);
	}
	
	GridData LoadData()
	{
		if(File.Exists(GridDataPath))
		{
			string jsonData = File.ReadAllText(GridDataPath);
			return gridData = JsonUtility.FromJson<GridData>(jsonData);
		}
		else return null;
	}
	
	public bool CheckStoredGridDataCompatibility()
	{
		if(gridData.cellCullingData.Length == _gridLengthX * _gridLengthZ) return true;
		else return false;
	}
	
	#endregion
	
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		// DRAW GRID WITH GIZMOS
		if(_enableEditorTools && _isGridVisible)
		{
			try
			{
				if(!_initialGenerationComplete) GenerateGrid();
			
				if(_initialGenerationComplete)
				{
					for(int x = 0; x < _gridLengthX; x++)
					{
						for(int y = 0; y < _gridLengthZ; y++)
						{
							if(_gridCulling[x, y]) Gizmos.color = Color.green;
							else Gizmos.color = Color.red;
							Vector3 drawPosition = _gridPositions[x, y];
							drawPosition.z = drawPosition.y;
							drawPosition.y = 0;
							Gizmos.DrawWireCube(drawPosition, _gridCellPrefab.transform.localScale);
						}
					}
				}
			}
			catch(System.Exception e)
			{
				Debug.Log("Grid Not Generated OR No Custom Grid Assigned In Inspector \r\n" + e.Message);
			}
		}
	}
	#endif
}

[System.Serializable]
public class GridData
{
	public bool[] cellCullingData;
	public int[] cellOccupantIDData;
}
