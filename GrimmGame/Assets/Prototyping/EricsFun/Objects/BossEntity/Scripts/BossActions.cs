//========== Grimm - BossActions.cs - 11/08/2020 ==========//
// Author:  Eric Brkic
// Purpose: 
//=========================================================//
using System;
using System.Collections.Generic;
using UnityEngine;

public enum SteeringBehaviours
{
    PASSIVE,
    DEFENSIVE,
    AGGRESSIVE,
    SEEK
}

[Serializable]
public class BossActions
{
    [SerializeField] private string attackName;
    [SerializeField] private float actionTimeLimit;
    [SerializeField] private int animationNumber;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackWeight;
    [SerializeField] private float lastTimeUsed;
    [SerializeField] private int numberOfUses;
    [SerializeField] private SteeringBehaviours behaviourType;
    [SerializeField] private List<HitEffectsEnum> attackEffectList;
    [SerializeField] private List<BossWeaponColliderEnums> collidersToModify;

    /* @brief Retrieve the animation of the attack within the list at the specified _attackIndex index.
     * @param Index of the attack within the list.
     * @return Int of the animation number.
     */
    public int GetAnimNum
    {
        get { return animationNumber; }
    }

    /* @brief Retrieve the attack damage of the attack within the list at the specified _attackIndex index.
     * @param Index of the attack within the list.
     * @return Int of the attack damage.
     */
    public float AttackDamage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }

    /* @brief Retrieve the attack range of the attack within the list at the specified _attackIndex index.
     * @param Index of the attack within the list.
     * @return Int of the attack range value.
     */
    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }

    /* @brief Retrieve all colliders of the attack within the list at the specified _attackIndex index.
     * @param Index of the attack within the list.
     * @return List of weapon collider enums of the colliders.
     */
    public List<BossWeaponColliderEnums> GetCollidersToModify
    {
        get { return collidersToModify; }
    }

    /* @brief Retrieve a collider of the attack within the list at the specified _attackIndex index.
     * @param Index of the attack within the list.
     * @return Int of the colliders value.
     */
    public int GetSingleColliderToModify(int _colliderIndex)
    {
        return Convert.ToInt32(collidersToModify[_colliderIndex]);
    }

    /* @brief Retrieve the number of colliders within the attack list at the speified _attackIndex.
     * @param Index of the attack within the list.
     * @return Int of the number of colliders to modify.
     */
    public int GetNumberOfColliders
    {
        get { return collidersToModify.Count; }
    }

    public SteeringBehaviours GetBehaviourType
    {
        get { return behaviourType; }
    }

    public float AttackWeight
    {
        get { return attackWeight; }
        set { attackWeight = value; }
    }

    public float LastTimeUsed
    {
        get { return lastTimeUsed; }
        set { lastTimeUsed = value; }
    }

    public int NumberOfUses
    {
        get { return numberOfUses; }
        set { numberOfUses = value; }
    }

    public string GetAttackName
    {
        get { return attackName; }
    }

    public void ResolveOnHitEffects(GameObject _target, Vector3 _collisionPoint)
    {
        for (int i = 0; i < attackEffectList.Count; i++)
        {
            switch (attackEffectList[i])
            {
                case HitEffectsEnum.KNOCKBACK_WEAK:
                    OnHitEffects.Instance.ResolveKnockbackWeak(_target, _collisionPoint);
                    break;
                case HitEffectsEnum.KNOCKBACK_STRONG:
                    OnHitEffects.Instance.ResolveKnockbackStrong(_target);
                    break;
                default:
                    break;
            }
        }
    }

    /* @brief Checks to see if the index passed is valid or not.
     * @param Index being checked against.
     * @return True if the position exists, false if not.
     */
    //private bool DoesAttackIndexExists(int _attackIndex)
    //{
    //    // If the attack index is greater than the number of attacks,
    //    // it is not possible to be within the list.
    //    if (_attackIndex > bossAttackList.Count)
    //    {
    //        // Print an error message.
    //        Debug.LogError("ERROR: The index " + _attackIndex + " is out of range of the attack list! The list current stores " + bossAttackList.Count + " attacks!");
    //        return false;
    //    }
    //    else
    //        return true;
    //}
}
