using UnityEngine;
using System.Collections;

public class EnemyBoss : EnemyAI {

    public enum ModeState {Wander,follow};
    public ModeState Mods = ModeState.Wander;
    public Vector3 initialPosition ;	// initial position on sphere
	public Vector3 initialDirection = new Vector3(1f, 0, 0) ;	// when on the sphere, first moving direction
	public AnimationCurve rotateCurve ;
    public AnimationCurve[] rotateCurves;
    int curveNumber = 0;

    public bool loop = true ;		// loop curve
    internal float elapse = 0f ;     // time for rotate curve



    void Start(){
		targetPoint = pl.DirToPoint(initialPosition) ;
        StartCoroutine(ModeChange(10));
        
    }
	
	public virtual void Update(){
        switch (Mods)
        {
            case ModeState.Wander:
                wander();
                break;

            case ModeState.follow:
                follow();
                break;
        }
       
    }
	
    public void wander()
    {
        if (onSphere)
        {
            if (loop)
            {
                elapse = Mathf.Repeat(elapse + Time.deltaTime, rotateCurves[curveNumber].keys[rotateCurves[curveNumber].keys.Length - 1].time);
            }
            else
            {
                elapse += Time.deltaTime;
            }
            Vector3 point = transform.position + transform.forward - pl.transform.position; // move forward
            point += transform.right * rotateCurves[curveNumber].Evaluate(elapse) * 0.6f; // get curve value, rotate via curve
            targetPoint = pl.DirToPoint(point);                                                    // calculate point
        }

        MoveTo();
    }

    public void follow()
    {
        targetPoint = pl.dragonHead.position;
        MoveTo();
    }

    IEnumerator ModeChange(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        switch (Mods)
        {
            case ModeState.Wander:
                Mods = ModeState.follow;
                break;

            case ModeState.follow:
                Mods = ModeState.Wander;
                curveNumber = Random.Range(0, rotateCurves.Length);
                break;
        }
        StartCoroutine(ModeChange(10));
    }

    public override void OnTouchSphere(){
		targetPoint = transform.position + pl.transform.TransformDirection(initialDirection.normalized) ;
	}
}
