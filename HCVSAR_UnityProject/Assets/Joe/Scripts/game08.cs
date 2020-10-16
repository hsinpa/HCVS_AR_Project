using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game08 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ransform.position = Camera.main.transform.position;
        transform.rotation = MainCompass.main.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = MainCompass.main.transform.rotation;
    }
}
