using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
	protected PlayerController Controller { get { return PlayerController.Instance; } }
	
	public Animator RightHandAnimator;
	
	[Space(5)]
	
	[SerializeField] float swingTime;
	
	BoxCollider damageAreaColldier;
	
	bool isAttacking;
	
	void Start()
	{
		damageAreaColldier = GetComponent<BoxCollider>();
		damageAreaColldier.enabled = false;
	}
	
	void Update()
	{
		if(Input.GetMouseButtonDown(1))
			RightHandAnimator.SetTrigger("UseBandage");
	}
	
	public void SwingWeapon()
	{
		if(!isAttacking)
		{
			isAttacking = true;
			StartCoroutine(AttackTiming());
		}
	}
	
	IEnumerator AttackTiming()
	{
		damageAreaColldier.enabled = true;
		yield return new WaitForSeconds(swingTime);
		damageAreaColldier.enabled = false;
		yield return new WaitForSeconds(RightHandAnimator.GetCurrentAnimatorStateInfo(0).length);
		isAttacking = false;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(damageAreaColldier.enabled)
		{
			if(col.gameObject.tag == "Enemy")
			{
				// Damage enemy
				// Use "PlayerController.Instance.InventoryMngr._equippedItem.GetComponent<Item>()._damage"
			}
			if(col.gameObject.tag == "Chest")
            {
				col.gameObject.GetComponent<TreasureChest>().HitChest();
            }
        }
	}
}
