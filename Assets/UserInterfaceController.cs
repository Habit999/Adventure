using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInterfaceController : MonoBehaviour
{
	public static UserInterfaceController Instance;
	
	protected int MaxActionKeys { get { return actionKeys.childCount; } }
	protected int MaxActionPages { get { return (TEMP_INVITEMCOUNT / MaxActionKeys) + ((TEMP_INVITEMCOUNT % MaxActionKeys > 0) ? 1 : 0); } }
	
	[SerializeField] Transform actionKeys;
	[SerializeField] TextMeshProUGUI pageNumberText;
	
	int actionKeyPage;
	
	int TEMP_INVITEMCOUNT = 12;
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
		
		// Defaults
		actionKeyPage = 1;
	}
	
	void Update()
	{
		UpdateUI();
	}
	
	void UpdateUI()
	{
		// Action Keys
		if(pageNumberText.gameObject != null) pageNumberText.SetText(actionKeyPage.ToString());
		else print("'Page Number Text' not assigned so will not be updated !");
	}
	
	#region Navigation
	public void ActionKeysUp()
	{
		if(actionKeyPage - 1 > 0)
		{
			actionKeyPage--;
		}
	}
	
	public void ActionKeysDown()
	{
		if(actionKeyPage + 1 <= MaxActionPages)
		{
			actionKeyPage++;
		}
	}
	#endregion
	
	
	#region Action Keys
	public void ActivateActionKey(int actionIndex)
	{
		int itemIndex = actionIndex + (MaxActionKeys * (actionKeyPage - 1));
	}
	#endregion
}
