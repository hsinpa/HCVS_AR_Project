using UnityEngine;
using UnityEngine.UI ;
using System.Collections;

public class ScreenUI : MonoBehaviour {
	public Image black ;	// image component
	float speed = -1f ;		// fade out speed
	
	void Awake(){
		FadeIn(true) ;
	}
	
	void Update(){
		// fade update
		Color c = black.color ;
		c.a += speed*Time.deltaTime ;
		c.a = Mathf.Clamp01(c.a) ;
		black.color = c ;
	}
	
	// Call Fade
	void FadeIn(bool force=false){
		if(force){
			Color c = black.color ;
			c.a = 1 ;
			black.color = c ;
		}
		speed = -Mathf.Abs(speed) ;
	}
	
	void FadeOut(bool force=true){
		if(force){
			Color c = black.color ;
			c.a = 0 ;
			black.color = c ;
		}
		speed = Mathf.Abs(speed) ;
	}
}
