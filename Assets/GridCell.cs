using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
	private static float checkCellTransformBuffer = 3f;
	
	[HideInInspector] public Vector2 _cellIndex;
	[HideInInspector] public CustomGrid _connectedGrid;
	
	[HideInInspector] public GameObject _activeCellOccupant;
	public GameObject _cellOccupantPrefab;
	
	[Space(10)]
	
	public Vector3 _cellPositionOffset;
	[Space(5)]
	public Vector3 _occupantPositionOffset;
	public Vector3 _occupantRotationOffset;
	
	IEnumerator transformCheckRoutine;
	
	void Start()
	{
		transformCheckRoutine = TransformCheckBuffer();
		StartCoroutine(transformCheckRoutine);
		
		SpawnOccupant();
	}
	
	public void SpawnOccupant()
	{
		if(_connectedGrid)
		{
			if(_activeCellOccupant != null) Destroy(_activeCellOccupant);
		
			if(_cellOccupantPrefab != null)
			{
				_activeCellOccupant = Instantiate(_cellOccupantPrefab, transform.position, _cellOccupantPrefab.transform.rotation * Quaternion.Euler(_occupantRotationOffset));
				_activeCellOccupant.transform.parent = transform;
				_activeCellOccupant.transform.position += _occupantPositionOffset;
			}
		}
	}
	
	IEnumerator TransformCheckBuffer()
	{
		CheckTransform();
		yield return new WaitForSeconds(checkCellTransformBuffer);
		transformCheckRoutine = TransformCheckBuffer();
		StartCoroutine(transformCheckRoutine);
	}
		
	void CheckTransform()
	{
		// Cell Position
		Vector3 correctedCellPosition = _connectedGrid._cellGenerationPositions[(int) _cellIndex.x, (int) _cellIndex.y];
		correctedCellPosition.z = correctedCellPosition.y;
		correctedCellPosition.y = 0;
		transform.position = correctedCellPosition + _cellPositionOffset;
		
		//Occupant Position & Rotation
		if(_activeCellOccupant != null)
		{
			_activeCellOccupant.transform.position = _occupantPositionOffset;
			_activeCellOccupant.transform.rotation = _cellOccupantPrefab.transform.rotation * Quaternion.Euler(_occupantRotationOffset);
		}
	}
}
