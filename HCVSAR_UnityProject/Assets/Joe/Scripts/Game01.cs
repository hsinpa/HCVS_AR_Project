using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class Game01 : MonoBehaviour
{
    public float OverTime = 10;
    public Text timeUI;
    public float f;
    public RectTransform strip;
    public UnityEvent unityEvent;
    public UnityEvent OverEvent;
    public VideoPlayer vp;
    public VideoClip vc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UI_Enter()
    {
        if (f<10) {
            f++;
        }
    }
    void Update()
    {
        //transform.position = Camera.main.transform.position;
        if (f >= 10)
        {
            vp.clip = vc;
            vp.Play();
            
            unityEvent.Invoke();
        }
        else
        {
            if (f > 0)
            {
                f -= Time.deltaTime * 3;

            }
            if (OverTime > 0)
            {
                OverTime -= Time.deltaTime;
                timeUI.text = OverTime.ToString();
                strip.localScale = new Vector3(0 + (f / 10), 1, 1);
            }
            else
            {
                OverEvent.Invoke();
            }
        }
        
    }
}
