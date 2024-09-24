using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGrid : MonoBehaviour
{
	public static MovementGrid Instance;
	
	[HideInInspector] public Vector2[,] gridPositions;
	
	[SerializeField] int gridLengthX;
	[SerializeField] int gridLengthZ;
	public float _gridCellSpacing;
	
	[Space(2)]
	
	public float _gridOffsetX;
	public float _gridOffsetZ;
	
	[Space(5)]
	
	[SerializeField] GameObject gridCellPrefab;
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
	}
	
	void Start()
	{
		SpawnGrid();
	}
	
	void SpawnGrid()
	{
		gridPositions = new Vector2[gridLengthX, gridLengthZ];
		
		float xAxis;
		float zAxis;
		
		// Grid X Axis
		for(int x = 0; x < gridLengthX; x++)
		{
			if(x == 0)
			{
				Vector3 lastCellPosition = Instantiate(gridCellPrefab, new Vector3(_gridOffsetX, 0, _gridOffsetZ), Quaternion.identity, transform).transform.position;
				gridPositions[x, 0] = new Vector2(lastCellPosition.x, lastCellPosition.z);
			}
			else
			{
				xAxis = gridPositions[x - 1, 0].x + _gridOffsetX;
				
				Vector3 lastCellPosition = Instantiate(gridCellPrefab, new Vector3(xAxis, 0, _gridOffsetZ), Quaternion.identity, transform).transform.position;
				gridPositions[x, 0] = new Vector2(lastCellPosition.x, lastCellPosition.z);
			}
			
			// Grid Z Axis
			for(int z = 1; z < gridLengthZ; z++)
			{
				xAxis = gridPositions[x, z - 1].x;
				zAxis = gridPositions[x, z - 1].y + _gridOffsetZ;
				
				Vector3 lastCellPosition = Instantiate(gridCellPrefab, new Vector3(xAxis, 0, zAxis), Quaternion.identity, transform).transform.position;
				gridPositions[x, z] = new Vector2(lastCellPosition.x, lastCellPosition.z);
			}
		}
	}
}
