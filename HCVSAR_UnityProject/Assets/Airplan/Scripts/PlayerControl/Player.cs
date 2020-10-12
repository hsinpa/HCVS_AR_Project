using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using UnityEngine.UI;
public class Player : MonoBehaviour {
	public Transform dragonHead ;	// the dragon head to control
	
	[Header("戰鬥資訊")]
	public float invinTime = 1f ;	// invincibility time cd
	bool invin = false ;	// is invincible?
    public List<DragonBody> myBodies = new List<DragonBody>() ;	// current bodies
    public LockMonter LM; //敵人鎖定
    public Camera C;
    public Transform tl;
    static public int monsterQuantity;
	int KillMonster;
	public Text KillUI;

	[Header("移動數值")]
	public float lookSpeed = 5f ;	// camera look speed
	public float flySpeed = 12f ;	// dragon fly speed
	
	[Header("生成物件")]
	public GameObject bodyObject ;	// dragon body object
	public GameObject itemSample ;	// item
    
	// Sphere
	[Header("每多一節，球的縮放比例")]
	public float ballScaleRate = 0.14f ;
	internal SphereCollider sphere ;	// player sphere collider
	float ballRadius ;
	
	// target value (for better movement)
	Quaternion look ;
    Transform targetPoint ;

	public static Player main;
	public void KillM()
    {
		KillMonster++;
		KillUI.text = KillMonster.ToString();
    }

    void Awake(){
		main = this;
		Game.ShowCursor(false) ;
		sphere = GetComponent<SphereCollider>() ;
		ballRadius = sphere.radius ;
		targetPoint = transform.Find("TargetPoint").GetChild(0) ;

    }
	
	void Start(){
		InitBody() ; 
		//for(int i=0 ; i<4 ; i++){SpawnBody();}  // Spawn 4 body (test only)
	}
	
	void Update(){
		MoveDragon() ;       
        //CameraFollow() ;
		ScaleBall() ;   // Sphere size control
                        // Drop item (Test only)
                        /*if(Input.GetKeyDown(KeyCode.D)){
                            Instantiate(itemSample, dragonHead.position, Quaternion.identity) ;
                        }*/
        //tl.transform.position = LM.monsters[0].transform.position;
	}
	//==========
	// Battle
	//==========
	// damage a body
	public void Damage(Transform bone){
        // search a body to destroy
        bool A = false;
		foreach(DragonBody b in myBodies){
            if (A == true)
            {
                myBodies.Remove(b);
                Destroy(b.body.gameObject);
            }else
            if (b.body == bone){             
				myBodies.Remove(b) ;
				Destroy(b.body.gameObject) ;
                A = true;
				break ;
			}
            
        }
		// set invinciable timer
		StartCoroutine("Invinciable") ;
	}
	// invin time count down
	IEnumerator Invinciable(){
		invin = true ;
		yield return new WaitForSeconds(invinTime) ;
		invin = false ;
	}
	//=========
	// Sphere
	//=========
	void ScaleBall(){
		sphere.radius = Mathf.Lerp(sphere.radius, ballRadius*(1+ballScaleRate*(myBodies.Count-1)),13f*Time.deltaTime) ;
	}
	
	//========
	// Body
	//========
	// spawn a new body
	public void SpawnBody(){
		//Transform b = Instantiate(bodyObject).transform ;
		//DragonBody dBody = new DragonBody() ;
		//dBody.body = b ;
		//myBodies.Add(dBody) ;
	}
	// Init myBodies (add DragonHead to first body), DO NOT modify this
	void InitBody(){
		DragonBody dBody = new DragonBody() ;
		dBody.body = dragonHead ;
		myBodies.Add(dBody) ;
	}
	
	//============
	// Movement
	//============
	// Camera follow
	void CameraFollow(){
		Transform t = Camera.main.transform ;
		t.rotation = Quaternion.Slerp(t.rotation, Quaternion.LookRotation(dragonHead.position-t.position), lookSpeed*Time.deltaTime) ;
	}
	// Move the dragon
	void MoveDragon(){
		float h = Input.GetAxis("Mouse X") ;
		float v = Input.GetAxis("Mouse Y") ;      
        targetPoint.localPosition = new Vector3(0,0,sphere.radius) ;
		//targetPoint.parent.Rotate(Camera.main.transform.TransformDirection(new Vector3(-v,h,0)*Time.deltaTime*flySpeed*2), Space.World) ;
		
		// Set position
		Vector3 realPos = PosToPoint(targetPoint.position) ;
		dragonHead.position = Vector3.Lerp(dragonHead.position, realPos, flySpeed*Time.deltaTime*2f) ;
		dragonHead.position = PosToPoint(dragonHead.position) ;	// attach to ball
		dragonHead.rotation = Quaternion.Slerp(dragonHead.rotation, look, 10f*Time.deltaTime) ;
		// If move, rotate the head to face the movement
		if(Vector3.Distance(targetPoint.position,dragonHead.position) >= 0.08f){
			Vector3 normal = Vector3.Lerp(Vector3.up, new Vector3(0.5f,0.5f,0), dragonHead.position.y/sphere.radius) ;
			look = Quaternion.LookRotation(targetPoint.position-dragonHead.position, normal) ;
		}
	}

    

    //==================
    // Useful function
    //==================
    // convert direction to ball point
    public Vector3 DirToPoint(Vector3 dir){
		dir = dir.normalized*sphere.radius ;
		return transform.position + dir ;
	}
	// convert position to ball point
	public Vector3 PosToPoint(Vector3 pos){
		Vector3 dir = pos - transform.position ;
		return dir.normalized*sphere.radius+transform.position ;
	}
	// Return height relative to player
	public float GetHeight(Vector3 pos){
		return (pos-transform.position).y/sphere.radius ;
	}

}
