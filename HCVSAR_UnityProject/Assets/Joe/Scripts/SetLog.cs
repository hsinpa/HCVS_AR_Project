using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetLog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        JoeGM.joeGM.logui = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
