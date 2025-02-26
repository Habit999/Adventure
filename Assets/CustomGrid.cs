using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
	public SO_GridData GridData;

	public event Action OnGridSpawned;
	
	public delegate void CellOccupantInstantiation();
	public static event CellOccupantInstantiation SpawnCellOccupants;
	
	protected Vector3 CellScale { get { return GridCellPrefab.transform.localScale; } }
	
	public struct GridGenerationData
	{
		public Vector3[,] CellPositions;
		public GridCell[,] SpawnedCells;
		public bool[,] ActiveCells;
		
		public GameObject[,] CellOccupantPrefabs;
		public GameObject[,] CellActiveOccupants;
		public bool[,] CellOccupantsActiveOnSpawn;

        public Vector3[,] CellOccupantPositions;
		public Vector3[,] CellOccupantEulerAngles;
	}
	public GridGenerationData GeneratedData = new GridGenerationData();
	
	public GameObject GridCellPrefab;
	[SerializeField] SO_ItemPrefabs prefabLibrary;
	
	[Header("Customise Grid")]
	public int GridLengthX;
	public int GridLengthZ;
	
	[Space(5)]
	
	public float CellSpacing;
	
	[Space(5)]
	
	public float CellOffsetX;
	public float CellOffsetZ;

	[Space(5)]

	[HideInInspector] public bool GenerationComplete;
	[HideInInspector] public bool CellsSpawned;
	
	[Space(10)]
	
	[HideInInspector] public bool EnableEditorTools;
	[HideInInspector] public bool IsGridActive;
	[HideInInspector] public bool ShowGrid;
	[HideInInspector] public bool GridPreviewToggled;
	[HideInInspector] public bool CellOccupantsToggled;
	
	void Awake()
	{
		EnableEditorTools = false;
		IsGridActive = true;
		
		GenerationComplete = false;
		CellsSpawned = false;
	}
	
	void Start()
	{
		if(GridPreviewToggled) TogglePreviewGrid();
		
		GenerateGrid();
        LoadGridData();
		
		SpawnActiveGrid();
	}
	
	#region Grid Control
	
	public void GenerateGrid()
	{
		GeneratedData.CellPositions = new Vector3[GridLengthX, GridLengthZ];
		GeneratedData.ActiveCells = new bool[GridLengthX, GridLengthZ];
		
		GeneratedData.CellOccupantPrefabs = new GameObject[GridLengthX, GridLengthZ];
        GeneratedData.CellOccupantsActiveOnSpawn = new bool[GridLengthX, GridLengthZ];


        GeneratedData.CellOccupantPositions = new Vector3[GridLengthX, GridLengthZ];
		GeneratedData.CellOccupantEulerAngles = new Vector3[GridLengthX, GridLengthZ];
		
		// Make sure no left over cells are in the scene
		int amountOfChildren = transform.childCount;
		if(amountOfChildren > 0)
		{
			for(int i = 0; i < amountOfChildren; i++)
			{
				DestroyImmediate(transform.GetChild(0).gameObject, false);
                GridPreviewToggled = false;
                GeneratedData.SpawnedCells = null;
            }
		}
		
		// Generate layout
		Vector3 generatedCellPosition;
		for(int axisX = 0; axisX < GridLengthX; axisX++)
		{
			if(axisX == 0)
			{
				generatedCellPosition = transform.position;
				generatedCellPosition.x += CellScale.x / 2;
				generatedCellPosition.z += CellScale.z / 2;
				GeneratedData.CellPositions[0, 0] = generatedCellPosition;
			}
			else
			{
				generatedCellPosition = GeneratedData.CellPositions[axisX - 1, 0];
				generatedCellPosition.x += CellScale.x + CellSpacing + CellOffsetX;
				GeneratedData.CellPositions[axisX, 0] = generatedCellPosition;
			}
			
			for(int axisZ = 1; axisZ < GridLengthZ; axisZ++)
			{
				generatedCellPosition = GeneratedData.CellPositions[axisX, axisZ - 1];
				generatedCellPosition.z += CellScale.z + CellSpacing + CellOffsetZ;
				GeneratedData.CellPositions[axisX, axisZ] = generatedCellPosition;
			}
		}
		
		GenerationComplete = true;
	}
	
	public void SpawnGrid()
	{
		GeneratedData.SpawnedCells = new GridCell[GridLengthX, GridLengthZ];
		
		for(int axisX = 0; axisX < GridLengthX; axisX++)
		{
			for(int axisZ = 0; axisZ < GridLengthZ; axisZ++)
			{
                GridCell cellInstance = Instantiate(GridCellPrefab, GeneratedData.CellPositions[axisX, axisZ], Quaternion.identity).GetComponent<GridCell>();
				cellInstance.transform.parent = transform;
				
				// Modify cell script here
				cellInstance.ConnectedGrid = this;
				cellInstance.CellActive = GeneratedData.ActiveCells[axisX, axisZ];
				cellInstance.GridIndex = new Vector2(axisX, axisZ);
				cellInstance.OccupantPrefab = GeneratedData.CellOccupantPrefabs[axisX, axisZ];
				cellInstance.OccupantPosition = GeneratedData.CellOccupantPositions[axisX, axisZ];
				cellInstance.OccupantEulerAngles = GeneratedData.CellOccupantEulerAngles[axisX, axisZ];
				
				GeneratedData.SpawnedCells[axisX, axisZ] = cellInstance;
			}
		}
		
		CellsSpawned = true;
	}
	
	public void SpawnActiveGrid()
	{
		GeneratedData.SpawnedCells = new GridCell[GridLengthX, GridLengthZ];
		
		for(int axisX = 0; axisX < GridLengthX; axisX++)
		{
			for(int axisZ = 0; axisZ < GridLengthZ; axisZ++)
			{
				if(GeneratedData.ActiveCells[axisX, axisZ])
				{
                    GridCell cellInstance = Instantiate(GridCellPrefab, GeneratedData.CellPositions[axisX, axisZ], Quaternion.identity).GetComponent<GridCell>();
					cellInstance.transform.parent = transform;
					
					// Modify cell script here
					cellInstance.ConnectedGrid = this;
					cellInstance.CellActive = true;
					cellInstance.GridIndex = new Vector2(axisX, axisZ);
					cellInstance.OccupantPrefab = GeneratedData.CellOccupantPrefabs[axisX, axisZ];
                    cellInstance.OccupantPosition = GeneratedData.CellOccupantPositions[axisX, axisZ];
                    cellInstance.OccupantEulerAngles = GeneratedData.CellOccupantEulerAngles[axisX, axisZ];

                    GeneratedData.SpawnedCells[axisX, axisZ] = cellInstance;
				}
			}
		}
		
		if(SpawnCellOccupants != null)
			SpawnCellOccupants();
		
		CellsSpawned = true;

		OnGridSpawned?.Invoke();
    }
	
	public void TogglePreviewGrid()
	{
		if(IsGridActive) return;
		
		GridPreviewToggled = !GridPreviewToggled;
		
		if(GridPreviewToggled)
		{
			SpawnGrid();
		}
		else
		{
			if(CellOccupantsToggled) ToggleCellOccupants();
			
			foreach(var cell in GeneratedData.SpawnedCells)
			{
				DestroyImmediate(cell.gameObject, false);
				GeneratedData.SpawnedCells = null;
			}
		}
	}
	
	public void ToggleCellOccupants()
	{
		CellOccupantsToggled = !CellOccupantsToggled;
		
		if(CellOccupantsToggled)
		{
			GeneratedData.CellActiveOccupants = new GameObject[GridLengthX, GridLengthZ];
			
			foreach(var cell in GeneratedData.SpawnedCells)
			{
				cell.SpawnOccupant();
				
				if(cell.ActiveOccupant != null)
				{
					Vector2 cellIndex = cell.GridIndex;
					GeneratedData.CellActiveOccupants[(int) cellIndex.x, (int) cellIndex.y] = cell.ActiveOccupant;
				}
			}
		}
		else
		{
			foreach(var cell in GeneratedData.SpawnedCells)
			{
				if(cell != null && cell.ActiveOccupant != null)
				{
					DestroyImmediate(cell.ActiveOccupant, false);
				}
				
				GeneratedData.CellActiveOccupants = null;
			}
		}
	}
	
	public void UpdateGridFromCellData()
	{
		for(int x = 0; x < GridLengthX; x++)
		{
			for(int z = 0; z < GridLengthZ; z++)
			{
                GeneratedData.ActiveCells[x, z] = GeneratedData.SpawnedCells[x, z].CellActive;
				GeneratedData.CellOccupantPrefabs[x, z] = GeneratedData.SpawnedCells[x, z].OccupantPrefab;
				GeneratedData.CellOccupantPositions[x, z] = GeneratedData.SpawnedCells[x, z].OccupantPosition;
				GeneratedData.CellOccupantEulerAngles[x, z] = GeneratedData.SpawnedCells[x, z].OccupantEulerAngles;
				GeneratedData.CellOccupantsActiveOnSpawn[x, z] = GeneratedData.SpawnedCells[x, z].OccupantActiveOnSpawn;


                GeneratedData.SpawnedCells[x, z].UpdateOccupantTransform();
			}
		}
	}
	
	#endregion
	
	#region Saving & Loading
	
	public bool SaveGridData()
	{
		if (!GenerationComplete)
		{
            Debug.LogWarning("Grid generation is not complete. Cannot save data.");
            return false;
        }

		if (GridData == null)
		{
            Debug.LogWarning("No grid data assigned. Cannot save data.");
			return false;
        }
		
		// Store data
		GridData.LengthX = GridLengthX;
		GridData.LengthZ = GridLengthZ;
		
		GridData.Spacing = CellSpacing;
		
		GridData.OffsetX = CellOffsetX;
		GridData.OffsetZ = CellOffsetZ;

        GridData.IsCellActive = new List<bool>();
        GridData.OccupantPrefabs = new List<GameObject>();
        GridData.OccupantPositions = new List<Vector3>();
        GridData.OccupantEulerAngles = new List<Vector3>();

        for (int x = 0; x < GridLengthX; x++)
		{
			for(int z = 0; z < GridLengthZ; z++)
			{
				GridData.IsCellActive.Add(GeneratedData.ActiveCells[x, z]);

                GridData.OccupantPrefabs.Add(GeneratedData.CellOccupantPrefabs[x, z]);
                GridData.OccupantPositions.Add(GeneratedData.CellOccupantPositions[x, z]);
				GridData.OccupantEulerAngles.Add(GeneratedData.CellOccupantEulerAngles[x, z]);
				GridData.OccupantActiveOnSpawn.Add(GeneratedData.CellOccupantsActiveOnSpawn[x, z]);
            }
		}

		EditorUtility.SetDirty(GridData);

        Debug.Log("Grid data saved");
        return true;
	}
	
	public bool LoadGridData()
	{
		if(GridData == null)
		{
			Debug.Log("No grid data assigned. Cannot load data.");
            return false;
        }
		
		GridLengthX = GridData.LengthX;
		GridLengthZ = GridData.LengthZ;
		
		CellSpacing = GridData.Spacing;
		
		CellOffsetX = GridData.OffsetX;
		CellOffsetZ = GridData.OffsetZ;

        GenerateGrid();

        int dataIndex = 0;
        for (int x = 0; x < GridLengthX; x++)
		{
			for(int z = 0; z < GridLengthZ; z++)
			{
				GeneratedData.ActiveCells[x, z] = GridData.IsCellActive[dataIndex];

				GeneratedData.CellOccupantPrefabs[x, z] = GridData.OccupantPrefabs[dataIndex];
                GeneratedData.CellOccupantPositions[x, z] = GridData.OccupantPositions[dataIndex];
				GeneratedData.CellOccupantEulerAngles[x, z] = GridData.OccupantEulerAngles[dataIndex];
                GeneratedData.CellOccupantsActiveOnSpawn[x, z] = GridData.OccupantActiveOnSpawn[dataIndex];

                dataIndex++;
            }
		}

        Debug.Log("Grid data loaded");
        return true;
	}
	
	#endregion
	
#if UNITY_EDITOR
	
	void OnDrawGizmos()
	{
		if(EnableEditorTools && ShowGrid && GeneratedData.CellPositions != null && GeneratedData.ActiveCells != null)
		{
			if(GeneratedData.CellPositions.GetLength(0) == GridLengthX && GeneratedData.CellPositions.GetLength(1) == GridLengthZ)
			{
				for(int x = 0; x < GridLengthX; x++)
				{
					for(int z = 0; z < GridLengthZ; z++)
					{
						if(GeneratedData.ActiveCells.GetLength(0) == GridLengthX && GeneratedData.ActiveCells.GetLength(1) == GridLengthZ)
						{
							Gizmos.color = GeneratedData.ActiveCells[x, z]? Color.green : Color.red;
						}
						else Gizmos.color = Color.yellow;
						Gizmos.DrawWireCube(GeneratedData.CellPositions[x, z], CellScale);
					}
				}
			}
		}
	}
	
#endif
}
