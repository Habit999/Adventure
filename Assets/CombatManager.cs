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

	[HideInInspector] public float AnimationTimer;
	
	[HideInInspector] public bool IsAnimating;

	private bool enemyHit;
	
	private void Start()
	{
		damageAreaColldier = GetComponent<BoxCollider>();
		damageAreaColldier.enabled = false;

        IsAnimating = false;
        enemyHit = false;
    }

    private void Update()
    {
		if (AnimationTimer > 0 && IsAnimating) AnimationTimer -= Time.deltaTime;
		else if (IsAnimating)
		{
			damageAreaColldier.enabled = false;
			IsAnimating = false;
            enemyHit = false;
        }
    }

    public void SwingWeapon()
	{
        damageAreaColldier.enabled = true;
    }

    public void TriggerAnimator(string triggerName)
	{
        IsAnimating = true;
		RightHandAnimator.SetTrigger(triggerName);
        AnimationTimer = RightHandAnimator.GetCurrentAnimatorStateInfo(0).length;
    }
	
	void OnTriggerEnter(Collider col)
	{
		if(damageAreaColldier.enabled)
		{
			if(col.gameObject.tag == "Enemy" && !enemyHit)
			{
				// Damage enemy
				float calculatedDamage = Controller.InventoryMngr.EquippedItem.GetComponent<Item>().Damage * (1 + (Controller.SkillsMngr.CurrentSkills.strength / 100));
                col.gameObject.GetComponent<Enemy>().TakeDamage(calculatedDamage);
                enemyHit = true;
            }
			if(col.gameObject.tag == "Chest")
            {
				col.gameObject.GetComponent<TreasureChest>().HitChest();
            }
        }
	}
}
