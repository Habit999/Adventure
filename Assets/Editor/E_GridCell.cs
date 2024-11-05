using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(GridCell))]
public class E_GridCell : Editor
{	
	GridCell targetCell;
	
	bool tempActiveCell;
	
	public override void OnInspectorGUI()
	{
		targetCell = (GridCell) EditorGUILayout.ObjectField("Target Cell", targetCell, typeof(GridCell), true);
		
		if(targetCell != null && targetCell._connectedGrid._enableEditorTools && targetCell._connectedGrid._activeGridPreview)
		{
			if(!targetCell._connectedGrid._initialGenerationComplete) targetCell._connectedGrid.GenerateGrid();
			
			tempActiveCell = targetCell._connectedGrid._gridCulling[(int) targetCell._cellIndex.x, (int) targetCell._cellIndex.y];
			tempActiveCell = EditorGUILayout.Toggle($"Cell Active", tempActiveCell);
			targetCell._connectedGrid._gridCulling[(int) targetCell._cellIndex.x, (int) targetCell._cellIndex.y] = tempActiveCell;
			
			if(GUILayout.Button("Update Cells"))
			{
				targetCell._connectedGrid.UpdateGridDataFromInstances();
			}
		}
		
		GUILayout.Space(10);
		
		base.OnInspectorGUI();
	}
}
