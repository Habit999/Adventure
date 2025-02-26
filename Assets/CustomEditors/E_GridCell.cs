using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridCell)), CanEditMultipleObjects]
public class E_GridCell : Editor
{
	GridCell targetCell;
	
	void OnEnable()
	{
		targetCell = (GridCell)target;
	}
	
	public override void OnInspectorGUI()
	{
		if(targetCell.ConnectedGrid != null)
		{
			if(GUILayout.Button("Update Grid"))
			{
				targetCell.ConnectedGrid.UpdateGridFromCellData();
			}
		}
		
		EditorGUILayout.Space(5);
		
		base.OnInspectorGUI();
	}
}
