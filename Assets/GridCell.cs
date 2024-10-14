using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
	[HideInInspector] public GameObject _activeCellOccupant;
	public GameObject _cellOccupantPrefab;
	
	[Space(3)]
	
	public Vector3 _occupantPositionOffset;
	public Vector3 _occupantRotationOffset;
	
	[HideInInspector] public Vector2 _cellIndex;
	[HideInInspector] public CustomGrid _connectedGrid;
	
	void Start()
	{
		if(_cellOccupantPrefab != null)
		{
			_activeCellOccupant = Instantiate(_cellOccupantPrefab, _cellOccupantPrefab.transform.position + _occupantPositionOffset, _cellOccupantPrefab.transform.rotation * Quaternion.Euler(_occupantRotationOffset), transform);
		}
	}
}
