using UnityEngine;
using System.Collections;

public class EnemyFollowFriend : EnemyAI {

    public Transform friend;
    public Vector3 fpos; //friend position
    float cd;

 

    void Start()
    {
        fpos = friend.position;
    }


    void Update(){
		targetPoint = fpos;
		MoveTo() ;
        if (cd <= 0)
        {
            fpos = friend.position;
            cd = 1;
        }
        else
        {
            cd -= 1 * Time.deltaTime;
        }
	}
	
}
