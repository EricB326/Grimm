using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockonMarker : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = EntityStats.Instance.GetObjectOfEntity("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<Player>().m_lockon)
        {
            this.transform.LookAt(player.transform.position);
        }
    }
}
