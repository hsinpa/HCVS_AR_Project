using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using RemptyTool.ES_MessageSystem;

[RequireComponent(typeof(ES_MessageSystem))]
public class Video360 : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer vp;
    public VideoTime[] videoTimes;
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
        if (nowItem<videoTimes.Length&&vp.clockTime>videoTimes[nowItem].time)
        {
            //paint();
            
            msgSys.SetText(videoTimes[nowItem].text);
            nowItem++;
        }
        if (msgSys.IsCompleted == false)
        {
            text.text = msgSys.text;
        }
    }
    public void UI_rePlayVideo()
    {
        text.text = "";
        nowItem = 0;
    }
    public void paint()
    {
        text.text = videoTimes[nowItem].text;
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(500, 100, 200, 40), "秒數 " + vp.clockTime);
       
    }

    [System.Serializable]
    public class VideoTime
    {
        public double time;
        public string text;
    }
}
