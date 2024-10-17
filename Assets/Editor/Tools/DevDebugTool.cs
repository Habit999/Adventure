using UnityEngine;
using UnityEditor;

public class DevDebugTool : EditorWindow
{
	CustomGrid grid;
	
	[MenuItem("Tools/DevDebugTool")]
	public static void ShowWindow()
	{
		GetWindow(typeof(DevDebugTool), false, "DevDebugTool", true);
	}
	
	void OnGUI()
	{
		GUILayout.Label("Dev Tools");
		
		GUILayout.Space(5);
		
		if(GUILayout.Button("Print Prefab Library"))
		{
			foreach(GameObject prefab in PrefabLibrary.PrefabID.Values)
			{
				Debug.Log(prefab);
			}
		}
		
		GUILayout.Space(10);
		
		grid = (CustomGrid) EditorGUILayout.ObjectField("Target Grid", grid, typeof(CustomGrid), true);
		if(GUILayout.Button("Print Grid Occupant ID Data"))
		{
			if(grid.LoadGridData())
			{
				foreach(int id in grid.gridData.cellOccupantIDData)
				{
					if(id != 0) Debug.Log(id);
				}
			}
			else Debug.Log("Dev Debug Error >> Couldn't Load Grid Data");
		}
	}
}
