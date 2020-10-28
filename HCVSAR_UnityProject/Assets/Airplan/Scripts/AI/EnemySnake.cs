using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : EnemyWanderer {
	
	public int keyOrder = 0 ;   // The keyframe we passed

   
    void Start(){
		targetPoint = pl.DirToPoint(initialPosition) ;
	}
	
	public override void Update(){
		// Repeat elapse time
        if(loop){
        	elapse = Mathf.Repeat(elapse+Time.deltaTime, rotateCurve.keys[rotateCurve.keys.Length-1].time) ;
        }else{
			elapse += Time.deltaTime ;
		}
        
        Vector3 point = transform.position + transform.forward ; // move forward
        
        // Rotate 90 degree when the time reach the keyframe
        if(Mathf.Abs(elapse-rotateCurve.keys[keyOrder].time) < Time.deltaTime*2){
        	float val = rotateCurve.Evaluate(rotateCurve.keys[keyOrder].time) ;
        	Vector3 normal = pl.transform.position - transform.position;
        	
        	if(!(Mathf.Abs(val) <= 0.01) ){
				point = transform.position + transform.right*Mathf.Sign(val) ;
				point = pl.PosToPoint(point) ;
				transform.rotation = Quaternion.LookRotation(point - transform.position, normal) ;
			}
			keyOrder ++ ;
			if(keyOrder == rotateCurve.keys.Length-1){
				keyOrder = 0 ;
			}
		}
		
        // Move function
        targetPoint = pl.PosToPoint(point) ;
		MoveTo() ;
	}
	
	public override void OnTouchSphere(){
		targetPoint = transform.position + pl.transform.TransformDirection(initialDirection.normalized) ;
	}
}
