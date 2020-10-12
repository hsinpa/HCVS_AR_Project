using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dtime : MonoBehaviour {
    public float time =3;
	// Use this for initialization
	void Start () {
        Destroy(gameObject,time);
	}
	
}
