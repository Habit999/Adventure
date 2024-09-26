using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovementGrid))]
public class Grideditor : Editor
{
	MovementGrid grid;
	
	bool isGridVisible = false;
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		EditorGUILayout.Space(20);
		
		grid = EditorGUILayout.ObjectField("TargetGrid", grid, typeof(MovementGrid), true) as MovementGrid;
		
		/*if(grid != null)
		{
			if(GUILayout.Button("View Grid"))
			{
				isGridVisible = !isGridVisible;
				Debug.Log($"View Grid = {isGridVisible}");
			}
			
			if(isGridVisible)
			{
				GUILayout.BeginVertical();
				for(int x = 0; x < grid._gridLengthX; x++)
				{
					if(GUILayout.Button(grid._gridCells[x, 0]._inUse.ToString()))
					{
						grid._gridCells[x, 0]._inUse = !grid._gridCells[x, 0]._inUse;
					}
					
					GUILayout.BeginHorizontal();
					for(int y = 1; y < grid._gridLengthZ; y++)
					{
						if(GUILayout.Button(grid._gridCells[x, y]._inUse.ToString()))
						{
							grid._gridCells[x, y]._inUse = !grid._gridCells[x, y]._inUse;
						}
					}
					GUILayout.EndHorizontal();
					
				}
				GUILayout.EndVertical();
			}
		}*/
		
		/*if(grid != null)
		{
			MonoBehaviour monBev = (MonoBehaviour)grid;
			MovementGrid moveGrid = monBev.GetComponent<MovementGrid>();
		}*/
	}
}
