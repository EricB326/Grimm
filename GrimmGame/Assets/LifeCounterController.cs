using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class LifeCounterController : MonoBehaviour
{
    //public int phaseAmount = 0;
    public GameObject featherParent = null;
    public GameObject originalFeather = null;
    public float featherOffsetX = 0f;

    private List<GameObject> newFeathers = new List<GameObject>();

    private BossBrain bwain;

    // Start is called before the first frame update
    void Start()
    {
        bwain = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>();

        Vector2 lastPos = Vector2.zero;

        for (int i = 0; i < bwain.m_bossPhaseList.Count; i++)
        {
            newFeathers.Add(Instantiate(originalFeather, featherParent.transform));

            string newName = "Feather0" + (i + 1);
            newFeathers[i].name = newName;

            if (newFeathers.Count > 1)
                newFeathers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(lastPos.x + featherOffsetX, 0);

            lastPos = newFeathers[i].GetComponent<RectTransform>().anchoredPosition;
        }

        EntityStats.Instance.onPlayerLifeLost += EntityStats_OnPlayerLifeLost;
        EntityStats.Instance.onBossPhaseChange += EntityStats_OnBossPhaseChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Destroy(newFeathers[newFeathers.Count - 1].gameObject);
            newFeathers.RemoveAt(newFeathers.Count - 1);
        }
    }

    private void EntityStats_OnPlayerLifeLost(object sender, System.EventArgs e)
    {
        
    }

    private void EntityStats_OnBossPhaseChange(object sender, System.EventArgs e)
    {
        Destroy(newFeathers[newFeathers.Count - 1].gameObject);
        newFeathers.RemoveAt(newFeathers.Count - 1);
    }
}
