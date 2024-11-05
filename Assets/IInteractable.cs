using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInteractable : MonoBehaviour
{
	public bool _canInteract;
	
	void Awake()
	{
		_canInteract = true;
	}
}
