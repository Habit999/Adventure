using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
	protected PlayerController Controller { get { return PlayerController.Instance; } }
	
	public Animator RightHandAnimator;
	
	[Space(5)]
	
	[SerializeField] private float swingTime;
	
	private BoxCollider damageAreaColldier;
	
	private bool isAttacking;
	
	void Start()
	{
		damageAreaColldier = GetComponent<BoxCollider>();
		damageAreaColldier.enabled = false;
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
				float calculatedDamage = Controller.InventoryMngr.EquippedItem.GetComponent<Item>().Damage * (1 + (Controller.SkillsMngr.CurrentSkills.strength / 100));
                col.gameObject.GetComponent<Enemy>().TakeDamage(calculatedDamage);
            }
			if(col.gameObject.tag == "Chest")
            {
				col.gameObject.GetComponent<TreasureChest>().HitChest();
            }
        }
	}
}
