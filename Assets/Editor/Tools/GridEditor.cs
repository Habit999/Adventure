using UnityEngine;
using UnityEditor;

public class GridEditor : EditorWindow
{
	[HideInInspector] public CustomGrid _grid;
	
	public bool[,] _cellCullingMap;
	
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
				localData._gridCulling = _cellCullingMap;
				_grid.SaveGridData(localData);
				
				Debug.Log($"CustomGrid {'"' + _grid.gameObject.name + '"'} >> Grid Data Saved");
			}
			
			try
			{
				if(isNewGrid)
				{
					_cellCullingMap = new bool[_grid._gridLengthX, _grid._gridLengthZ];
					for(int x = 0; x < _grid._gridLengthX; x++)
					{
						_cellCullingMap[x, 0] = true;
						for(int z = 1; z < _grid._gridLengthZ; z++)
						{
							_cellCullingMap[x, z] = true;
						}
					}
					isNewGrid = false;
				}
				
				GUILayout.BeginVertical();
				for(int x = 0; x < _grid._gridLengthX; x++)
				{
					GUILayout.BeginHorizontal();
					for(int y = 0; y < _grid._gridLengthZ; y++)
					{
						if(GUILayout.Button(_cellCullingMap[x, y].ToString()))
						{
							_cellCullingMap[x, y] = !_cellCullingMap[x, y];
						}
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
