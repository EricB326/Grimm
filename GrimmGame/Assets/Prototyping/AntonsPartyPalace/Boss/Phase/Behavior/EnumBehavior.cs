using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SM = StateMachine
public enum BossBehavior
{
    // Will move towards certain points. Idle
    Wander,
    // In case of interuptions the boss will wait a moment. From animation SM?
    Interupted,
    // Move towards the player. Different phases may have different fashions of this
    Seek,
    // Enters the animation SM to then do the logic there.
    // It will need to pass some information to the SM.
    Attack
}
