using System.Collections;
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
    private Camera _camera;
    //public GameObject video360;
    // Start is called before the first frame update
    void Start()
    {
        //video360.SetActive(false);
        _camera = MissionsController.Instance.isARsupport ? MissionsController.Instance.ARcamera : MissionsController.Instance.MainCamera;
        transform.position = new Vector3(_camera.transform.position.x, transform.position.y, _camera.transform.position.z);
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
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity, layer))
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


