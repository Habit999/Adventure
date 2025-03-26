#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomGrid))]
public class E_CustomGrid : Editor
{
	CustomGrid targetGrid;
	
	void Awake()
	{
		targetGrid = (CustomGrid)target;
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		if(targetGrid != null)
		{
			string editorModeState = targetGrid.EnableEditorTools ? "Disable" : "Enable";
			if(GUILayout.Button($"{editorModeState} Editor Tools"))
			{
				// Disable & Destroy Preview Grid
				if(targetGrid.GridPreviewToggled) targetGrid.TogglePreviewGrid();
				
				targetGrid.EnableEditorTools = !targetGrid.EnableEditorTools;
			}
			
			EditorGUILayout.Space(10);
		
			if(targetGrid.EnableEditorTools)
			{
				if(GUILayout.Button("Generate New Grid"))
				{
					targetGrid.GenerateGrid();
				}
				
				if(targetGrid.GeneratedData.SpawnedCells != null)
				{
					if(GUILayout.Button("Update Grid From Cell Data"))
					{
						targetGrid.UpdateGridFromCellData();
					}
				}
				
				EditorGUILayout.Space(5);
				
				if(targetGrid.GenerationComplete)
				{
					string gizmosActive = targetGrid.ShowGrid ? "Active" : "Inactive";
					GUILayout.Label($"Grid Gizmos : {gizmosActive}");
									
					if(GUILayout.Button("Toggle Grid Gizmos"))
					{
						targetGrid.ShowGrid = !targetGrid.ShowGrid;
					}
					
					string previewActive = targetGrid.GridPreviewToggled ? "Active" : "Inactive";
					GUILayout.Label($"Grid Preview : {previewActive}");
					
					if(GUILayout.Button("Toggle Preview Grid"))
					{
						targetGrid.TogglePreviewGrid();
					}
					
					if(targetGrid.GridPreviewToggled)
					{
						string occupantsActive = targetGrid.CellOccupantsToggled ? "Active" : "Inactive";
						GUILayout.Label($"Cell Occupants : {occupantsActive}");
						
						if(GUILayout.Button("Toggle Cell Occupants"))
						{
							targetGrid.ToggleCellOccupants();
						}
					}
					
					EditorGUILayout.Space(20);
					
					if(targetGrid.GridData != null)
					{
                        if (GUILayout.Button("Save Grid Data"))
                        {
                            targetGrid.SaveGridData();
                        }

						EditorGUILayout.Space(10);

                        if (GUILayout.Button("Load Grid Data"))
						{
							targetGrid.GenerateGrid();
							targetGrid.LoadGridData();
						}
					}
				}
			}
		}
	}
}
#endif
