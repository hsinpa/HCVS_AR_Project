using UnityEngine;
using System.Collections;

public class LvUp : MonoBehaviour
{
	Player pl ;	// Player script
	
	void Awake(){
		pl = Object.FindObjectOfType<Player>() ;
	}
	
	void Update(){
		transform.position = pl.PosToPoint(transform.position) ;	// Stick at ball
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pl.SpawnBody();
            Destroy(gameObject);
        }
    }
    
}