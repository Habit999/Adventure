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

	public float AnimationTimer;
	
	public bool IsAnimating;
	
	private void Start()
	{
		damageAreaColldier = GetComponent<BoxCollider>();
		damageAreaColldier.enabled = false;

        IsAnimating = false;
    }

    private void Update()
    {
        if(AnimationTimer > 0 && IsAnimating) AnimationTimer -= Time.deltaTime;
		else IsAnimating = false;
    }

    public void SwingWeapon()
	{
		if(!IsAnimating)
		{
			IsAnimating = true;
			StartCoroutine(AttackTiming());
		}
	}

	public void TriggerAnimator(string triggerName)
	{
        IsAnimating = true;
		RightHandAnimator.SetTrigger(triggerName);
        AnimationTimer = RightHandAnimator.GetCurrentAnimatorStateInfo(0).length;
    }

	public void AnimationFinished()
	{
		IsAnimating = false;
	}
	
	IEnumerator AttackTiming()
	{
		damageAreaColldier.enabled = true;
		yield return new WaitForSeconds(swingTime);
		damageAreaColldier.enabled = false;
		yield return new WaitForSeconds(RightHandAnimator.GetCurrentAnimatorStateInfo(0).length);
		IsAnimating = false;
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
