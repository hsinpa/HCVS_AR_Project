using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Game04 : MonoBehaviour
{
    public float OverTime = 90;
    public Text timeUI;
    public float f = 10;
    public Transform targer;
    public UnityEvent unityEvent;
    public UnityEvent OverEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UI_Enter()
    {
        
    }
    void Update()
    {
        f = Vector3.Distance(Camera.main.transform.position, targer.position);
        if (f < 2)
        {
            unityEvent.Invoke();
        }
        else
        {
            if (f > 0)
            {
                f -= Time.deltaTime * 3;
            }
        }
        if (OverTime>0)
        {
            OverTime -= Time.deltaTime;
            timeUI.text = OverTime.ToString();
                
        }
        else
        {

            OverEvent.Invoke();
        }
    }
}
