//====================
// Manage all item
//====================
using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	
	void OnTriggerEnter(Collider co){
		if(co.tag != "Player"){return;}
		//Destroy(gameObject) ;
	}
}
