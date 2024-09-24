using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
	public static float MouseX { get { return Input.GetAxis("Mouse X"); } }
	public static float MouseY { get { return Input.GetAxis("Mouse Y"); } }
	
	public static KeyCode MoveForward = KeyCode.W;
	public static KeyCode MoveBackward = KeyCode.S;
	public static KeyCode MoveLeft = KeyCode.A;
	public static KeyCode MoveRight = KeyCode.D;
}
