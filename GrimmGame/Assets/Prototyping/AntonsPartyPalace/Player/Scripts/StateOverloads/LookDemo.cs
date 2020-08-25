using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDemo : StateMachineBehaviour
{

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float ikWeight = animator.GetFloat("LookWeight");
        // Probably better for lock on and not running.
        // Direction isn't really needed.
        Vector3 direction = EntityStats.Instance.GetObjectOfEntity("Boss").transform.position 
            - animator.gameObject.transform.position;
        if (animator.gameObject.GetComponentInParent<Player>().m_lockon)
        {
            ikWeight += 0.05f;
            if (ikWeight > 1)
            {
                ikWeight = 1;
            }
            animator.SetLookAtWeight(ikWeight, 0.55f, 0.8f, 1.0f);
            // This should be target
            Vector3 targetPos = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossVariables>().m_facingPosition.transform.position;
            //targetPos.y -= 1f;
            animator.SetLookAtPosition(targetPos);
        }
        else
        {
            ikWeight -= 0.1f;
            if (ikWeight < 0)
            {
                ikWeight = 0;
            }
            animator.SetLookAtWeight(ikWeight, 0.55f, 0.8f, 1.0f);
            Vector3 targetPos = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossVariables>().m_facingPosition.transform.position;
            //targetPos.y += 1;
            animator.SetLookAtPosition(targetPos);
        }
        animator.SetFloat("LookWeight", ikWeight);

        // Could this be used in ik to move the body to the correct
        // place when legs are applying Ik?
        //animator.MatchTarget(AvatarTarget.Body);
    }

}
