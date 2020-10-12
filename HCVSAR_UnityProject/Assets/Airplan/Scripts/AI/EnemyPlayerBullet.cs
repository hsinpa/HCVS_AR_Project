using UnityEngine;
using System.Collections;

public class EnemyPlayerBullet : EnemyAI {
	public Vector3 initialPosition ;	// initial position on sphere
	public Vector3 initialDirection = new Vector3(1f, 0, 0) ;	// when on the sphere, first moving direction
	public AnimationCurve rotateCurve ; // rotate curve

    public float DestroyTime = 3;
    public int Damage = 1;
    public bool Through = false;
    //public GameObject[] Effect;
    float elapse = 0f ;  // time for rotate curve
    

    void Start(){
        gameObject.transform.parent = Game.moveall;
        Destroy(gameObject, DestroyTime);
        targetPoint = pl.DirToPoint(initialPosition);
    }
	
	void Update(){
		// When on sphere
		
			elapse += Time.deltaTime ; // elapse time ++
			Vector3 point = transform.position + transform.forward - pl.transform.position ; // move forward
			point += transform.right*rotateCurve.Evaluate(elapse)*0.6f ; // get curve value, rotate via curve
			// calculate point
			targetPoint = pl.DirToPoint(point) ;
		
		MoveTo() ;
     
    }
	
	public override void OnTouchSphere(){
		targetPoint = transform.position + pl.transform.TransformDirection(initialDirection.normalized) ;
	}




    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            other.GetComponent<EnemyAI>().BeHurt(Damage);
            if (Through == false)
            {
                Destroy(gameObject);
                
            }
            Dead();
            // Instantiate(Effect[Random.Range(0,Effect.Length)], transform.position, transform.rotation);
        }
    }
}
