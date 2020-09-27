using UnityEngine;
using Expect.View;

public class MissionsController : Singleton<MissionsController>
{
    protected MissionsController() { } // guarantee this will be always a singleton only - can't use the constructor!

    public GameObject[] MissionsObj;
    public ViewController[] viewControllers;

    public EnterMissionView enterMissionView;

    //[HideInInspector]
    public bool isEnter;

    private void Awake()
    {
        InitController();
        ReSetMissions();
    }

    private void InitController()
    {
        for (int i = 0; i < MissionsObj.Length; i++)
        {
            viewControllers[i] = MissionsObj[i].GetComponent<ViewController>();
        }
    }

    public void ReSetMissions()
    {
        isEnter = false;
        foreach (GameObject g in MissionsObj) { g.SetActive(false); }
    }

    public void Missions(int number)
    {
        MissionStart(number);

        if (number != 3)
        { 
            enterMissionView.EnterButton.onClick.AddListener(() => EnterGame(number));
            enterMissionView.LeaveButton.onClick.AddListener(() => LeaveGame(number));
        }
    }

    public void EnterGame(int number)
    {
        MissionsObj[number].SetActive(true);
        viewControllers[number].Enable();
        viewControllers[number].isEnter = true;

        isEnter = true;

        CloseEnterView();
    }

    public void LeaveGame(int number)
    {
        viewControllers[number].isEnter = false;
        ReSetMissions();
        CloseEnterView();
    }

    public void MissionStart(int missionNumber)
    {
        TypeFlag.InGameType.MissionType[] missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
        MainView.Instance.studentScoreData.mission_id = missionArray[missionNumber].mission_id;
        MainView.Instance.missionNumber = missionNumber;
        
        if (missionNumber == 3) { EnterGame(missionNumber); }

        if (missionNumber != 3)
        {
            enterMissionView.Show(true);
            enterMissionView.EnterMission(missionArray[missionNumber].mission_name, missionArray[missionNumber].mission_name);
        }

    }

    // MARK: If Dont use Delete
    public void CloseEnterView()
    {
        enterMissionView.Show(false);
        enterMissionView.RemoveListeners();
    }
}
