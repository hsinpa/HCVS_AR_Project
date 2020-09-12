using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
[CreateAssetMenu(menuName = "Data/Create_VideoData ")]
public class VideoData : ScriptableObject
{
    public VideoTime[] videoTimes;
    public VideoClip clip;
    // Start is called before the first frame update
    void v_Start()
    {
        
    }

    // Update is called once per frame
    void v_Update()
    {
        
    }

    [System.Serializable]
    public class VideoTime
    {
        public double time;
        public string text;
    }
}
