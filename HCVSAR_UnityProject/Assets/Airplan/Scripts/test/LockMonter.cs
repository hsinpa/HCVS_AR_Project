using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LockMonter : MonoBehaviour
{
    public GameObject[] monsters;

    public ArrayList mm = new ArrayList();

    void Start()
    {
        // mm.Add(gameObject);
        // monters = mm.ToArray()as GameObject[];
        InvokeRepeating("check", 0.5f, 0.5f);
    }

    void Update()
    {
        monsters = new GameObject[mm.Count];
        mm.CopyTo(monsters);
    }

    void check()
    {
        foreach (GameObject monster in monsters)
        {
            if (!monster)
            {
                mm.Remove(monster);
            }
            else
            {
                if (!monster.GetComponent<EnemyAI>())
                {
                    mm.Remove(monster);
                }
            }

        }
    }
    //=========
    // Trigger
    //=========
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            if(other.GetComponent<EnemyAI>())
            mm.Add(other.gameObject);
        }

    }

    void OnTriggerExit(Collider other)
    {

        if (other.tag == "Monster")
        {
            if (other.GetComponent<EnemyAI>())
                mm.Remove(other.gameObject);
        }

    }


}
