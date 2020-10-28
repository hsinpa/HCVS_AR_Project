using UnityEngine;
using System.Collections;

public class Playerbullet : MonoBehaviour {
    public float DestroyTime = 3;
    public int Damage = 1;
    public bool Through =false;
    public GameObject Effect;
    void Start () {
        gameObject.transform.parent = Game.moveall;
        Destroy(gameObject, DestroyTime);
	}
	
	
	void Update () {
        transform.position += transform.forward*5*Time.deltaTime;
	}

    

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            other.GetComponent<EnemyAI>().BeHurt(Damage);
            if (Through == false)
            {
                Destroy(gameObject);
                GameObject effect = Instantiate(Effect, transform.position, transform.rotation) as GameObject;
                effect.transform.parent = Game.moveall;
            }
        }
    }

}
