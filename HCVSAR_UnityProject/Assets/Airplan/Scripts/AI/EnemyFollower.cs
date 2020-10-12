using UnityEngine;
using System.Collections;

public class EnemyFollower : EnemyAI {

   /* void Start()
    {
        targetPoint = pl.transform.position;
    }*/

    void Update(){
        if (onSphere)
        {
            targetPoint = pl.dragonHead.position;
        }
        else
        {
            targetPoint = pl.transform.position;
        }
		MoveTo() ;
	}
	
}
