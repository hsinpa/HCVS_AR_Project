using UnityEngine;
using System.Collections;

public enum EnemyMode { Wander = 0, follow = 1, Snake = 2 }


public class EnemyGeneral : EnemyAI {



    public EnemyMode Mods = EnemyMode.Wander;
    public Vector3 initialPosition ;	// initial position on sphere
	public Vector3 initialDirection = new Vector3(1f, 0, 0) ;	// when on the sphere, first moving direction
	public AnimationCurve rotateCurve ;
    public AnimationCurve[] rotateCurves;
    public int keyOrder = 0;
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
            case EnemyMode.Wander:
                wander();
                break;

            case EnemyMode.follow:
                follow();
                break;
            case EnemyMode.Snake:
                Snake();
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

    public void Snake()
    {
        if (loop)
        {
            elapse = Mathf.Repeat(elapse + Time.deltaTime, rotateCurve.keys[rotateCurve.keys.Length - 1].time);
        }
        else
        {
            elapse += Time.deltaTime;
        }

        Vector3 point = transform.position + transform.forward; // move forward

        // Rotate 90 degree when the time reach the keyframe
        if (Mathf.Abs(elapse - rotateCurve.keys[keyOrder].time) < Time.deltaTime * 2)
        {
            float val = rotateCurve.Evaluate(rotateCurve.keys[keyOrder].time);
            Vector3 normal = pl.transform.position - transform.position;

            if (!(Mathf.Abs(val) <= 0.01))
            {
                point = transform.position + transform.right * Mathf.Sign(val);
                point = pl.PosToPoint(point);
                transform.rotation = Quaternion.LookRotation(point - transform.position, normal);
            }
            keyOrder++;
            if (keyOrder == rotateCurve.keys.Length - 1)
            {
                keyOrder = 0;
            }
        }

        // Move function
        targetPoint = pl.PosToPoint(point);
        MoveTo();
    }

    IEnumerator ModeChange(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        switch (Mods)
        {
            case EnemyMode.Wander:
                Mods = EnemyMode.follow;
                break;

            case EnemyMode.follow:
                Mods = EnemyMode.Wander;
                curveNumber = Random.Range(0, rotateCurves.Length);
                break;
        }
        StartCoroutine(ModeChange(10));
    }

    public override void OnTouchSphere(){
		targetPoint = transform.position + pl.transform.TransformDirection(initialDirection.normalized) ;
	}
}
