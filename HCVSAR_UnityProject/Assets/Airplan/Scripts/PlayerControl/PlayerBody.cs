using UnityEngine;
using System.Collections;
// This script attach to "PlayerBody" prefab

public class PlayerBody : MonoBehaviour {
	public GameObject bullet;	// bullet prefab
	public Transform[] Gun;	// gun point
	public float cd = 0.5f ;    // bullet cd time
    
    Player pl ;	// player script
	float bulletTime ;
	
	void Awake(){
		pl = Object.FindObjectOfType<Player>() ;
        gameObject.transform.parent = Game.moveall;
    }
	
	void Update () {
		
	}
	
	public void Shoot(){
		
			
                GameObject Bullet = Instantiate(bullet, Gun[0].transform.position, Gun[0].transform.rotation) as GameObject;
                if (pl.LM.monsters.Length > 0)
                {
                    if (pl.LM.monsters[0])
                        Bullet.GetComponent<BulletFollower>().Target = pl.LM.monsters[0].transform;
                }
        
	}
	
	void OnTriggerEnter(Collider co){
		// hit the bullet
		if(co.tag == "Bullet"){
			pl.Damage(transform) ;	// call damage function
			Destroy(co.gameObject) ;	// destory the bullet
		}
	}
}
