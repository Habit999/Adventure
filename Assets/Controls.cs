using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Controls
{
	public static float MouseX { get { return Input.GetAxis("Mouse X"); } }
	public static float MouseY { get { return Input.GetAxis("Mouse Y"); } }
	
	public static KeyCode Forward;
	public static KeyCode Backward;
	public static KeyCode Left;
	public static KeyCode Right;
	
	public static KeyCode Sprint;
	
	public static KeyCode Interact;
	
	public static KeyCode Back;
	
	public static void LoadDefaults()
	{
		Forward = KeyCode.W;
		Backward = KeyCode.S;
		Left = KeyCode.A;
		Right = KeyCode.D;
		
		Sprint = KeyCode.LeftShift;
		
		Interact = KeyCode.E;
		
		Back = KeyCode.Escape;
	}
}
