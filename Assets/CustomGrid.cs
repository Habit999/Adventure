using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class CustomGrid : MonoBehaviour
{
	[HideInInspector] public GridData _gridData;
	
	public string GridDataPath { get { return Application.dataPath + "/CustomGridData.json"; } }
	
	public delegate void CellOccupantInstantiation();
	public static event CellOccupantInstantiation SpawnCellOccupants;
	
	protected Vector3 CellScale { get { return _gridCellPrefab.transform.localScale; } }
	
	public struct GridGenerationData
	{
		public Vector3[,] _cellPositions;
		public GameObject[,] _spawnedCells;
		public bool[,] _activeCells;
		
		public GameObject[,] _cellOccupantPrefabs;
		public GameObject[,] _cellActiveOccupant;
		
		public Vector3[,] _cellOccupantPositions;
		public Vector3[,] _cellOccupantEulerAngles;
	}
	public GridGenerationData GeneratedData = new GridGenerationData();
	
	public GameObject _gridCellPrefab;
	[SerializeField] SO_PrefabLibrary prefabLibrary;
	
	[Header("Customise Grid")]
	public int _gridLengthX;
	public int _gridLengthZ;
	
	[Space(5)]
	
	public float _cellSpacing;
	
	[Space(5)]
	
	public float _cellOffsetX;
	public float _cellOffsetZ;
	
	[Space(5)]
	
	[HideInInspector] public bool _generationComplete;
	[HideInInspector] public bool _cellsSpawned;
	
	[Space(10)]
	
	[HideInInspector] public bool _enableEditorTools;
	[HideInInspector] public bool _isGridActive;
	[HideInInspector] public bool _showGrid;
	[HideInInspector] public bool _gridPreviewToggled;
	[HideInInspector] public bool _cellOccupantsToggled;
	
	void Awake()
	{
		_enableEditorTools = false;
		_isGridActive = true;
		
		_generationComplete = false;
		_cellsSpawned = false;
	}
	
	void Start()
	{
		if(_gridPreviewToggled) TogglePreviewGrid();
		
		GenerateGrid();
		LoadGridData();
		
		SpawnActiveGrid();
	}
	
	#region Grid Control
	
	public void GenerateGrid()
	{
		GeneratedData._cellPositions = new Vector3[_gridLengthX, _gridLengthZ];
		GeneratedData._activeCells = new bool[_gridLengthX, _gridLengthZ];
		
		GeneratedData._cellOccupantPrefabs = new GameObject[_gridLengthX, _gridLengthZ];
		
		GeneratedData._cellOccupantPositions = new Vector3[_gridLengthX, _gridLengthZ];
		GeneratedData._cellOccupantEulerAngles = new Vector3[_gridLengthX, _gridLengthZ];
		
		// Make sure no left over cells are in the scene
		int amountOfChildren = transform.childCount;
		if(amountOfChildren > 0)
		{
			for(int i = 0; i < amountOfChildren; i++)
			{
				DestroyImmediate(transform.GetChild(0).gameObject, false);
			}
			if(_gridPreviewToggled) TogglePreviewGrid();
		}
		
		// Generate layout
		Vector3 generatedCellPosition;
		for(int axisX = 0; axisX < _gridLengthX; axisX++)
		{
			if(axisX == 0)
			{
				generatedCellPosition = transform.position;
				generatedCellPosition.x += CellScale.x / 2;
				generatedCellPosition.z += CellScale.z / 2;
				GeneratedData._cellPositions[0, 0] = generatedCellPosition;
			}
			else
			{
				generatedCellPosition = GeneratedData._cellPositions[axisX - 1, 0];
				generatedCellPosition.x += CellScale.x + _cellSpacing + _cellOffsetX;
				GeneratedData._cellPositions[axisX, 0] = generatedCellPosition;
			}
			
			for(int axisZ = 1; axisZ < _gridLengthZ; axisZ++)
			{
				generatedCellPosition = GeneratedData._cellPositions[axisX, axisZ - 1];
				generatedCellPosition.z += CellScale.z + _cellSpacing + _cellOffsetZ;
				GeneratedData._cellPositions[axisX, axisZ] = generatedCellPosition;
			}
		}
		
		_generationComplete = true;
	}
	
	public void SpawnGrid()
	{
		GeneratedData._spawnedCells = new GameObject[_gridLengthX, _gridLengthZ];
		
		for(int axisX = 0; axisX < _gridLengthX; axisX++)
		{
			for(int axisZ = 0; axisZ < _gridLengthZ; axisZ++)
			{
				GameObject cellInstance = Instantiate(_gridCellPrefab, GeneratedData._cellPositions[axisX, axisZ], Quaternion.identity);
				cellInstance.transform.parent = transform;
				
				// Modify cell script here
				GridCell cellScript = cellInstance.GetComponent<GridCell>();
				cellScript._connectedGrid = this;
				cellScript._cellActive = GeneratedData._activeCells[axisX, axisZ];
				cellScript._gridIndex = new Vector2(axisX, axisZ);
				cellScript._occupantPrefab = GeneratedData._cellOccupantPrefabs[axisX, axisZ];
				
				GeneratedData._spawnedCells[axisX, axisZ] = cellInstance;
			}
		}
		
		_cellsSpawned = true;
	}
	
	public void SpawnActiveGrid()
	{
		GeneratedData._spawnedCells = new GameObject[_gridLengthX, _gridLengthZ];
		
		for(int axisX = 0; axisX < _gridLengthX; axisX++)
		{
			for(int axisZ = 0; axisZ < _gridLengthZ; axisZ++)
			{
				if(GeneratedData._activeCells[axisX, axisZ])
				{
					GameObject cellInstance = Instantiate(_gridCellPrefab, GeneratedData._cellPositions[axisX, axisZ], Quaternion.identity);
					cellInstance.transform.parent = transform;
					
					// Modify cell script here
					GridCell cellScript = cellInstance.GetComponent<GridCell>();
					cellScript._connectedGrid = this;
					cellScript._cellActive = true;
					cellScript._gridIndex = new Vector2(axisX, axisZ);
					cellScript._occupantPrefab = GeneratedData._cellOccupantPrefabs[axisX, axisZ];
					
					GeneratedData._spawnedCells[axisX, axisZ] = cellInstance;
				}
			}
		}
		
		if(SpawnCellOccupants != null)
			SpawnCellOccupants();
		
		_cellsSpawned = true;
	}
	
	public void TogglePreviewGrid()
	{
		if(_isGridActive) return;
		
		_gridPreviewToggled = !_gridPreviewToggled;
		
		if(_gridPreviewToggled)
		{
			SpawnGrid();
		}
		else
		{
			if(_cellOccupantsToggled) ToggleCellOccupants();
			
			foreach(GameObject cell in GeneratedData._spawnedCells)
			{
				DestroyImmediate(cell, false);
				GeneratedData._spawnedCells = null;
			}
		}
	}
	
	public void ToggleCellOccupants()
	{
		_cellOccupantsToggled = !_cellOccupantsToggled;
		
		if(_cellOccupantsToggled)
		{
			GeneratedData._cellActiveOccupant = new GameObject[_gridLengthX, _gridLengthZ];
			
			foreach(GameObject cell in GeneratedData._spawnedCells)
			{
				cell.GetComponent<GridCell>().SpawnOccupant();
				
				if(cell.GetComponent<GridCell>()._activeOccupant != null)
				{
					Vector2 cellIndex = cell.GetComponent<GridCell>()._gridIndex;
					GeneratedData._cellActiveOccupant[(int) cellIndex.x, (int) cellIndex.y] = cell.GetComponent<GridCell>()._activeOccupant;
				}
			}
		}
		else
		{
			foreach(GameObject cell in GeneratedData._spawnedCells)
			{
				if(cell.GetComponent<GridCell>()._activeOccupant != null)
				{
					DestroyImmediate(cell.GetComponent<GridCell>()._activeOccupant, false);
				}
				
				GeneratedData._cellActiveOccupant = null;
			}
		}
	}
	
	public void UpdateGridFromCellData()
	{
		for(int x = 0; x < _gridLengthX; x++)
		{
			for(int z = 0; z < _gridLengthZ; z++)
			{
				GeneratedData._activeCells[x, z] = GeneratedData._spawnedCells[x, z].GetComponent<GridCell>()._cellActive;
				GeneratedData._cellOccupantPrefabs[x, z] = GeneratedData._spawnedCells[x, z].GetComponent<GridCell>()._occupantPrefab;
				GeneratedData._cellOccupantPositions[x, z] = GeneratedData._spawnedCells[x, z].GetComponent<GridCell>()._occupantPosition;
				GeneratedData._cellOccupantEulerAngles[x, z] = GeneratedData._spawnedCells[x, z].GetComponent<GridCell>()._occupantEulerAngles;
				
				GeneratedData._spawnedCells[x, z].GetComponent<GridCell>().UpdateOccupantTransform();
			}
		}
	}
	
	#endregion
	
	#region Saving & Loading
	
	public bool SaveGridData()
	{
		if(!_generationComplete) return false;
		
		_gridData = new GridData();
		
		_gridData.isCellActive = new bool[_gridLengthX * _gridLengthZ];
		_gridData.occupantPrefabIDs = new int[_gridLengthX * _gridLengthZ];
		_gridData.occupantPositions = new Vector3[_gridLengthX * _gridLengthZ];
		_gridData.occupantEulerAngles = new Vector3[_gridLengthX * _gridLengthZ];
		
		// Store data
		_gridData.lengthX = _gridLengthX;
		_gridData.lengthZ = _gridLengthZ;
		
		_gridData.spacing = _cellSpacing;
		
		_gridData.offsetX = _cellOffsetX;
		_gridData.offsetZ = _cellOffsetZ;
		
		int dataIndex = 0;
		for(int x = 0; x < _gridLengthX; x++)
		{
			for(int z = 0; z < _gridLengthZ; z++)
			{
				_gridData.isCellActive[dataIndex] = GeneratedData._activeCells[x, z];
				
				// Setting cell data
				_gridData.occupantPrefabIDs[dataIndex] = -1;
				for(int i = 0; i < prefabLibrary._gridPresets.Count; i++)
				{
					if(prefabLibrary._gridPresets[i] == GeneratedData._cellOccupantPrefabs[x, z])
					{
						_gridData.occupantPrefabIDs[dataIndex] = i;
						break;
					}
				}
				_gridData.occupantPositions[dataIndex] = GeneratedData._cellOccupantPositions[x, z];
				_gridData.occupantEulerAngles[dataIndex] = GeneratedData._cellOccupantEulerAngles[x, z];
				
				dataIndex++;
			}
		}
		
		SaveData(_gridData);
		return true;
	}
	
	public bool LoadGridData()
	{
		GridData loadedData = LoadData();
		if(loadedData == null) return false;
		else _gridData = loadedData;
		
		_gridLengthX = _gridData.lengthX;
		_gridLengthZ = _gridData.lengthZ;
		
		_cellSpacing = _gridData.spacing;
		
		_cellOffsetX = _gridData.offsetX;
		_cellOffsetZ = _gridData.offsetZ;
		
		int dataIndex = 0;
		for(int x = 0; x < _gridLengthX; x++)
		{
			for(int z = 0; z < _gridLengthZ; z++)
			{
				GeneratedData._activeCells[x, z] = _gridData.isCellActive[dataIndex];
				
				// Setting cell data
				for(int i = 0; i < prefabLibrary._gridPresets.Count; i++)
				{
					if(i == _gridData.occupantPrefabIDs[dataIndex])
					{
						GeneratedData._cellOccupantPrefabs[x, z] = prefabLibrary._gridPresets[i];
						break;
					}
					else GeneratedData._cellOccupantPrefabs[x, z] = null;
				}
				GeneratedData._cellOccupantPositions[x, z] = _gridData.occupantPositions[dataIndex];
				GeneratedData._cellOccupantEulerAngles[x, z] = _gridData.occupantEulerAngles[dataIndex];
				
				dataIndex++;
			}
		}
		
		return true;
	}
	
	void SaveData(GridData gData)
	{
		if(File.Exists(GridDataPath))
		{
			File.Delete(GridDataPath);
		}
		
		string jsonData = JsonUtility.ToJson(gData);
		File.WriteAllText(GridDataPath, jsonData);
	}
	
	GridData LoadData()
	{
		if(File.Exists(GridDataPath))
		{
			string jsonData = File.ReadAllText(GridDataPath);
			return _gridData = JsonUtility.FromJson<GridData>(jsonData);
		}
		else return null;
	}
	
	#endregion
	
	#if UNITY_EDITOR
	
	void OnDrawGizmos()
	{
		if(_enableEditorTools && _showGrid && GeneratedData._cellPositions != null && GeneratedData._activeCells != null)
		{
			if(GeneratedData._cellPositions.GetLength(0) == _gridLengthX && GeneratedData._cellPositions.GetLength(1) == _gridLengthZ)
			{
				for(int x = 0; x < _gridLengthX; x++)
				{
					for(int z = 0; z < _gridLengthZ; z++)
					{
						if(GeneratedData._activeCells.GetLength(0) == _gridLengthX && GeneratedData._activeCells.GetLength(1) == _gridLengthZ)
						{
							Gizmos.color = GeneratedData._activeCells[x, z]? Color.green : Color.red;
						}
						else Gizmos.color = Color.yellow;
						Gizmos.DrawWireCube(GeneratedData._cellPositions[x, z], CellScale);
					}
				}
			}
		}
	}
	
	#endif
}

[System.Serializable]
public class GridData
{
	public int lengthX;
	public int lengthZ;
	
	public float spacing;
	
	public float offsetX;
	public float offsetZ;
	
	public bool[] isCellActive;
	public int[] occupantPrefabIDs;
	public Vector3[] occupantPositions;
	public Vector3[] occupantEulerAngles;
}
