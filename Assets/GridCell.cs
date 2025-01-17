using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
	public CustomGrid _connectedGrid;
	
	public bool _cellActive;
	
	public Vector3 OccupantPosition { get { return _activeOccupant != null ? _activeOccupant.transform.position : transform.position; } }
	
	[HideInInspector] public Vector2 _gridIndex;
	
	public GameObject _occupantPrefab;
	[HideInInspector] public GameObject _activeOccupant;
	
	[Space(5)]
	
	public Vector3 _occupantPosition;
	public Vector3 _occupantEulerAngles;
	
	void OnEnable()
	{
		CustomGrid.SpawnCellOccupants += SpawnOccupant;
	}
	
	void OnDisable()
	{
		CustomGrid.SpawnCellOccupants -= SpawnOccupant;
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.T)) SpawnOccupant();
	}
	
	public void UpdateOccupantTransform()
	{
		if(_activeOccupant != null)
		{
			_activeOccupant.transform.position = _occupantPosition;
			_activeOccupant.transform.eulerAngles = _occupantEulerAngles;
		}
	}
	
	public void SpawnOccupant()
	{
		if(_occupantPrefab != null)
		{
			if(!_connectedGrid._enableEditorTools)
			{
				if(_activeOccupant != null) Destroy(_activeOccupant);
			}
			else
			{
				if(_activeOccupant != null) DestroyImmediate(_activeOccupant, false);
			}
			
			_activeOccupant = Instantiate(_occupantPrefab, transform.position, Quaternion.identity);
			_activeOccupant.transform.parent = transform;
			
			_activeOccupant.transform.localPosition = _occupantPosition;
			_activeOccupant.transform.rotation = Quaternion.Euler(_occupantEulerAngles);
		}
	}
}
