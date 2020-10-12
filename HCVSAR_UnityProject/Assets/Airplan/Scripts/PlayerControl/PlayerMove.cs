using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    public float Speed = 0;
    public float smoothTime = 0.3F;

    public float Targetspeed = 0;
    float yVelocity = 0.0F;
 

    void Start () {

        Targetspeed = Speed;
	}

	void Update () {
    
        Speed = Mathf.SmoothDamp(Speed, Targetspeed, ref yVelocity, smoothTime);
        transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z+(Speed*Time.deltaTime));

    }

    public void MoveSpeed(float targetspeed, float smooth)
    {
        Targetspeed = targetspeed;
        smoothTime = smooth;
     
    }
}


