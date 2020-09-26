using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;

public class MapCollect : MonoBehaviour
{
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite map;
    [SerializeField]
    private Sprite mapSprite;
    [SerializeField]
    private Image mapImage;
    [SerializeField]
    private Button map1Button;
    [SerializeField]
    private Button map2Button;

    [SerializeField]
    DialogMissionView dialogMissionView;
    [SerializeField]
    EndMissionView endMissionView;
    [SerializeField]
    FingerClickEvent fingerClick;
    [SerializeField]
    BagPanel bagPanel;

    // Message
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string mapName = StringAsset.MissionsDialog.Person.map;
    private string dogMessage1 = StringAsset.MissionsDialog.Map.d1;
    private string dogMessage2 = StringAsset.MissionsDialog.Map.d2;
    private string mapessage1 = StringAsset.MissionsDialog.Map.map1;
    private string endMessage = StringAsset.MissionsEnd.End.message;

    private bool isCollectMap1;
    private bool isCollectMap2;
    private int clickCount;

    public GameObject toolView;

    private void Start()
    {
        map1Button.onClick.AddListener(ClickMap1);
        map2Button.onClick.AddListener(ClickMap2);
    }

    private void ClickMap1()
    {
        isCollectMap1 = true;
        
        if (isCollectMap2) { StartCoroutine(GetAllMap()); }
    }

    private void ClickMap2()
    {
        isCollectMap2 = true;
        
        if (isCollectMap1) {  StartCoroutine(GetAllMap()); }
            
    }

    public IEnumerator GetAllMap()
    {
        yield return new WaitForSeconds(1);
        
        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += ClickCount; // Add fingerClick event

        dialogMissionView.Show(true);
        dialogMissionView.DialogView(dogName, dogMessage1, dog);
    }

    private void ClickCount()
    {
        clickCount++;

        if (clickCount >= 0)
        {
            Convercestion();
        }

        Debug.Log("clickCount: " + clickCount);
    }

    private void Convercestion()
    {
        if (clickCount == 1)
        {
            mapImage.sprite = mapSprite;
            mapImage.enabled = true;
            dialogMissionView.DialogView(dogName, dogMessage2, dog);
        }

        if (clickCount == 2)
        {
            dialogMissionView.DialogView(mapName, mapessage1, map);
        }

        if (clickCount == 3)
        {
            mapImage.enabled = false;
            dialogMissionView.Show(false);
            toolView.SetActive(true);
        }
    }

    public void toolButtonClick()
    {
        int score = 15;
        toolView.SetActive(false);
        endMissionView.Show(true);
        endMissionView.EndMission(score, endMessage);
        endMissionView.OnEnable += LeaveEvent;

        MainView.Instance.studentScoreData.mission_id = "MAP_BONUS";
        PostScoreEvent.Instance.PostScore(score, MainView.Instance.loginData.userType);

        var id = MainView.Instance.studentScoreData.mission_id;
        var user = MainView.Instance.studentScoreData.student_id;
        Debug.Log("id: " + id + " user" + user);
    }

    private void LeaveEvent()
    {
        endMissionView.Show(false);

        bagPanel.RemoveMapChip();
        bagPanel.AddAllMapInfo();

        InitFingerClick();
        RemoveAllEvent();
        RemoveAllListeners();

        Debug.Log("Map Leave");
    }

    private void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        map1Button.onClick.RemoveAllListeners();
        map2Button.onClick.RemoveAllListeners();
    }

    private void RemoveAllEvent()
    {
        fingerClick.Click -= ClickCount;
        endMissionView.OnEnable -= LeaveEvent;
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial
    }

}
