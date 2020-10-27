using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerFire : MonoBehaviour {
    public GameObject bullet;
    public Transform Gun;
    public float MainCD;
    public int HP;
    float bulletTime;
    public float cd = 0.5f;
    public bool L;
    int mb=1;
    public delegate void gameOver();
    public static event gameOver og;
    public launcher1 launcher;
    public GameObject OverUI;
    public GameObject fp;
    public GameObject p;
    //public Transform[] T;

    float Tt;
    Player pl;  // player script
   
    void Awake()
    {
        pl = Object.FindObjectOfType<Player>();
    }

    void Update () {
        if (Input.GetButton("Fire1")&&Time.time>=Tt)
        {
            GameObject Bullet = Instantiate(bullet, Gun.transform.position, Gun.transform.rotation) as GameObject;         
            Tt = Time.time + MainCD;
        }

        if (Time.time >= bulletTime&&pl.LM.monsters.Length>0&&L ==true)
        {
            int num =0;
            foreach (DragonBody D in pl.myBodies)
            {
                if (num > 0)
                {
                    D.body.gameObject.GetComponent<PlayerBody>().Shoot();
                }
                else
                {
                    num++;
                }

            }
            
            bulletTime = Time.time + cd;
        }

        if (Time.time >= bulletTime && pl.LM.monsters.Length > 0&&L==false)
        {
            if (mb < pl.myBodies.Count)
            {
                pl.myBodies[mb].body.gameObject.GetComponent<PlayerBody>().Shoot();
                mb++;
            }
            else
            {
                mb = 1;
            }
            bulletTime = Time.time + cd;
        }

    }

        void OnTriggerEnter(Collider co)
    {
        //print("0"+pl.myBodies.Count);
        // hit the bullet
        if (co.tag == "Bullet")
        {
            launcher.gameObject.SetActive(false);
            og();
            Instantiate(fp, p.transform.position, p.transform.rotation);
            p.SetActive(false);
            OverUI.SetActive(true);
            if (pl.myBodies.Count>=2)
            {
              //  pl.Damage(pl.myBodies[1].body);
                
            }
            else
            {
              //  Destroy(gameObject);
              //  SceneManager.LoadScene("DemoScene", LoadSceneMode.Single);
            } // call damage function
            
            Destroy(co.gameObject); // destory the bullet
        }
    }


    public void UI_re()
    {
        p.SetActive(true);
        Player.monsterQuantity = 0;
        pl.KillMonster = -1;
        pl.KillM();
        launcher.gameObject.SetActive(true);
    }

}
