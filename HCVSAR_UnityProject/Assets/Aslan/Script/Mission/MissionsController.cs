using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.View;

public class MissionsController : Singleton<MissionsController>
{
    protected MissionsController() { } // guarantee this will be always a singleton only - can't use the constructor!

    public MissionViewController_1 mission_1;
    public MissionViewController_3 mission_3;
    public MissionViewController_6 mission_6;
    public MissionViewController_7 mission_7;

    EnterMissionView enterMissionView;
    QuestionMissionView questionMissionView;
    EndMissionView endMissionView;

    private void Awake()
    {
        InitController();
    }

    private void InitController()
    {
        mission_1 = mission_1.GetComponent<MissionViewController_1>();
        mission_3 = mission_3.GetComponent<MissionViewController_3>();
        mission_6 = mission_3.GetComponent<MissionViewController_6>();
        mission_7 = mission_7.GetComponent<MissionViewController_7>();
    }

    // MARK: If Dont use Delete
    public void RemoveAllListeners()
    {
        endMissionView.RemoveListeners();
        questionMissionView.RemoveListeners();
        enterMissionView.RemoveListeners();
    }
}
