using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    

     // Stores dettected input.
    //public List<InputBuffer> m_inputBuffer =  new List<InputBuffer>();

    private Animator m_characterAnim;

    private void Start()
    {
        m_characterAnim = this.GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        float x = 0;
        float y = 0;
        bool attack = false;
        // Inputs not stored should be 
        if (Input.GetKey(KeyCode.W))
        {
            y++;
        }
        if(Input.GetKey(KeyCode.S))
        {
            y--;
        }
        if (Input.GetKey(KeyCode.A))
        {
            x--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            x++;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            attack = true;
        }

        UpdateAnimations(x, y, attack);

    }


    // Send inputs to animator
    void UpdateAnimations(float a_x, float a_y, bool a_attack)
    {

        // x axis
        m_characterAnim.SetFloat("Input/X", a_x);
        // y axis
        m_characterAnim.SetFloat("Input/Z", a_y);

        if (a_attack)
            m_characterAnim.SetTrigger("Input/Attack");

    }

    // EveryFrame animation occurs.
    // Occurs after
    private void OnAnimatorMove()
    {
        // Note that you can get information in and out of the animator.
        // Useful for logic
        //m_characterAnim.GetFloat("");
     
    }

}



