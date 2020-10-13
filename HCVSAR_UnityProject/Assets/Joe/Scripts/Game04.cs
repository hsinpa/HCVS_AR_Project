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

    [Header("Audio Play")]
    [SerializeField]
    private AudioSource AirRaidtSound;

    void Update()
    {
        f = Vector3.Distance(Camera.main.transform.position, targer.position);
       
        if (JoeGM.AirRaid)
        {
            unityEvent.Invoke();
            AirRaidtSound.Stop();
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
            AirRaidtSound.enabled = true;
        }
        else
        {
            AirRaidtSound.Stop();
            OverEvent.Invoke();
        }
    }
}
