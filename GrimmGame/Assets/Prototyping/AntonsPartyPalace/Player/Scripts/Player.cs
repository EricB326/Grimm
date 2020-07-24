using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A placeholder till input buffer is completed
// Wasd controls passed to animator.


public class Player : MonoBehaviour
{
   

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

}



