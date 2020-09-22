using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX.Utility;

public class TransitionManager : MonoBehaviour
{
	private struct TransitionData
    {
        private Vector3 tt;
    }

	private TransitionData transitionData;

    private static TransitionManager instance;

	/* @brief Get the instance of the static singleton.
    */
	public static TransitionManager Instance { get { return instance; } }
	private void Awake()
	{
		// Assure that the object can be destroyed if has to be created again later.
		if (instance != null && instance != this)
			Destroy(this.gameObject);
		else
			instance = this;
	}

	private void ExecutePhaseChange()
    { 

    }


	//public TransitionData SetTransitionData
	//{
	//	get { return instance.transitionData; }
	//}








 //   [SerializeField] private List<Transition> bossPhaseTransitions = new List<Transition>();

 //   private bool inTransition = false;

 //   private static TransitionManager instance;
    
	//private void Awake()
	//{
 //       // Assure that the object can be destroyed if has to be created again later.
 //       if (instance != null && instance != this)
 //           Destroy(this.gameObject);
 //       else
 //           instance = this;
 //   }

 //   /* @brief Get the instance of the static singleton.
 //   */
 //   public static TransitionManager Instance { get { return instance; } }

 //   public void SetParticleData(GameObject a_newParticle, Vector3 a_newParticlePos)
 //   {
        
 //   }

 //   public void PlayBossTransition(int a_indexOfTransition)
 //   {
 //       bossPhaseTransitions[a_indexOfTransition].Execute();
 //   }

}
