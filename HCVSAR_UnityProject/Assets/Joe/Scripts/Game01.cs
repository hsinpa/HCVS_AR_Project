using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class Game01 : MonoBehaviour
{
    public float OverTime = 10;
    public float VideoTime = 12;
    public Text timeUI;
    public float f;
    public RectTransform strip;
    public UnityEvent unityEvent;
    public UnityEvent OverEvent;
    public VideoPlayer vp;
    public VideoClip vc;
    public VideoClip vc2;
    public GameObject gameUI;
    
    void Start()
    {
        vp.clip = vc;
        vp.Play();
    }

    public void UI_Enter()
    {
        if (f < 10) { f++; }
    }

    void Update()
    {
        
        if (f >= 10)
        {
            gameUI.SetActive(false);
            vp.clip = vc2;
            vp.Play();
            
            if (VideoTime > 0)
            {
                VideoTime -= Time.deltaTime;
            }

            if (VideoTime < 0)
            {
                vp.Pause();
                unityEvent.Invoke();
            }
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
                timeUI.text = Mathf.RoundToInt(OverTime).ToString();
                strip.localScale = new Vector3(0 + (f / 10), 1, 1);
            }
            else
            {
                OverEvent.Invoke();
            }
        }
        
    }

}
