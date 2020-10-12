using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {

    public Transform target;
    GameObject G;
    Camera camera;
    public RectTransform P;
    void Start()
    {
     
    }

    void Update()
    {
        transform.LookAt(target);  
    }
}