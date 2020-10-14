using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ideally this will store just imformation for an attack
// Damage done, Range ect and be set in the inspector 
// window.



public class BossAttackOLD : MonoBehaviour
{
    // Mostly to easily navigate list.
    public string m_name;
    // The int passed into the animator to play the animation
    public int m_animatorInformation;
    // Which colliders to activate for the attack.
    public List<BossWeaponColliderEnums> m_attackColliders;
    // Number of attacks.
    // May be redundant if attacks are and needs more thinking.
    public int m_numberOfAttacks;
}