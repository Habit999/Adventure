using UnityEngine;
using UnityEditor;

public class GridEditor : EditorWindow
{
	[HideInInspector] public CustomGrid _grid;
	
	[MenuItem("Tools/GridEditor")]
	public static void ShowWindow() // Runs when opening window
	{
		GetWindow(typeof(GridEditor), true, "GridEditor", true);
	}
	
	void OnGUI()
	{
		GUILayout.Label("Grid Editor Tools", EditorStyles.boldLabel);
		
		GUILayout.Space(10);
		
		// "varible" = EditorGUILayout.(TextField, IntField, Slider, FloatField, ObjectField)
		
		_grid = (CustomGrid) EditorGUILayout.ObjectField("Target Grid", _grid, typeof(CustomGrid), true);
		
		GUILayout.Space(10);
		
		if(GUILayout.Button("Save & Generate Grid"))
		{
			Debug.Log("Saving & Generating Grid");
		}
		
		if(_grid != null)
		{
			try
			{
				_grid.GenerateGrid();
				GUILayout.BeginVertical();
				for(int x = 0; x < _grid._gridLengthX; x++)
				{
					GUILayout.BeginHorizontal();
					for(int y = 0; y < _grid._gridLengthZ; y++)
					{
						if(GUILayout.Button(_grid.gridData._gridCulling[x, y].ToString()))
						{
							_grid.gridData._gridCulling[x, y] = !_grid.gridData._gridCulling[x, y];
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
