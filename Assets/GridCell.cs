using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls a grid cell from the 3D grid system
/// </summary>

public class GridCell : MonoBehaviour
{
	public CustomGrid ConnectedGrid;
	
	public bool CellActive;
	
	[HideInInspector] public Vector2 GridIndex;
	
	public GameObject OccupantPrefab;
	[HideInInspector] public GameObject ActiveOccupant;
	
	[Space(5)]
	
	public Vector3 OccupantPosition;
	public Vector3 OccupantEulerAngles;

	[Space(5)]

	public bool OccupantActiveOnSpawn;

	private bool isRunTime;
	
	void Awake()
	{
		CustomGrid.SpawnCellOccupants += SpawnOccupant;
		isRunTime = true;
	}
	
	void OnDisable()
	{
		CustomGrid.SpawnCellOccupants -= SpawnOccupant;
	}

    private void Start()
    {
		GetComponent<MeshRenderer>().enabled = false;
    }

    public void UpdateOccupantTransform()
	{
		if(ActiveOccupant != null)
		{
			ActiveOccupant.transform.localPosition = OccupantPosition;
			ActiveOccupant.transform.eulerAngles = OccupantEulerAngles;
		}
	}
	
	public void SpawnOccupant()
	{
		if(OccupantPrefab != null)
		{
			if(!ConnectedGrid.EnableEditorTools)
			{
				if(ActiveOccupant != null) Destroy(ActiveOccupant);
			}
			else
			{
				if(ActiveOccupant != null) DestroyImmediate(ActiveOccupant, false);
			}
			
			ActiveOccupant = Instantiate(OccupantPrefab, transform.position, Quaternion.identity);
			ActiveOccupant.transform.parent = transform;
			
			ActiveOccupant.transform.localPosition = OccupantPosition;
			ActiveOccupant.transform.rotation = Quaternion.Euler(OccupantEulerAngles);

			if(!OccupantActiveOnSpawn && isRunTime) ActiveOccupant.SetActive(false);
        }
	}

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position + OccupantPosition, 0.3f);
    }

#endif
}
