using UnityEngine;
using System.Collections;

public class AIfort : MonoBehaviour {

    //public GameObject kk;

    public LockMonter LM;
    public Transform Target;
    public float range;

    public Transform Barrel;
    public GameObject B_bullet;
    public GameObject B_bulletFire;
    public ParticleSystem fx_muzzleshot;
    public Transform[] FireObjs;
    public float B_bulletSpeed;
    public float CD;
    float fire_CD_Time;
    public bool open = true;

    public float hp= 100;
    int LMnum=0;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.O))
        {
            open = open == true ? false : true;
        }

        if (LM.monsters.Length > 0&&open==true)
        {
            if (LM.monsters[0])
            {
                Target = LM.monsters[LMnum].transform;
                Vector3 tn = new Vector3(Target.position.x, Target.position.y + 1, Target.position.z);
                Barrel.LookAt(tn);
                if (Ray())
                {
                    Fire(FireObjs[Random.Range(0,FireObjs.Length)]);
                    Debug.Log("fire");
                }
                else
                {
                    Debug.Log("change");
                    if (LM.monsters.Length-1 > LMnum)
                    {
                        LMnum++;
                    }
                    else
                    {
                        LMnum = 0;
                    }
                    
                }
            }
        }
        
    }

    bool Ray()
    {
        RaycastHit hit;

        if (Physics.Raycast(Barrel.position, Barrel.forward, out hit, 200f, 1023))
        {
           // kk = hit.collider.gameObject;
            if(hit.collider.tag == "Boss")
            {
                return true;
            }else
            if (Vector3.Distance(Target.position, Barrel.position) > Vector3.Distance(Barrel.position, hit.point))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        else
        {
            return true;
        }
        
    }

    public void UnderAttack(int damage) {
		hp -= damage;
		if (hp <= 0) GameObject.Destroy(gameObject);
    }

    public void Fire(Transform B_FireObj)
    {
        if (Time.time > fire_CD_Time )
        {
           
            fire_CD_Time = Time.time + CD;


            GameObject bulle = Instantiate(B_bullet, B_FireObj.transform.position, B_FireObj.transform.rotation) as GameObject;
            GameObject bulle1 = Instantiate(B_bulletFire, B_FireObj.transform.position, B_FireObj.transform.rotation) as GameObject;
            bulle.GetComponent<Rigidbody>().AddForce((B_FireObj.transform.forward + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0)) * B_bulletSpeed);
            /*foreach (Transform B_FireObj in FireObjs)
            {
                GameObject bulle = Instantiate(B_bullet, B_FireObj.transform.position, B_FireObj.transform.rotation) as GameObject;
                bulle.GetComponent<Rigidbody>().AddForce((B_FireObj.transform.forward + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.1f, 0.1f), 0)) * B_bulletSpeed);
                //  fx_muzzleshot.transform.position.x = fire_point[fire_point_index].position.x;
                // fx_muzzleshot.transform.position.y = fire_point[fire_point_index].position.y;
                // if (SHOWING_FX)
                //fx_muzzleshot.Emit(2);
            }*/
        }
        
    }

}
