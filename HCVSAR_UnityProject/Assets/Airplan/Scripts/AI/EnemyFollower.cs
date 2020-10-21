using UnityEngine;
using System.Collections;

public class EnemyFollower : EnemyAI {

   void Start()
    {
        targetPoint = pl.transform.position;
        Player.monsterQuantity++;
    }

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
