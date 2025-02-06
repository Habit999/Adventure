using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Controls", menuName = "Controls")]
public class SO_Controls : ScriptableObject
{
	public float MouseX { get { return Input.GetAxis("Mouse X"); } }
	public float MouseY { get { return Input.GetAxis("Mouse Y"); } }
	
	public int MousePrimary = 0;
	public int MouseSecondary = 1;
	public float MouseSensitivity = 1;
	
	public KeyCode Forward = KeyCode.W;
	public KeyCode Backward = KeyCode.S;
	public KeyCode Left = KeyCode.A;
	public KeyCode Right = KeyCode.D;
	
	public KeyCode Sprint = KeyCode.LeftShift;
	
	public KeyCode Interact = KeyCode.E;
	
	public KeyCode Back = KeyCode.Escape;
	
	public KeyCode ToggleUI = KeyCode.Tab;
	
	public KeyCode HotBar0 = KeyCode.Alpha0;
	public KeyCode HotBar1 = KeyCode.Alpha1;
	public KeyCode HotBar2 = KeyCode.Alpha2;
	public KeyCode HotBar3 = KeyCode.Alpha3;
	public KeyCode HotBar4 = KeyCode.Alpha4;
	public KeyCode HotBar5 = KeyCode.Alpha5;
	public KeyCode HotBar6 = KeyCode.Alpha6;
	public KeyCode HotBar7 = KeyCode.Alpha7;
	
	public void LoadDefaults()
	{
		MousePrimary = 0;
		MouseSecondary = 1;
        MouseSensitivity = 1;

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
