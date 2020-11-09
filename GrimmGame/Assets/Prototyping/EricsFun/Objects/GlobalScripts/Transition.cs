using UnityEngine;

public class Transition
{

    ///////////////////////////////////////////////////////////////////////
    /// Boss Phase Transitions
    ///////////////////////////////////////////////////////////////////////

    static public void PhaseOneTransition()
    {
        Debug.Log("PhaseOneX");
        //////////////////////
        // All Example Shit //
        //////////////////////

        // Set the positions of the characters for this transition.
        //EntityStats.Instance.GetObjectOfEntity("Player").transform.position = . . .
        //EntityStats.Instance.GetObjectOfEntity("Boss").transform.position = . . .

        // Disable the player camera and enable the cinematic camera.
        //playerCam.gameObject.SetActive(false);
        //cinematicCam.gameObject.SetActive(true);

        // Do other cinematic camera functionality to get it ready.
        //cinematicCam.transform.postion = . . .
        //cinematicCam.transform.LookAt(cameraLockOnPoint);

        // Begin animations of the characters
        //EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetInteger("SomeAnimPara/PhaseOne", 0);
        //EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<Animator>().SetInteger("SomeAnimPara/PhaseOne", 0);
        BossBrain bossbrain = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>();
        EntityStats.Instance.SetMaxHealthOfEntity("Boss", bossbrain.m_bossPhaseList[bossbrain.m_currentPhase + 1].m_healthAtStart);
        Camera.main.GetComponentInChildren<BossMusicScript>().IncrementBossPhase();
        EntityStats.Instance.RestoreAllStatsInstant();
    }

    static public void PhaseTwoTransition()
    {
        //    Debug.Log("PhaseTwoX");
        //    ////////////////////
        //    All Example Shit //
        //                     ////////////////////

        //    Set the positions of the characters for this transition.
        //   EntityStats.Instance.GetObjectOfEntity("Player").transform.position = . . .
        //   EntityStats.Instance.GetObjectOfEntity("Boss").transform.position = . . .


        //    Disable the player camera and enable the cinematic camera.
        //   playerCam.gameObject.SetActive(false);
        //   cinematicCam.gameObject.SetActive(true);

        //    Do other cinematic camera functionality to get it ready.
        //   cinematicCam.transform.postion = . . .
        //   cinematicCam.transform.LookAt(cameraLockOnPoint);

        BossBrain bossbrain = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>();
        EntityStats.Instance.SetMaxHealthOfEntity("Boss", bossbrain.m_bossPhaseList[bossbrain.m_currentPhase + 1].m_healthAtStart);
        Camera.main.GetComponentInChildren<BossMusicScript>().IncrementBossPhase();
        EntityStats.Instance.RestoreAllStatsInstant();
    }

    static public void PhaseThreeTransition()
    {
        BossBrain bossbrain = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>();
        EntityStats.Instance.SetMaxHealthOfEntity("Boss", bossbrain.m_bossPhaseList[bossbrain.m_currentPhase + 1].m_healthAtStart);
        Camera.main.GetComponentInChildren<BossMusicScript>().IncrementBossPhase();
        EntityStats.Instance.RestoreAllStatsInstant();
    }

    static public void PhaseFourTransition()
    {
        BossBrain bossbrain = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>();
        EntityStats.Instance.SetMaxHealthOfEntity("Boss", bossbrain.m_bossPhaseList[bossbrain.m_currentPhase + 1].m_healthAtStart);
        Camera.main.GetComponentInChildren<BossMusicScript>().IncrementBossPhase();
        EntityStats.Instance.RestoreAllStatsInstant();
    }



    static public void BossDefeatedTransition()
    {
        Debug.Log("Ripperino in pelagrino.");
        BossBrain bossbrain = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>();
        Camera.main.GetComponentInChildren<BossMusicScript>().IncrementBossPhase();


        bossbrain.enabled = false;
        EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<Animator>().SetInteger("Death",1);


    }

    // IDK in case we need some default thing if we add more phases??
    // Aint gotta be filled in.
    static public void DefaultBossTransision()
	{

        //////////////////////
        // All Example Shit //
        //////////////////////

        // Set the positions of the characters for this transition.
        //EntityStats.Instance.GetObjectOfEntity("Player").transform.position = . . .
        //EntityStats.Instance.GetObjectOfEntity("Boss").transform.position = . . .

        // Disable the player camera and enable the cinematic camera.
        //playerCam.gameObject.SetActive(false);
        //cinematicCam.gameObject.SetActive(true);

        // Do other cinematic camera functionality to get it ready.
        //cinematicCam.transform.postion = . . .
        //cinematicCam.transform.LookAt(cameraLockOnPoint);

        // Begin animations of the characters
        //EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetInteger("SomeAnimPara/PhaseOne", 0);
        //EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<Animator>().SetInteger("SomeAnimPara/PhaseOne", 0);
        //EntityStats.Instance.RestoreAllStats();
        BossBrain bossbrain = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>();
        EntityStats.Instance.SetMaxHealthOfEntity("Boss", bossbrain.m_bossPhaseList[bossbrain.m_currentPhase + 1].m_healthAtStart);
        EntityStats.Instance.RestoreAllStatsInstant();
	}

	///////////////////////////////////////////////////////////////////////
	/// Player Phase Transitions
	///////////////////////////////////////////////////////////////////////

	static public void PlayerPhaseResetTransiton()
	{
        GameObject player = EntityStats.Instance.GetObjectOfEntity("Player");
        Camera.main.GetComponentInChildren<BossMusicScript>().gameObject.SetActive(false);
        if (!player.GetComponent<Player>().m_Dead) // I don't really like this
        {
            player.GetComponent<AnimationEventsPlayer>().ResetAnimator();
            GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
            boss.GetComponent<AnimationEventsBoss>().ResetAnimator();
            player.GetComponent<Player>().m_InvinceFrames = true;
            Debug.Log("Player died, reset phase");
            player.GetComponent<Player>().m_lockon = false;

            EntityStats.Instance.SetHealthOfEntity("Player", 0.1f);

            player.GetComponent<Animator>().SetInteger("AnyState/Death", 1);
            boss.GetComponent<BossBrain>().enabled = false;
            boss.GetComponent<BossVariables>().m_activationScript.GetComponent<Collider>().isTrigger = true;
            boss.GetComponent<Animator>().SetInteger("CutScene", 2);
            player.GetComponent<Player>().m_Dead = true;
        }
    }

	static public void PlayerDefeatedTransition()
	{
        GameObject player = EntityStats.Instance.GetObjectOfEntity("Player");
        Camera.main.GetComponentInChildren<BossMusicScript>().gameObject.SetActive(false);
        if (!player.GetComponent<Player>().m_Dead) // I don't really like this
        {
            player.GetComponent<AnimationEventsPlayer>().ResetAnimator();
            GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
            boss.GetComponent<AnimationEventsBoss>().ResetAnimator();
            player.GetComponent<Player>().m_InvinceFrames = true;
            Debug.Log("Player died");
            player.GetComponent<Player>().m_lockon = false;

            //EntityStats.Instance.DeminishHealthOffEntity("Player", -1);

            player.GetComponent<Animator>().SetInteger("AnyState/Death", 1);
            boss.GetComponent<BossBrain>().enabled = false;
            boss.GetComponent<BossVariables>().m_activationScript.GetComponent<Collider>().isTrigger = true;
            boss.GetComponent<Animator>().SetInteger("CutScene", 2);
            player.GetComponent<Player>().m_Dead = true;
        }
    }

}
