using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BulletFollower : EnemyAI {
    public Transform Target;
    public Vector3 initialPosition; // initial position on sphere
    public Vector3 initialDirection = new Vector3(1f, 0, 0);    // when on the sphere, first moving direction
    public AnimationCurve rotateCurve;  // rotate curve
    public RectTransform lockUI;
    
    void Awake()
    {
        pl = Object.FindObjectOfType<Player>();

        gameObject.transform.parent = Game.moveall;
    }

    void Start()
    {
        targetPoint = pl.DirToPoint(initialPosition);
    }

    void Update(){
        if (Target)
        {
            targetPoint = Target.position;

            lockUI.position = pl.C.WorldToScreenPoint(Target.position);
            lockUI.gameObject.SetActive(true);
            MoveTo();
        }
        else
        {
            // When on sphere
            if (onSphere)
            {
                elapse += Time.deltaTime; // elapse time ++
                Vector3 point = transform.position + transform.forward - pl.transform.position; // move forward
                point += transform.right * rotateCurve.Evaluate(elapse) * 0.6f; // get curve value, rotate via curve
                                                                                // calculate point
                targetPoint = pl.DirToPoint(point);
            }
            lockUI.gameObject.SetActive(false);
            MoveTo();
        }
        //lockUI.position = pl.C.WorldToScreenPoint();
    }
   

    float elapse = 0f;   // time for rotate curve


    public void FollowTarget(Transform target)
    {
        
    }
  
    public override void OnTouchSphere()
    {
        targetPoint = transform.position + pl.transform.TransformDirection(initialDirection.normalized);
    }
}
