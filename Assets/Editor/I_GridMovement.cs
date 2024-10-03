using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomGrid))]
public class I_GridMovement : Editor
{
	CustomGrid targetGrid;
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		EditorGUILayout.Space(20);
		
		targetGrid = (CustomGrid) EditorGUILayout.ObjectField("Target Grid", targetGrid, typeof(CustomGrid), true);
		
		EditorGUILayout.Space(10);
		
		/*if(GUILayout.Button("Save Grid") && grid != null)
		{
			grid.SaveGridData(grid.gridData);
		}*/
		
		if(GUILayout.Button("Generate Grid") && targetGrid != null)
		{
			targetGrid.GenerateGrid();
		}
		
		EditorGUILayout.Space(5);
		
		if(GUILayout.Button("Toggle Grid Visibility") && targetGrid != null)
		{
			targetGrid._isGridVisible = !targetGrid._isGridVisible;
		}
		
		if(GUILayout.Button("Open GridEditor"))
		{
			GridEditor gridEditor = (GridEditor) EditorWindow.GetWindow(typeof(GridEditor), false, "GridEditor", true);
			gridEditor._grid = targetGrid;
			gridEditor.Show();
		}
		
		/*if(grid != null)
		{
			MonoBehaviour monBev = (MonoBehaviour)grid;
			MovementGrid moveGrid = monBev.GetComponent<MovementGrid>();
		}*/
	}
}
