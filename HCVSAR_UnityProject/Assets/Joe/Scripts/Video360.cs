using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using RemptyTool.ES_MessageSystem;


public class Video360 : MonoBehaviour
{
    // Start is called before the first frame update
    public Video360[] video360s;
    public VideoPlayer vp;
    //public VideoTime[] videoTimes;
    int nowItem;
    public Text text;
    private ES_MessageSystem msgSys;
    //public UnityEngine.UI.Text uiText;
    void Start()
    {
        msgSys = this.GetComponent<ES_MessageSystem>();
    }

    // Update is called once per frame

    void Update()
    {
     
    }
    public void UI_rePlayVideo()
    {
        text.text = "";
        nowItem = 0;
    }
    public void paint()
    {
       // text.text = videoTimes[nowItem].text;
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(500, 100, 200, 40), "秒數 " + vp.clockTime);
       
    }

    
}
