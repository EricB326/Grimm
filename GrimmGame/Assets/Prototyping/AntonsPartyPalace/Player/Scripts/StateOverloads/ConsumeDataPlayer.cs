﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeDataPlayer : StateMachineBehaviour
{
    public List<PlayerDataEnums> m_dataToClear;


    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (PlayerDataEnums clear in m_dataToClear)
        {
            switch (clear)
            {
                case PlayerDataEnums.Attack:
                    {
                        animator.SetBool("Input/Attack", false);
                        break;
                    }
                case PlayerDataEnums.Roll:
                    {
                        animator.SetBool("Input/Roll", false);
                        break;
                    }
                case PlayerDataEnums.Damage:
                    {
                        animator.SetInteger("AnyState/Damage", 0);
                        break;
                    }
                case PlayerDataEnums.Death:
                    {
                        animator.SetInteger("AnyState/Death", 0);
                        break;
                    }
                case PlayerDataEnums.Cutscene:
                    {
                        animator.SetInteger("AnyState/Cutscene", 0);
                        break;
                    }
            }
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}