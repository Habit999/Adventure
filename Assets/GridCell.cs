using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
	[Tooltip("If checked the cell will be added to the grid for use in project")]
	public bool _inUse = true;
	
	public GameObject _cellOccupant;
	[SerializeField] GameObject cellOccupantPrefab;
	
	void Awake()
	{
		gameObject.SetActive(_inUse);
	}
	
	void Start()
	{
		if(_inUse && cellOccupantPrefab != null)
		{
			SpawnOccupant();
		}
	}
	
	void OnEnable()
	{
		if(_inUse && cellOccupantPrefab != null)
		{
			if(_cellOccupant == null)
			{
				SpawnOccupant();
			}
			_cellOccupant.SetActive(true);
		}
	}
	
	void OnDisable()
	{
		if(_cellOccupant != null) _cellOccupant.SetActive(false);
	}
	
	void SpawnOccupant()
	{
		_cellOccupant = Instantiate(cellOccupantPrefab, cellOccupantPrefab.transform.position, cellOccupantPrefab.transform.rotation, transform);
	}
}
