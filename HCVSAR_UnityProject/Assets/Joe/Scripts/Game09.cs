using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Game09 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Camera.main.transform.position;
        Input.compass.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float dushu = 0;
    float tempdushu = 0;
    
    void FixedUpdate()
    {
      


        Input.location.Start();
     
        dushu = Input.compass.trueHeading;

      
        if (Mathf.Abs(tempdushu - dushu) > 3)
        {
            tempdushu = dushu;
            transform.eulerAngles = new Vector3(0, 0, dushu);
        }


    }
}


