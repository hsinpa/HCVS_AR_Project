using UnityEngine;
using System.Collections;

public class DragonHead : MonoBehaviour {
	public Transform[] joint ;			
	public DragonBody[] bodies ;
	float delayTime = 10f ;
	Player pl ;
	
	void Awake(){
		pl = Object.FindObjectOfType<Player>() ;
		
	}
	
	void Update(){
		// tails follow
		bodies = pl.myBodies.ToArray() ;
		for(int i=1 ; i<bodies.Length ; i++){
			DelayFollow(i) ;
		}
	}
	
	void DelayFollow(int id){
		
		DragonBody me = bodies[id] ;
		DragonBody pre = bodies[id-1] ;
		// position
		Vector3 tarPos = pre.body.position - pre.body.forward*me.distance ;
		//tarPos = pl.PosToPoint(tarPos) + joint[id].TransformDirection(joint[id].localPosition) ;
		me.body.position = Vector3.Lerp(me.body.position, tarPos, delayTime*Time.deltaTime) ;
		// rotation
		Quaternion rot = joint[id-1].rotation ;
		me.body.rotation = Quaternion.Slerp(me.body.rotation, rot, delayTime*Time.deltaTime) ;
	}
}
