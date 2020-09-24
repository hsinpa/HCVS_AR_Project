using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using RemptyTool.ES_MessageSystem;

[RequireComponent(typeof(ES_MessageSystem))]
public class JoeMain : MonoBehaviour
{
    Example example;
    public static JoeMain Main;
    public VideoData[] VideoData;
    public VideoData NowVideoData;
    public VideoPlayer vp;
    public GameObject VideoPlane;
    int nowItem;
    public Text text;
    private ES_MessageSystem msgSys;
    public GameObject UI;
    public GameObject[] games;
    public GameObject ARcamera;
    private void Awake()
    {
        Main = this;
    }// Start is called before the first frame update
    void Start()
    {
        vp.Play();
        example = GetComponent<Example>();

        msgSys = this.GetComponent<ES_MessageSystem>();
    }
    
    public void Start360Video(int number)
    {
        NowVideoData = VideoData[number];
        vp.clip = NowVideoData.clip;
        StartCoroutine(CoroutineTest());
        
        
        VideoPlane.SetActive(true);
        
    }
    IEnumerator CoroutineTest()
    {
        vp.Play();
        yield return new WaitForSeconds(1.5f);
        vp.Pause();
    }
    public void Play360Video()
    {
        vp.Play();
        UI.SetActive(true);
    }

    public void Stop360Video()
    {
        vp.Stop();
        VideoPlane.SetActive(false);
        UI.SetActive(false);
    }

    public void ControllerARCamera(bool open)
    {
        ARcamera.SetActive(open);
    }

    public void PlayGame(int number)
    {
        games[number].SetActive(true);
    }

    public void CloseGame(int number)
    {
        games[number].SetActive(false);
    }

    public void PlayARGame(int number)
    {
        games[number].SetActive(true);
        //ARcamera.SetActive(true);
    }

    public void CloseARGame(int number)
    {
        games[number].SetActive(false);
        ARcamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position;
        if (NowVideoData!=null&&nowItem < NowVideoData.videoTimes.Length && vp.clockTime > NowVideoData.videoTimes[nowItem].time)
        {
            //paint();

            msgSys.SetText(NowVideoData.videoTimes[nowItem].text);
            nowItem++;
        }
        if (msgSys.IsCompleted == false)
        {
            text.text = msgSys.text;
        }
        UpdateIBeacon();
    }
    bool CheckDistance = true;
    public static bool AirRaid;
    public static bool[] Missiont = new bool[13];
    private void UpdateIBeacon()
    {
        //MainView.Instance.studentScoreData
        if (CheckDistance) {
            foreach (Beacon beacon in example.mybeacons)
            {
                if (beacon.minor == 3&& beacon.accuracy > 5f)
                {
                    AirRaid = false;
                }
                if (beacon.accuracy < 5f&& beacon.major == 0)
                {
                    switch (beacon.minor)
                    {
                        case 0:
                            if(!Missiont[0])
                                MissionsController.Instance.Missions(0);
                            break;
                        case 1:
                            if (!Missiont[1])
                                MissionsController.Instance.Missions(1);
                            break;
                        case 2:
                            if (!Missiont[2])
                                MissionsController.Instance.Missions(2);
                            break;
                        case 3:
                            if (!Missiont[3])
                                AirRaid = true;
                                break;
                        case 4:
                            if (!Missiont[4])
                                MissionsController.Instance.Missions(4);
                            break;
                        case 5:
                            if (!Missiont[5])
                                MissionsController.Instance.Missions(5);
                            break;
                        case 6:
                            if (!Missiont[6])
                                MissionsController.Instance.Missions(6);
                            break;
                        case 7:
                            if (!Missiont[7])
                                MissionsController.Instance.Missions(7);
                            break;
                        case 8:
                            if (!Missiont[8])
                                MissionsController.Instance.Missions(8);
                            break;
                        case 9:
                            if (!Missiont[9])
                                MissionsController.Instance.Missions(9);
                            break;
                    }
                    CheckDistance = false;
                    Invoke("time30",30f);
                    break;

                }
            }
        }
    }

    public void time30()
    {
        CheckDistance = true;
    }
    public void UI_rePlayVideo()
    {
        text.text = "";
        nowItem = 0;
    }
    public void paint()
    {
        text.text = NowVideoData.videoTimes[nowItem].text;
    }
    private void OnGUI()
    {
       // GUI.Label(new Rect(500, 100, 200, 40), "秒數 " + vp.clockTime);

    }

    [System.Serializable]
    public class VideoTime
    {
        public double time;
        public string text;
    }

}
