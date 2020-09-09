using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Game07 : MonoBehaviour
{
    public UnityEvent StartEvent;
    public UnityEvent unityEvent;
    
    public UnityEvent OverEvent;
    
    public Transform t;
    public Transform t2;
    public Image image;
    int v = 1;
    public Animator ani;
    public float speed = -2;
    public bool OK;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UI_Start()
    {

        StartEvent.Invoke();

    }
    public void UI_Enter()
    {
        //ani.speed = 0;
        if (Mathf.Abs(t.localPosition.x)<0.5)
        {
            
            OK = true;
            ani.SetBool("OK", true);
            Invoke("right", 4);
        }
        else
        {
            OverEvent.Invoke();
            ani.speed = 0;
        }
      
    }
    void right()
    {
        unityEvent.Invoke();
    }
    void Update()
    {
        if (OK)
        {
            
            if (t.localPosition.y>0)
            {
                //t.localPosition += new Vector3(0, Time.deltaTime * speed, 0);
            }
            else
            {
                
            }
        }
    }
}
