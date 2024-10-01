using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomGrid))]
public class I_GridMovement : Editor
{
	CustomGrid grid;
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		EditorGUILayout.Space(20);
		
		grid = (CustomGrid) EditorGUILayout.ObjectField("Target Grid", grid, typeof(CustomGrid), true);
		
		EditorGUILayout.Space(10);
		
		if(GUILayout.Button("Save Grid") && grid != null)
		{
			grid.SaveGridData();
		}
		
		EditorGUILayout.Space(5);
		
		if(GUILayout.Button("Toggle Grid Visibility") && grid != null)
		{
			grid._isGridVisible = !grid._isGridVisible;
		}
		
		if(GUILayout.Button("Open GridEditor"))
		{
			GridEditor gridEditor = (GridEditor) EditorWindow.GetWindow(typeof(GridEditor), true, "GridEditor", true);
			gridEditor._grid = grid;
			gridEditor.Show();
		}
		
		/*if(grid != null)
		{
			MonoBehaviour monBev = (MonoBehaviour)grid;
			MovementGrid moveGrid = monBev.GetComponent<MovementGrid>();
		}*/
	}
}
