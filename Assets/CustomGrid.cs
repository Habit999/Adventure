using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class CustomGrid : MonoBehaviour
{
	public static CustomGrid Instance;
		
	// Grid Data Variables
	protected static string GridDataPath { get { return Application.dataPath + $"/CustomGridData.json"; } }
	
	[HideInInspector] public Vector2[,] _gridPositions;
	[HideInInspector] public GridCell[,] _gridCells;
	
	[SerializeField] public GridData gridData;
		
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
	[HideInInspector] public bool _isGridVisible = false;
	
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
		if(_initialGenerationComplete && _gridCells != null)
		{
			foreach(GridCell cell in _gridCells)
			{
				if(cell != null) Destroy(cell.gameObject);
			}
		}
		
		_gridPositions = new Vector2[_gridLengthX, _gridLengthZ];
		_gridCells = new GridCell[_gridLengthX, _gridLengthZ];
		
		//Replace this line with load gridData
		gridData._gridCulling = new bool[_gridLengthX, _gridLengthZ];
		
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
			if(gridData._gridCulling[xPos, yPos])
				_gridCells[xPos, yPos] = Instantiate(_gridCellPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<GridCell>();
			if(xPos < _gridLengthX) xPos++;
			else break;
			if(yPos < _gridLengthZ) yPos++;
			else yPos = 0;
		}
	}
	
	#region Saving & Loading Grid Data
	
	public void SaveGridData(GridData gData)
	{
		gridData = gData;
		
		if(File.Exists(GridDataPath))
		{
			File.Delete(GridDataPath);
		}
		
		string jsonData = JsonUtility.ToJson(gData);
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
	
	public bool CheckStoredGridData()
	{
		/*if(!File.Exists(GridDataPath)) return false;
		
		GridData loadedData = LoadGridData();
		foreach(bool loadedCell in loadedData._gridCulling)
		{
			foreach(bool gridCell in gridData._gridCulling)
			{
				if(gridCell != loadedCell) return false;
			}
		}*/
		
		return true;
	}
	
	#endregion
	
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		// DRAW GRID WITH GIZMOS
		if(_isGridVisible)
		{
			if(_initialGenerationComplete) GenerateGrid();
			
			for(int x = 0; x < _gridLengthX; x++)
			{
				for(int y = 0; y < _gridLengthZ; y++)
				{
					if(gridData._gridCulling[x, y]) Gizmos.color = Color.green;
					else Gizmos.color = Color.red;
					Vector3 drawPosition = _gridPositions[x, y];
					drawPosition.z = drawPosition.y;
					drawPosition.y = 0;
					Gizmos.DrawWireCube(drawPosition, _gridCellPrefab.transform.localScale);
				}
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
