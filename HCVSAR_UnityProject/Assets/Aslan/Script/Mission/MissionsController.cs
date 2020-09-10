using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsController : Singleton<MissionsController>
{
    protected MissionsController() { } // guarantee this will be always a singleton only - can't use the constructor!

    public MissionViewController_1 mission_1;
    public MissionViewController_3 mission_3;
    public MissionViewController_7 mission_7;
    
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        mission_1 = mission_1.GetComponent<MissionViewController_1>();
        mission_3 = mission_3.GetComponent<MissionViewController_3>();
        mission_7 = mission_7.GetComponent<MissionViewController_7>();
    }
}
