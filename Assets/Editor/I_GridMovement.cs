using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomGrid))]
public class I_GridMovement : Editor
{
	CustomGrid targetGrid;
	
	GameObject[,] tempPreviewOccupants;
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		targetGrid = (CustomGrid) EditorGUILayout.ObjectField("Target Grid", targetGrid, typeof(CustomGrid), true);
		
		EditorGUILayout.Space(10);
		
		if(targetGrid != null && targetGrid._enableEditorTools && targetGrid._initialGenerationComplete)
		{
			try
			{
				string gridVisible = targetGrid._isGridVisible? "Active" : "InActive";
				string gridPreviewActive = targetGrid._activeGridPreview? "Active" : "InActive";
				GUILayout.Label($"Grid Gizmos : {gridVisible}");
				GUILayout.Label($"Grid Cell Preview : {gridPreviewActive}");
				
				EditorGUILayout.Space(5);
				
				if(GUILayout.Button("Generate Grid"))
				{
					targetGrid.GenerateGrid();
					
					if(tempPreviewOccupants != null)
					{
						foreach(GameObject previewOccupant in tempPreviewOccupants)
						{
							if(previewOccupant != null && previewOccupant.activeSelf)
							{
								DestroyImmediate(previewOccupant);
								continue;
							}
						}
					}
					tempPreviewOccupants = new GameObject[targetGrid._gridLengthX, targetGrid._gridLengthZ];
				}
				
				EditorGUILayout.Space(5);
				
				if(GUILayout.Button("Toggle Grid Visibility"))
				{
					targetGrid._isGridVisible = !targetGrid._isGridVisible;
				}
				
				if(GUILayout.Button("Toggle Grid Preview"))
				{
					targetGrid._activeGridPreview = !targetGrid._activeGridPreview;
				}
				
				if(GUILayout.Button("Open GridEditor"))
				{
					GridEditor gridEditor = (GridEditor) EditorWindow.GetWindow(typeof(GridEditor), false, "GridEditor", true);
					gridEditor._grid = targetGrid;
					if(targetGrid.LoadGridData())
					{
						gridEditor._tempCullingData = targetGrid._gridCulling;
						gridEditor._tempOccupantData = targetGrid._gridCellOccupants;
					}
					else
					{
						gridEditor._tempCullingData = new bool[targetGrid._gridLengthX, targetGrid._gridLengthZ];
						gridEditor._tempOccupantData = new GameObject[targetGrid._gridLengthX, targetGrid._gridLengthZ];
					}
					
					gridEditor.Show();
				}
				
				
				// Preview Grid
				
				if(targetGrid._initialGenerationComplete && targetGrid._activeGridPreview)
				{
					for(int x = 0; x < targetGrid._gridLengthX; x++)
					{
						for(int y = 0; y < targetGrid._gridLengthZ; y++)
						{
							if(targetGrid._gridCellOccupants[x, y] != null && tempPreviewOccupants[x, y] == null)
							{
								Vector3 occupantPosition = targetGrid._cellPositions[x, y];
								occupantPosition.z = occupantPosition.y;
								occupantPosition.y = 0;
								tempPreviewOccupants[x, y] = Instantiate(targetGrid._gridCellOccupants[x, y], occupantPosition, Quaternion.identity, targetGrid.transform);
							}
							else if(targetGrid._gridCellOccupants[x, y] == null && tempPreviewOccupants[x, y] != null)
							{
								DestroyImmediate(tempPreviewOccupants[x, y]);
							}
						}
					}
				}
				else if(targetGrid._initialGenerationComplete && !targetGrid._activeGridPreview)
				{
					foreach(GameObject occupant in tempPreviewOccupants)
					{
						if(occupant != null && occupant.activeSelf)
						{
							DestroyImmediate(occupant);
							continue;
						}
					}
				}
			}
			catch(System.Exception e)
			{
				Debug.Log("No Target Grid Assigned OR No Custom Grid Save Data\r\nTry Generate Grid OR Set Up With GridEditor" + e.ToString());
			}
		}
	}
}
