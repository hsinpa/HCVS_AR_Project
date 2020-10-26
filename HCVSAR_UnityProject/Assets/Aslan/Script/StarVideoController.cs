using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StarVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoObject;
    public Button cameraButton;
    [HideInInspector]
    public bool isStarVideoEnd;

    public void StartPlay()
    {
        int isPlayVideo = PlayerPrefs.GetInt("StarPlayVideo");
        
        if (isPlayVideo == 1)
        {
            Invoke("startIbeacon", 1);
            videoObject.SetActive(false);
        }
        else
        {
            cameraButton.enabled = false;
            videoObject.SetActive(true);
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnMovieFinished;
        }
    }

<<<<<<< HEAD
    public void startIbeacon()
    {
        JoeGM.joeGM.startIbeacon();
    }

=======
>>>>>>> c4a371fde35a598e9a39b438470ce6b2c962a9d7
    void OnMovieFinished(VideoPlayer player)
    {
        isStarVideoEnd = true;
        PlayerPrefs.SetInt("StarPlayVideo", 1);        
        cameraButton.enabled = true;
        videoObject.SetActive(false);
<<<<<<< HEAD
        try
        {
            JoeGM.joeGM.startIbeacon();

        }
        catch
        {
            Debug.Log("Star ibeacon error");
        }
=======
        JoeGM.joeGM.startIbeacon();
>>>>>>> c4a371fde35a598e9a39b438470ce6b2c962a9d7
    }
}
