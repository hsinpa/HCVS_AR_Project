using UnityEngine;
using System.Collections;

public class MinGun : EnemyAI
{
    public GameObject p;
    public EnemyAI main;
    public bool look=true;
    // Use this for initialization
    void Start () {

        p = pl.dragonHead.gameObject;
        
       // onSphere = true;
	}
	
	// Update is called once per frame
	void Update () {
        onSphere = main.onSphere;
        if(look)
        transform.LookAt(p.transform.position);
	}
}
