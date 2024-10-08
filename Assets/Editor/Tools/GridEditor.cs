using UnityEngine;
using UnityEditor;

public class GridEditor : EditorWindow
{
	[HideInInspector] public CustomGrid _grid;
	
	public bool[,] _tempCullingData;
	public GameObject[,] _cellResidentData;
	
	bool isNewGrid = true;
	
	GridData localData = new GridData();
	
	[MenuItem("Tools/GridEditor")]
	public static void ShowWindow() // Runs when opening window
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
				_grid.SaveGridData();
				
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
			
			try
			{
				if(isNewGrid)
				{
					_tempCullingData = new bool[_grid._gridLengthX, _grid._gridLengthZ];
					_cellResidentData = new GameObject[_grid._gridLengthX, _grid._gridLengthZ];
					
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
				
				GUILayout.BeginVertical();
				for(int x = 0; x < _grid._gridLengthX; x++)
				{
					GUILayout.BeginHorizontal();
					for(int y = 0; y < _grid._gridLengthZ; y++)
					{
						GUILayout.BeginVertical();
						if(GUILayout.Button(_tempCullingData[x, y].ToString()))
						{
							_tempCullingData[x, y] = !_tempCullingData[x, y];
						}
						
						_cellResidentData[x, y] = (GameObject) EditorGUILayout.ObjectField("Spawn Object", _cellResidentData[x, y], typeof(GameObject), false);
						
						GUILayout.EndVertical();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
			}
			catch(System.Exception e)
			{
				Debug.Log("GridEditor ERROR >> " + e.ToString());
			}
		}
	}
}
