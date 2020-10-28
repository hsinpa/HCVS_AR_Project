using UnityEngine;
using System.Collections;

public class launcher1 : MonoBehaviour {
    EnemyAI enemyAI;

    public Transform target;//目標
    public Transform child;
    [Header("子彈物件")]
    public GameObject B_bullet;
    [Header("射擊模式")]
    public ModeState Mode = ModeState.Ring;    
    [Header("相關參數")]
    public float B_angle=360;//發射擴散角度
	public int B_ring_quantity = 12;//單次發測量
	public int B_loop_Qt = 6;//總發射次數
    public int bout = 10; //執行次數
    public float B_CD = 1;//CD時間
    public float L_CD = 0; //CD
    public float delay = 0;//等待時間
    int loopS = 0;
    float x;
    

    [Header("怪物路徑")]
    public bool replace = false;
    public Vector3 initialPosition; // initial position on sphere
    public Vector3 initialDirection = new Vector3(1f, 0, 0);    // when on the sphere, first moving direction
    public AnimationCurve rotateCurve; // rotate curve


    float Tt;
	float qr;
    Player pl;

	void Start () {

        child = new GameObject("axis").GetComponent<Transform>();      
        child.position = transform.position;
        child.rotation = transform.rotation;
        child.parent = transform;

        //enemyAI = GetComponent<EnemyAI>();
        /*switch (Mode)
        {
            case ModeState.Create:
                enemyAI = GetComponent<EnemyAI>();
                break;
            case ModeState.Ring:
                enemyAI = GetComponent<EnemyAI>();
                break;
            case ModeState.Swirl:
                enemyAI = GetComponent<EnemyAI>();
                break;
            case ModeState.Line:
                enemyAI = GetComponentInParent<EnemyAI>();
                break;

        }*/


        qr = B_angle / B_ring_quantity;

        Tt = Time.time + delay;
        pl = Object.FindObjectOfType<Player>();
    }

	void Update () {
        if (Player.monsterQuantity<6) 
        {
            Create();
        }

    }

	public void Fire(Vector3 pos , Quaternion rota){
        if (B_loop_Qt > 0 && Time.time > Tt)
        {
            GameObject bullet1 = Instantiate (B_bullet, pos, rota) as GameObject;
            bullet1.transform.parent = Game.moveall;
            if (replace == true)
            {
                EnemyWanderer w;
                w = bullet1.GetComponent<EnemyWanderer>();
                w.initialPosition=initialPosition;
                w.initialDirection = initialDirection;
                w.rotateCurve = rotateCurve;
            }
            B_loop_Qt--;
            Tt = Time.time + B_CD;
        }
    }

    public void Create()
    {
             
            Fire(transform.position, transform.rotation);
      
    }

   


}
