using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossTestin : MonoBehaviour
{
	[SerializeField] private GameObject playerTarget = null;
	[SerializeField] private BossAttacks availableAttacks = null;
	private int currentAttackIndex = 0;
	private bool canCalculateNextAttack = false;

	private void Update()
	{
		if (canCalculateNextAttack)
			SelectNextAttack();
	}

	private void SelectNextAttack()
	{
		canCalculateNextAttack = false;

		// Vector3.Distance gives the sqrt result. The following avoids the sqrt function... There is probably a function for this somewhere.
		float distance = Mathf.Pow(this.transform.position.x - playerTarget.transform.position.x, 2f) +
						 Mathf.Pow(this.transform.position.y - playerTarget.transform.position.y, 2f) +
						 Mathf.Pow(this.transform.position.z - playerTarget.transform.position.z, 2f);

		currentAttackIndex = DetermineBestAttack(distance);
		Debug.Log("Winning attack is: " + availableAttacks.GetAttackName(currentAttackIndex));

		// Some function Anton creates to actually get the boss in position and play the animation etc...
		ResolveAttack();
	}

	private int DetermineBestAttack(float _bossToTargetDist)
	{
		// This is all fake and not needed, I just wanted to add more numbers to confuse people, but also to show off attacks will be weighted by N values.
		float coolnessFactor = 0; // Currently using the "Animation Number" as coolness :).
		float effortToGetInRange = 0;
		float damageOfAttack = 0;

		// Store the current highest scoring attack.
		float highestScoringAttack = 0;
		int optimalAttackIndex = 0;

		// Loop through each available attack within the BossAttacks list.
		for (int i = 0; i < availableAttacks.GetNumberOfAttacksInList(); i++)
		{
			float currentAttackScore = 0;

			coolnessFactor = availableAttacks.GetAnimNum(i);
			currentAttackScore += coolnessFactor;

			damageOfAttack = availableAttacks.GetAttackDamage(i);
			currentAttackScore += damageOfAttack;

			float attackRange = availableAttacks.GetAttackRange(i);
			// If the attack is out of range, it is more effort.
			if (attackRange > _bossToTargetDist)
				effortToGetInRange += attackRange - _bossToTargetDist;
			currentAttackScore += effortToGetInRange;

			// If this attack scores higher than the current highest, it becomes the new hights and current optimal index.
			if (currentAttackScore > highestScoringAttack)
			{ 
				highestScoringAttack = currentAttackScore;
				optimalAttackIndex = i;
			}

			Debug.Log("Attack: " + availableAttacks.GetAttackName(i) + " has a score of: " + currentAttackScore);
		}

		return optimalAttackIndex;
	}

	private void ResolveAttack()
	{
		// Do some shit to get the boss in range and ready to attack.
		// . . .

		// Once in range, pass the animation number to animator with:
		// AnimationEventThingIDK = availableAttacks.GetAnimNum(currentAttackIndex);

		// Stuff like removing health I would imagine happens in the OnCollision. AFter all, it is possible that the animation will miss the player.
	}

	private void OnCollisionEnter(Collision collision)
	{
		// Yay, the animation hit, we are now here.

		// Remove the health from the playerTarget
		// EntityStats.Instance.DeminishHealthOffEntity("Player", availableAttacks.GetAttackDamage(currentAttackIndex));

		// Resolve any and all On Hit Effects the attack at the current index may have. The point of collision should also be passed for particles.
		// I've just passed the first contact points position.
		availableAttacks.ResolveOnHitEffects(currentAttackIndex, playerTarget, collision.contacts[0].point);

		// After all OnCollision logic is done, it will be appropraite to allowed the next attack to be added in.
		canCalculateNextAttack = true;
	}
}
