using UnityEngine;
using UnityEditor;

public class MovementGridEditor : EditorWindow
{
	protected bool isShowingGrid = false;
	
	[MenuItem("Tools/GridEditor")]
	public static void ShowWindow() // Runs when opening window
	{
		GetWindow(typeof(MovementGridEditor));
	}
	
	void OnGUI()
	{
		GUILayout.Label("Grid Editor Tools", EditorStyles.boldLabel);
		
		GUILayout.Space(10);
		
		// "varible" = EditorGUILayout.(TextField, IntField, Slider, FloatField, ObjectField)
		
		if(GUILayout.Button("Show Grid"))
		{
			Debug.Log("Editor Button Works");
		}
	}
}
