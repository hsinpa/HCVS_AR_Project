using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
public class AirGameStart : MonoBehaviour
{
    public UnityEvent ue;
    public float time = 5;
    // Start is called before the first frame update
    public void GO()
    {
        Invoke("Rgo", time);
    }
    public void Rgo()
    {
        ue.Invoke();
    }
    
}
