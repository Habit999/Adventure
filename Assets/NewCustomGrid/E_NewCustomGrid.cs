using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

[CustomEditor(typeof(NewCustomGrid))]
public class E_NewCustomGrid : Editor
{
	NewCustomGrid targetGrid;
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		EditorGUILayout.Space(5);
		
		targetGrid = (NewCustomGrid) EditorGUILayout.ObjectField("Target Grid", targetGrid, typeof(NewCustomGrid), true);
		
		if(targetGrid != null)
		{
			string editorModeState = targetGrid._enableEditorTools ? "Disable" : "Enable";
			if(GUILayout.Button($"{editorModeState} Editor Tools"))
			{
				// Disable & Destroy Preview Grid
				if(targetGrid._gridPreviewToggled) targetGrid.TogglePreviewGrid();
				
				targetGrid._enableEditorTools = !targetGrid._enableEditorTools;
			}
			
			EditorGUILayout.Space(10);
		
			if(targetGrid._enableEditorTools)
			{
				if(GUILayout.Button("Generate New Grid"))
				{
					targetGrid.GenerateGrid();
				}
				
				if(targetGrid.GeneratedData._spawnedCells != null)
				{
					if(GUILayout.Button("Update Grid From Cell Data"))
					{
						targetGrid.UpdateGridFromCellData();
					}
				}
				
				EditorGUILayout.Space(5);
				
				if(targetGrid._generationComplete)
				{
					string gizmosActive = targetGrid._showGrid ? "Active" : "Inactive";
					GUILayout.Label($"Grid Gizmos : {gizmosActive}");
									
					if(GUILayout.Button("Toggle Grid Gizmos"))
					{
						targetGrid._showGrid = !targetGrid._showGrid;
					}
					
					string previewActive = targetGrid._gridPreviewToggled ? "Active" : "Inactive";
					GUILayout.Label($"Grid Preview : {previewActive}");
					
					if(GUILayout.Button("Toggle Preview Grid"))
					{
						targetGrid.TogglePreviewGrid();
					}
					
					EditorGUILayout.Space(20);
					
					if(GUILayout.Button("Save Grid Data"))
					{
						targetGrid.SaveGridData();
					}
					
					if(File.Exists(targetGrid.GridDataPath))
					{
						if(GUILayout.Button("Load Grid Data"))
						{
							targetGrid.LoadGridData();
						}
					}
				}
			}
		}
	}
}
