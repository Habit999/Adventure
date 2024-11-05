using UnityEngine;
using UnityEditor;

public class GridEditor : EditorWindow
{
	[HideInInspector] public CustomGrid _grid;
	
	//Cell Data
	public bool[,] _tempCullingData;
	public GameObject[,] _loadedOccupantData;
	GameObject[,] tempOccupantData;
	
	public Vector3[,] _loadedCellPositionOffset;
	Vector3[,] tempCellPositionOffset;
	
	//Occupant Data
	public Vector3[,] _loadedOccupantPositionOffset;
	Vector3[,] tempOccupantPositionOffset;
	public Vector3[,] _loadedOccupantRotationOffset;
	Vector3[,] tempOccupantRotationOffset;
	
	bool isNewGrid = true;
	
	GridData localData = new GridData();
	
	Vector2 scrollPosition;
	
	[MenuItem("Tools/GridEditor")]
	public static void ShowWindow() // Runs when opening window from tool bar
	{
		GetWindow(typeof(GridEditor), false, "GridEditor", true);
	}
	
	void OnGUI()
	{
		GUILayout.Label("Grid Editor Tools", EditorStyles.boldLabel);
		
		GUILayout.Space(10);
		
		// "varible" = EditorGUILayout.(TextField, IntField, Slider, FloatField, ObjectField)
		
		CustomGrid newGrid = (CustomGrid) EditorGUILayout.ObjectField("Target Grid", _grid, typeof(CustomGrid), true);
		if(newGrid != _grid || _grid == null) isNewGrid = true;
		
		GUILayout.Space(10);
		
		if(_grid != null)
		{
			if(GUILayout.Button("Save Grid Data"))
			{
				_grid._gridCulling = _tempCullingData;
				_loadedOccupantData = tempOccupantData;
				_grid._gridCellOccupants = _loadedOccupantData;
				
				_loadedCellPositionOffset = tempCellPositionOffset;
				_loadedOccupantPositionOffset = tempOccupantPositionOffset;
				_loadedOccupantRotationOffset = tempOccupantRotationOffset;
				_grid._cellPositionOffsetData = _loadedCellPositionOffset;
				_grid._occupantPositionOffsetData = _loadedOccupantPositionOffset;
				_grid._occupantRotationOffsetData = _loadedOccupantRotationOffset;
				
				_grid.SaveGridData();
				_grid.GenerateGrid();
				
				Debug.Log($"CustomGrid {'"' + _grid.gameObject.name + '"'} >> Grid Data Saved");
			}
			
			GUILayout.Space(10);
			
			if(GUILayout.Button("Set All True"))
			{
				for(int x = 0; x < _grid._gridLengthX; x++)
				{
					for(int y = 0; y < _grid._gridLengthZ; y++)
					{
						_tempCullingData[x, y] = true;
					}
				}
			}
			
			if(GUILayout.Button("Set All False"))
			{
				for(int x = 0; x < _grid._gridLengthX; x++)
				{
					for(int y = 0; y < _grid._gridLengthZ; y++)
					{
						_tempCullingData[x, y] = false;
					}
				}
			}
			
			GUILayout.Space(25);
			
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			try
			{
				if(isNewGrid)
				{
					_tempCullingData = new bool[_grid._gridLengthX, _grid._gridLengthZ];
					tempOccupantData = new GameObject[_grid._gridLengthX, _grid._gridLengthZ];
					tempCellPositionOffset = new Vector3[_grid._gridLengthX, _grid._gridLengthZ];
					tempOccupantPositionOffset = new Vector3[_grid._gridLengthX, _grid._gridLengthZ];
					tempOccupantRotationOffset = new Vector3[_grid._gridLengthX, _grid._gridLengthZ];
					
					for(int x = 0; x < _grid._gridLengthX; x++)
					{
						_tempCullingData[x, 0] = true;
						for(int z = 1; z < _grid._gridLengthZ; z++)
						{
							_tempCullingData[x, z] = true;
						}
					}
					
					if(_grid._initialGenerationComplete)
					{
						_tempCullingData = _grid._gridCulling;
					}
					
					isNewGrid = false;
				}
				
				#region Dynamic Grid Cell Layout
				
				GUILayout.BeginVertical();
				int cellIndex = 1;
				for(int x = 0; x < _grid._gridLengthX; x++)
				{
					GUILayout.Space(10);
					GUILayout.BeginHorizontal();
					for(int y = 0; y < _grid._gridLengthZ; y++)
					{
						GUILayout.Space(10);
						GUILayout.BeginVertical();
						
						GUILayout.Label($"Cell {cellIndex}", EditorStyles.boldLabel);
						
						// Cell Culling
						if(GUILayout.Button($"Active: {_tempCullingData[x, y].ToString()}"))
						{
							_tempCullingData[x, y] = !_tempCullingData[x, y];
						}
						
						if(GUILayout.Button("Reset Cell"))
						{
							_tempCullingData[x, y] = false;
							_loadedOccupantData[x, y] = null;
							_loadedCellPositionOffset[x, y] = Vector3.zero;
							_loadedOccupantPositionOffset[x, y] = Vector3.zero;
							_loadedOccupantRotationOffset[x, y] = Vector3.zero;
						}
						
						GUILayout.Space(2);
						
						// Cell Occupant
						if(tempOccupantData[x, y] != _loadedOccupantData[x, y]) tempOccupantData[x, y] = _loadedOccupantData[x, y];
						tempOccupantData[x, y] = (GameObject) EditorGUILayout.ObjectField("Cell Occupant", tempOccupantData[x, y], typeof(GameObject), false);
						_loadedOccupantData[x, y] = tempOccupantData[x, y];
						
						if(tempCellPositionOffset[x, y] != _loadedCellPositionOffset[x, y]) tempCellPositionOffset[x, y] = _loadedCellPositionOffset[x, y];
						tempCellPositionOffset[x, y] = (Vector3) EditorGUILayout.Vector3Field("Cell Position Offset", tempCellPositionOffset[x, y]);
						_loadedCellPositionOffset[x, y] = tempCellPositionOffset[x, y];
						
						if(tempOccupantPositionOffset[x, y] != _loadedOccupantPositionOffset[x, y]) tempOccupantPositionOffset[x, y] = _loadedOccupantPositionOffset[x, y];
						tempOccupantPositionOffset[x, y] = (Vector3) EditorGUILayout.Vector3Field("Occupant Position Offset", tempOccupantPositionOffset[x, y]);
						_loadedOccupantPositionOffset[x, y] = tempOccupantPositionOffset[x, y];
						
						if(tempOccupantRotationOffset[x, y] != _loadedOccupantRotationOffset[x, y]) tempOccupantRotationOffset[x, y] = _loadedOccupantRotationOffset[x, y];
						tempOccupantRotationOffset[x, y] = (Vector3) EditorGUILayout.Vector3Field("Occupant Rotation Offset", tempOccupantRotationOffset[x, y]);
						_loadedOccupantRotationOffset[x, y] = tempOccupantRotationOffset[x, y];
						
						cellIndex++;
						
						GUILayout.EndVertical();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
				
				#endregion
			}
			catch(System.Exception e)
			{
				Debug.Log("GridEditor ERROR >> " + e.ToString());
			}
			
			GUILayout.EndScrollView();
		}
	}
}
