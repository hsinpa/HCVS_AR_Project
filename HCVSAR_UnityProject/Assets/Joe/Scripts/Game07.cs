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
    public Transform t3;
    public Image image;
    int v = 1;
    public Animator ani;
    public float speed = -2;
    public bool OK;

    public GameObject sg;
    public GameObject eg;
    // Start is called before the first frame update
    void Start()
    {
        t.gameObject.SetActive(true);
        t2.gameObject.SetActive(true);
        t3.gameObject.SetActive(true);
    }

    // Update is called once per frame
    public void UI_Start()
    {

        StartEvent.Invoke();
        t.transform.parent = null;
        

    }
    public void UI_Enter()
    {
        t3.position = t2.position;
        //ani.speed = 0;
        if (Mathf.Abs(t.localPosition.x)<0.5)
        {
            
            OK = true;
            ani.SetBool("OK", true);
            Invoke("right", 4);
            sg.SetActive(false);
            eg.SetActive(true);
        }
        else
        {
            OverEvent.Invoke();
            ani.speed = 0;
            //ani.enabled = false;
            //t3.GetComponent<test07>().enabled = false;
            t3.GetComponent<Rigidbody>().isKinematic = false;
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
            
            if (t.localPosition.x>5)
            {
               // t.localPosition += new Vector3(0, Time.deltaTime * speed, 0);
            }
            else
            {
                
            }
        }
    }
}
