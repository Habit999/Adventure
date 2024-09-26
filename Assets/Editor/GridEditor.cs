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
		
		if(GUILayout.Button("View Grid"))
		{
			isGridVisible = !isGridVisible;
			Debug.Log($"View Grid = {isGridVisible}");
		}
	}
	
	void OnDrawGizmos()
	{
		// DRAW GRID WITH GIZMOS
		if(isGridVisible)
		{
			Vector3 lastPositionX = Vector3.zero;
			Vector3 lastPositionZ = Vector3.zero;
			for(int x = 0; x < grid._gridLengthX; x++)
			{
				if(x == 0)
				{
					Gizmos.color = Color.green;
					Gizmos.DrawWireCube(grid.transform.position + new Vector3(grid._gridOffsetX, 0, grid._gridOffsetZ), grid._gridCellPrefab.transform.localScale);
				}
				else
				{
					Gizmos.color = Color.green;
					Gizmos.DrawWireCube(lastPositionX + new Vector3(grid._gridOffsetX + grid._gridCellSpacing, 0, grid._gridOffsetZ + grid._gridCellSpacing), grid._gridCellPrefab.transform.localScale);
				}
				
				for(int y = 0; y < grid._gridLengthZ; y++)
				{
					if(y == 0)
					{
						lastPositionZ = lastPositionX;
					}
					else
					{
						Gizmos.color = Color.green;
						Gizmos.DrawWireCube(lastPositionZ + new Vector3(grid._gridOffsetX + grid._gridCellSpacing, 0, grid._gridOffsetZ + grid._gridCellSpacing), grid._gridCellPrefab.transform.localScale);
					}
				}
			}
		}
	}
}
