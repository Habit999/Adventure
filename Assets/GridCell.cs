using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
	private static float checkCellPositionBuffer = 3f;
	
	[HideInInspector] public Vector2 _cellIndex;
	[HideInInspector] public CustomGrid _connectedGrid;
	
	[HideInInspector] public GameObject _activeCellOccupant;
	public GameObject _cellOccupantPrefab;
	
	[Space(10)]
	
	public Vector3 _cellPositionOffset;
	[Space(5)]
	public Vector3 _occupantPositionOffset;
	public Vector3 _occupantRotationOffset;
	
	IEnumerator positionCheckRoutine;
	bool isCheckRoutineActive;
	
	void OnDisable()
	{
		if(isCheckRoutineActive) StopCoroutine(positionCheckRoutine);
	}
	
	void Start()
	{
		isCheckRoutineActive = false;
		positionCheckRoutine = PositionCheckBuffer();
		StartCoroutine(positionCheckRoutine);
		
		if(_cellOccupantPrefab != null)
		{
			_activeCellOccupant = Instantiate(_cellOccupantPrefab, _cellOccupantPrefab.transform.position + _occupantPositionOffset, _cellOccupantPrefab.transform.rotation * Quaternion.Euler(_occupantRotationOffset), transform);
		}
	}
	
	IEnumerator PositionCheckBuffer()
	{
		isCheckRoutineActive = true;
		CheckPosition();
		yield return new WaitForSeconds(checkCellPositionBuffer);
		positionCheckRoutine = PositionCheckBuffer();
		StartCoroutine(positionCheckRoutine);
	}
		
	void CheckPosition()
	{
		Vector3 correctedCellPosition = _connectedGrid._cellPositions[(int) _cellIndex.x, (int) _cellIndex.y];
		correctedCellPosition.z = correctedCellPosition.y;
		correctedCellPosition.y = 0;
		if(correctedCellPosition + _cellPositionOffset != transform.position)
		{
			transform.position = correctedCellPosition + _cellPositionOffset;
		}
	}
}
