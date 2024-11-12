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
	
	public static KeyCode ToggleUI;
	
	public static KeyCode HotBar0;
	public static KeyCode HotBar1;
	public static KeyCode HotBar2;
	public static KeyCode HotBar3;
	public static KeyCode HotBar4;
	public static KeyCode HotBar5;
	public static KeyCode HotBar6;
	public static KeyCode HotBar7;
	
	public static void LoadDefaults()
	{
		Forward = KeyCode.W;
		Backward = KeyCode.S;
		Left = KeyCode.A;
		Right = KeyCode.D;
		
		Sprint = KeyCode.LeftShift;
		
		Interact = KeyCode.E;
		
		Back = KeyCode.Escape;
		
		ToggleUI = KeyCode.Space;
		
		HotBar0 = KeyCode.Alpha0;
		HotBar1 = KeyCode.Alpha1;
		HotBar2 = KeyCode.Alpha2;
		HotBar3 = KeyCode.Alpha3;
		HotBar4 = KeyCode.Alpha4;
		HotBar5 = KeyCode.Alpha5;
		HotBar6 = KeyCode.Alpha6;
		HotBar7 = KeyCode.Alpha7;
	}
}
