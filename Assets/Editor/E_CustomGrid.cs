using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomGrid))]
public class E_CustomGrid : Editor
{
	CustomGrid targetGrid;
	
	GameObject[,] tempPreviewCells;
	Vector3[,] tempCellPositions;
	Vector3[,] tempOccupantPositions;
	Vector3[,] tempOccupantRotations;
	
	bool previewsActive;
	
	bool GetOffsetData()
	{
		if(tempPreviewCells != null && tempPreviewCells.Length > 0)
		{
			for(int x = 0; x < targetGrid._gridLengthX; x++)
			{
				for(int z = 0; z < targetGrid._gridLengthZ; z++)
				{
					if(targetGrid._cellsAreActive)
					{
						tempCellPositions[x, z] = targetGrid._gridCells[x, z]._cellPositionOffset;
						tempOccupantPositions[x, z] = targetGrid._gridCells[x, z]._occupantPositionOffset;
						tempOccupantRotations[x, z] = targetGrid._gridCells[x, z]._occupantRotationOffset;
					}
					else
					{
						tempCellPositions[x, z] = Vector3.zero;
						tempOccupantPositions[x, z] = Vector3.zero;
						tempOccupantRotations[x, z] = Vector3.zero;
					}
				}
			}
			return true;
		}
		else return false;
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		targetGrid = (CustomGrid) EditorGUILayout.ObjectField("Target Grid", targetGrid, typeof(CustomGrid), true);
		
		EditorGUILayout.Space(10);
		
		// Removes temp preview on startup or missing target grid
		if(tempPreviewCells != null && (!targetGrid._enableEditorTools || targetGrid == null))
		{
			foreach(GameObject previewOccupant in tempPreviewCells)
			{
				if(previewOccupant != null && previewOccupant.activeSelf)
				{
					DestroyImmediate(previewOccupant);
					continue;
				}
			}
		}
		
		if(targetGrid != null && targetGrid._enableEditorTools && targetGrid._initialGenerationComplete)
		{
			string gridVisible = targetGrid._isGridVisible? "Active" : "InActive";
			string gridPreviewActive = targetGrid._activeGridPreview? "Active" : "InActive";
			GUILayout.Label($"Grid Gizmos : {gridVisible}");
			GUILayout.Label($"Grid Cell Preview : {gridPreviewActive}");
			
			EditorGUILayout.Space(5);
			
			if(GUILayout.Button("Generate Grid"))
			{
				targetGrid.GenerateGrid();
				
				if(tempPreviewCells != null)
				{
					foreach(GameObject previewOccupant in tempPreviewCells)
					{
						if(previewOccupant != null && previewOccupant.activeSelf)
						{
							DestroyImmediate(previewOccupant);
							continue;
						}
					}
				}
				tempPreviewCells = new GameObject[targetGrid._gridLengthX, targetGrid._gridLengthZ];
				tempCellPositions = new Vector3[targetGrid._gridLengthX, targetGrid._gridLengthZ];
				tempOccupantPositions = new Vector3[targetGrid._gridLengthX, targetGrid._gridLengthZ];
				tempOccupantRotations = new Vector3[targetGrid._gridLengthX, targetGrid._gridLengthZ];
			}
			
			EditorGUILayout.Space(5);
			
			if(GUILayout.Button("Toggle Grid Visibility"))
			{
				targetGrid._isGridVisible = !targetGrid._isGridVisible;
			}
			
			if(GUILayout.Button("Toggle Grid Preview"))
			{
				targetGrid._activeGridPreview = !targetGrid._activeGridPreview;
				targetGrid.TogglePreviewGrid();
			}
			
			if(GUILayout.Button("Open GridEditor"))
			{
				if(GetOffsetData())
				{
					GridEditor gridEditor = (GridEditor) EditorWindow.GetWindow(typeof(GridEditor), false, "GridEditor", true);
					gridEditor._grid = targetGrid;
					
					if(targetGrid.LoadGridData())
					{
						gridEditor._tempCullingData = targetGrid._gridCulling;
						gridEditor._loadedOccupantData = targetGrid._gridCellOccupants;
					}
					else
					{
						gridEditor._tempCullingData = new bool[targetGrid._gridLengthX, targetGrid._gridLengthZ];
						gridEditor._loadedOccupantData = new GameObject[targetGrid._gridLengthX, targetGrid._gridLengthZ];
					}
					
					gridEditor._loadedCellPositionOffset = tempCellPositions;
					gridEditor._loadedOccupantPositionOffset = tempOccupantPositions;
					gridEditor._loadedOccupantRotationOffset = tempOccupantRotations;
					
					gridEditor.Show();
				}
			}
		}
	}
}
