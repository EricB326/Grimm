//========== Grimm - BossAttacks.cs - 11/08/2020 ==========//
// Author:  Eric Brkic
// Purpose: 
//=========================================================//
using System;
using System.Collections.Generic;
using System.Configuration;
using UnityEditor.PackageManager;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    /* Struct containing the different boss attacks available to be used.
    */
    [Serializable]
    private struct bossAttackField
    {
        public string attackName;
        public int animationNumber;
        public float attackDamage;
        public float attackRange;
        public List<HitEffectsEnum> attackEffectList;
        public List<BossWeaponColliderEnums> collidersToModify;
    }

    /* Store a list of the attacks the boss can do as well as some extra values.
    */
    [SerializeField] private List<bossAttackField> bossAttackList = new List<bossAttackField>();

    /* @brief Retrieve the animation of the attack within the list at the specified _attackIndex index.
     * @param Index of the attack within the list.
     * @return Int of the animation number.
     */
    public int GetAnimNum(int _attackIndex)
    {
        if (!DoesAttackIndexExists(_attackIndex))
            return -1;

        return bossAttackList[_attackIndex].animationNumber;
    }

    /* @brief Retrieve the attack damage of the attack within the list at the specified _attackIndex index.
     * @param Index of the attack within the list.
     * @return Int of the attack damage.
     */
    public float GetAttackDamage(int _attackIndex)
    {
        if (!DoesAttackIndexExists(_attackIndex))
            return -1;

        return bossAttackList[_attackIndex].attackDamage;
    }

    /* @brief Retrieve the attack range of the attack within the list at the specified _attackIndex index.
     * @param Index of the attack within the list.
     * @return Int of the attack range value.
     */
    public float GetAttackRange(int _attackIndex)
    {
        if (!DoesAttackIndexExists(_attackIndex))
            return -1;

        return bossAttackList[_attackIndex].attackRange;
    }

    /* @brief Retrieve all colliders of the attack within the list at the specified _attackIndex index.
     * @param Index of the attack within the list.
     * @return List of weapon collider enums of the colliders.
     */
    public List<BossWeaponColliderEnums> GetCollidersToModify(int _attackIndex)
    {
        if (!DoesAttackIndexExists(_attackIndex))
            return new List<BossWeaponColliderEnums>();

        return bossAttackList[_attackIndex].collidersToModify;
    }
    
    /* @brief Retrieve a collider of the attack within the list at the specified _attackIndex index.
     * @param Index of the attack within the list.
     * @return Int of the colliders value.
     */
    public int GetSingleColliderToModify(int _attackIndex, int _colliderIndex)
    {
        if (!DoesAttackIndexExists(_attackIndex))
            return -1;

        return Convert.ToInt32(bossAttackList[_attackIndex].collidersToModify[_colliderIndex]);
    }

    /* @brief Retrieve the number of colliders within the attack list at the speified _attackIndex.
     * @param Index of the attack within the list.
     * @return Int of the number of colliders to modify.
     */
    public int GetNumberOfColliders(int _attackIndex)
    {
        if (!DoesAttackIndexExists(_attackIndex))
            return -1;

        int numberOfColliders = bossAttackList[_attackIndex].collidersToModify.Count;
        return numberOfColliders;
    }

    public string GetAttackName(int _attackIndex)
    {
        if (!DoesAttackIndexExists(_attackIndex))
            return string.Empty;

        return bossAttackList[_attackIndex].attackName;
    }

    public int GetNumberOfAttacksInList()
    {
        return bossAttackList.Count;
    }

    public void ResolveOnHitEffects(int _attackIndex, GameObject _target, Vector3 _collisionPoint)
    {
        if (!DoesAttackIndexExists(_attackIndex))
            return;

        for (int i = 0; i < bossAttackList[_attackIndex].attackEffectList.Count; i++)
        {
            switch (bossAttackList[_attackIndex].attackEffectList[i])
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
    private bool DoesAttackIndexExists(int _attackIndex)
    {
        // If the attack index is greater than the number of attacks,
        // it is not possible to be within the list.
        if (_attackIndex > bossAttackList.Count)
        {
            // Print an error message.
            Debug.LogError("ERROR: The index " + _attackIndex + " is out of range of the attack list! The list current stores " + bossAttackList.Count + " attacks!");
            return false;
        }
        else
            return true;
    }
}
