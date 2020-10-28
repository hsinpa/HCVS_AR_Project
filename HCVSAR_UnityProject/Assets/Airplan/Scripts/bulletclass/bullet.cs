using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
    public float DestroyTime = 3;

    void Start () {
        Destroy(gameObject, DestroyTime);
	}
	
	
	void Update () {
        //transform.position += transform.forward*5*Time.deltaTime;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
        
            //Destroy(gameObject);
        }
    }

}
