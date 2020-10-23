﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
public class Game09 : MonoBehaviour
{
    public LayerMask layer;
    public GameObject UI;
    public Text text;
    public string[] vs;
    public GameObject video360;
    // Start is called before the first frame update
    void Start()
    {
        video360.SetActive(false);
        transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        transform.rotation = MainCompass.main.transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = MainCompass.main.transform.rotation;
    }

  
    RaycastHit hit;
    void FixedUpdate()
    {



        
     
       
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, layer))
        {
            if (!UI.activeInHierarchy) {
                UI.SetActive(true);
                text.text = vs[int.Parse(hit.collider.gameObject.name)];

            }
        }
        else
        {
            if (UI.activeInHierarchy)
            {

                UI.SetActive(false);

            }
        }

    }
}


