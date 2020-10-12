using UnityEngine;
using System.Collections;

public class EnemyWanderer : EnemyAI {
	public Vector3 initialPosition ;	// initial position on sphere
	public Vector3 initialDirection = new Vector3(1f, 0, 0) ;	// when on the sphere, first moving direction
	public AnimationCurve rotateCurve ; // rotate curve
	public bool loop = true ;		// loop curve?
    internal float elapse = 0f ;     // time for rotate curve

   

    void Start(){
		targetPoint = pl.DirToPoint(initialPosition) ;
	}
	
	public virtual void Update(){
        // Repeat elapse time
        if (onSphere)
        {
            if (loop)
            {
                elapse = Mathf.Repeat(elapse + Time.deltaTime, rotateCurve.keys[rotateCurve.keys.Length - 1].time);
            }
            else
            {
                elapse += Time.deltaTime;
            }

            Vector3 point = transform.position + transform.forward - pl.transform.position; // move forward
            point += transform.right * rotateCurve.Evaluate(elapse) * 0.6f; // get curve value, rotate via curve
            targetPoint = pl.DirToPoint(point);                                                    // calculate point



        }
        else
        {
            targetPoint = pl.DirToPoint(initialPosition);
        }
        
        MoveTo();
    }
	
	public override void OnTouchSphere(){
		targetPoint = transform.position + pl.transform.TransformDirection(initialDirection.normalized) ;
	}
}
