using UnityEngine;
using System.Collections;

public class Faller : EnemyAI {
	
	void Start(){
		transform.position = pl.PosToPoint(transform.position) ;
		onSphere = true ;
	}
	
	void Update(){
		targetPoint = transform.position - Vector3.up*Time.deltaTime*speed ;
		targetPoint = pl.PosToPoint(targetPoint) ;
		if(onSphere){
			MoveTo() ;
		}
		if(pl.GetHeight(transform.position) < -0.9 && onSphere){
			onSphere = false ;
			gameObject.AddComponent<Rigidbody>() ;
			StartCoroutine("Die") ;
		}
	}
	
	IEnumerator Die(){
		yield return new WaitForSeconds(3f) ;
		Destroy(gameObject) ;
	}
	
	// No use, for debugging
	void Reset(){
		rSpeed = 0 ;
		HP = 0 ;
	}
}
