using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomGrid))]
public class I_GridMovement : Editor
{
	CustomGrid targetGrid;
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		targetGrid = (CustomGrid) EditorGUILayout.ObjectField("Target Grid", targetGrid, typeof(CustomGrid), true);
		
		EditorGUILayout.Space(10);
		
		if(targetGrid != null && targetGrid._enableEditorTools)
		{
			try
			{
				if(GUILayout.Button("Generate Grid") && targetGrid != null)
				{
					targetGrid.GenerateGrid();
				}
				
				EditorGUILayout.Space(5);
				
				if(GUILayout.Button("Toggle Grid Visibility") && targetGrid != null)
				{
					targetGrid._isGridVisible = !targetGrid._isGridVisible;
				}
				
				if(GUILayout.Button("Open GridEditor") && targetGrid != null)
				{
					GridEditor gridEditor = (GridEditor) EditorWindow.GetWindow(typeof(GridEditor), false, "GridEditor", true);
					gridEditor._grid = targetGrid;
					gridEditor.Show();
				}
			}
			catch(System.Exception e)
			{
				Debug.Log("No Target Grid Assigned To Custom Grid\r\n" + e.ToString());
			}
		}
	}
}
