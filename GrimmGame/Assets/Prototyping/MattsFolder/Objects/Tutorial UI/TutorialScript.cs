using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerVerb
{
    Light_Attack,
    Heavy_Attack,
    Lockon,
    Dodge,
    Run
}

public class TutorialScript : MonoBehaviour
{
    //tutorial group vars
    public List<GameObject> tutGroupList; //list of the gameobjects that contain the tutorial grup buttons. Can be active or inactive.
    public Animator[][] animatorArray;
    [SerializeField]
    private int[] activeGroupAnimator;
    [SerializeField]
    private int activeGroup;

    //tutorial strings (not implementing a parser or writing a custom editor just make another here please)
    public PlayerVerb[][] tutVerbArray;
    public PlayerVerb[] tut1Verbs;
    public PlayerVerb[] tut2Verbs;
    public PlayerVerb[] tut3Verbs;

    //Player reference
    private Animator playerAnimator;

    //Non-generalised logic vars
    private bool[] tutGroupComplete;
    //public bool lockonComplete = false;
    //public bool gateComplete = false;
    //public bool dodgeComplete = false;


    //----
    // Antons additional shit
    // Get rid of it if you want
    [HideInInspector]
    public AnimEnums m_lastAnimation;
    
    // Start is called before the first frame update
    void Start()
    {
        //frankensteining tutVerb arrays together
        tutVerbArray = new PlayerVerb[][]
        {
            tut1Verbs,
            tut2Verbs,
            tut3Verbs
        };

        tutGroupComplete = new bool[3];

        #region get animator array
        animatorArray = new Animator[tutGroupList.Count][];

        for (int i = 0; i < tutGroupList.Count; i++)
        {
            var groupParent = tutGroupList[i].transform;

            var childCounter = 0;
            for (int k = 0; k < groupParent.childCount; k++)
            {
                if(groupParent.GetChild(k).tag == "UIbutton")
                {
                    childCounter++;
                }
            }
            var childNumber = childCounter;

            animatorArray[i] = new Animator[childNumber];

            int childIndex = 0;
            for (int j = 0; j < groupParent.childCount; j++)
            {
                Animator childAnimator = null;
                var child = groupParent.GetChild(j);

                if(child.tag == "UIbutton")
                {
                    childAnimator = child.GetComponent<Animator>();

                    if (childAnimator != null)
                    {
                        animatorArray[i][childIndex] = childAnimator;
                        childIndex++;
                    } else
                    {
                        Debug.Log("Missing animator on UI button");
                    }
                } 
            }
        }
        #endregion

        activeGroupAnimator = new int[tutGroupList.Count];

        activeGroup = -1; //-1 is a null value

        GameObject player = EntityStats.Instance.GetObjectOfEntity("Player");
        playerAnimator = player.GetComponent<Animator>();
    }

    public void Update()
    {
        //currentverb is the action whose button we're checking for
        if(activeGroup != -1)
        {
            var currentVerb = tutVerbArray[activeGroup][activeGroupAnimator[activeGroup]];
            var successful = false; //this is true if the current verb's condition was fulfilled

            

            switch (currentVerb)
            {
                case PlayerVerb.Light_Attack:
                    if (m_lastAnimation == AnimEnums.Light)
                    { TriggerCurrentButton(); successful = true; }
                    break;
                case PlayerVerb.Heavy_Attack:
                    if (m_lastAnimation == AnimEnums.Heavy)
                    { TriggerCurrentButton(); successful = true; }
                    break;
                case PlayerVerb.Lockon:
                    //replace K with some kind of actual reference
                    if (CameraShaker.Instance.cameraInfo.m_selectedCamera == 1)
                    { TriggerCurrentButton(); successful = true; }
                    break;
                case PlayerVerb.Dodge:
                    if (m_lastAnimation == AnimEnums.Dodge)
                    { TriggerCurrentButton(); successful = true; }
                    break;
                case PlayerVerb.Run:
                    if (m_lastAnimation == AnimEnums.Run)
                    { TriggerCurrentButton(); successful = true; }
                    break;
            }

            //replace this with input from the gate
            if (Input.GetKeyDown(KeyCode.O))
            { ProgressActiveAnimator(activeGroup); }

            if (successful)
            {
                if (activeGroupAnimator[activeGroup] == tutVerbArray[activeGroup].Length - 1)
                {
                    //if(activeGroup == 0 || activeGroup == 2)
                    //tutGroupComplete[activeGroup] = true;
                    tutGroupComplete[activeGroup] = true;
                }
            }
        }
    }

    public void TriggerCurrentButton()
    {
        animatorArray[activeGroup][activeGroupAnimator[activeGroup]].SetBool("Pressed", true);
    }

    public void UntriggerCurrentButton()
    {
        animatorArray[activeGroup][activeGroupAnimator[activeGroup]].SetBool("Pressed", false);
    }

    public void PressedAnimationOver()
    {
        if(tutGroupComplete[activeGroup])
        {
            var currentActiveGroup = activeGroup;

            DisableTutGroup(currentActiveGroup);
            if (currentActiveGroup == 0 && tutGroupComplete[1] == false)
            {
                EnableTutGroup(1);
            }
        }
    }

    public void ProgressActiveAnimator(int tutorialGroup)
    {
        if (activeGroupAnimator[activeGroup] < animatorArray[activeGroup].Length)
        {
            activeGroupAnimator[activeGroup]++;
        } else
        {
            Debug.Log("Active animator advanced outside expected range");
        }
    }

    public void ResetActiveAnimator(int tutorialGroup)
    {
        activeGroupAnimator[tutorialGroup] = 0;
    }

    public void EnableTutGroup(int index)
    {
        tutGroupList[index].SetActive(true);
        activeGroup = index;
        Debug.Log("enabled group");
    }

    public void DisableTutGroup(int index)
    {
        tutGroupList[index].SetActive(false);
        activeGroup = -1; // null value
        Debug.Log("disabled group");
    }

    public void GateTriggerEnter()
    {
        if(!tutGroupComplete[0] && activeGroup != 0)
        {
            EnableTutGroup(0);
        } else if(!tutGroupComplete[1] && activeGroup != 1)
        {
            EnableTutGroup(1);
        }
    }

    public void GateTriggerExit()
    {
        int failsafe = 0; //this checks if nothing was disabled
        if(tutGroupList[0].activeSelf)
        {
            DisableTutGroup(0);
            failsafe++;
        }
        if (tutGroupList[1].activeSelf)
        {
            DisableTutGroup(1);
            failsafe++;
        }
        if(failsafe == 0)
        {
            Debug.Log("Trigger was exited, nothing was disabled");
        }
    }




}
