using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using RemptyTool.ES_MessageSystem;
using Expect.StaticAsset;
using Hsinpa.Video;

[RequireComponent(typeof(ES_MessageSystem))]
public class JoeMain : MonoBehaviour
{
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite primeMinister;
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string primeMinisterName = StringAsset.MissionsDialog.Person.NPC_5;

    public GameObject UI;
    public Text text;
    public Text name;
    public Image image;

    public static JoeMain Main;
    public VideoData[] VideoData;
    public VideoData NowVideoData;
    public VideoPlayer vp;
    public GameObject VideoPlane;
    public int nowItem;
    private int currentElement;
    private bool isVideoEnd;
    
    private ES_MessageSystem msgSys;
    public GameObject[] games;

    private void Awake()
    {
        Main = this;
    }

    void Start()
    {
        //vp.Play();

        msgSys = this.GetComponent<ES_MessageSystem>();
    }
    
    public void Start360Video(int number)
    {
        NowVideoData = VideoData[number];
        vp.clip = NowVideoData.clip;

        StartCoroutine(CoroutineTest());

        UI_rePlayVideo();
        UI.SetActive(false);
        currentElement = number;
        
        // dialog view
        name.text = number == 6 ? primeMinisterName : dogName;
        image.sprite = number == 6 ? primeMinister : dog;

        isVideoEnd = false;
        VideoPlane.SetActive(true);
        
    }

#if UNITY_IOS
    IEnumerator CoroutineTest()
    {
        vp.Play();

        yield return new WaitForSeconds(0.1f);
        vp.Pause();
    }
#endif

#if UNITY_ANDROID
    IEnumerator CoroutineTest()
    {
        yield return new WaitForSeconds(1f);
        vp.Play();

        yield return new WaitForSeconds(0.1f);
        vp.Pause();
    }
#endif

    public void StarAndPlay360Video(int number)
    {
        NowVideoData = VideoData[number];
        vp.clip = NowVideoData.clip;
        vp.Play();

        UI_rePlayVideo();
        UI.SetActive(true);

        currentElement = number;
        
        // dialog view
        name.text = number == 6 ? primeMinisterName : dogName;
        image.sprite = number == 6 ? primeMinister : dog;

        isVideoEnd = false;
        VideoPlane.SetActive(true);

    }

    public void Play360Video()
    {
        vp.Play();
        //UI.SetActive(true);
    }

    public void Stop360Video()
    {
        //vp.clip = null;
        NowVideoData.clip = null;

        vp.Stop();
        VideoPlane.SetActive(false);
        UI.SetActive(false);
    }

    public void ControllerVideoPlane(bool open)
    {
        VideoPlane.SetActive(!open);
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
        //ARcamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var currentTime = Mathf.FloorToInt((float)vp.clockTime);
        var endTime = Mathf.FloorToInt((float)vp.length);

        if (NowVideoData == null) return;

        if (nowItem < NowVideoData.videoTimes.Length)
        {
            if (vp.clockTime > NowVideoData.videoTimes[nowItem].time)
            {
                UI.SetActive(true);
                msgSys.SetText(NowVideoData.videoTimes[nowItem].text);
                nowItem++;
            }
        }

        if (msgSys.IsCompleted == false)
        {
            text.text = msgSys.text;
        }
        
        if (currentTime == endTime && !isVideoEnd)
        {
            isVideoEnd = true;

            MissionsController.Instance.viewControllers[currentElement].NextAction();
        }
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
