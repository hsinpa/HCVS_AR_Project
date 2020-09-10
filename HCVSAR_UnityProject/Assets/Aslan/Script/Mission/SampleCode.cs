using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleCode : MonoBehaviour
{
    public Button[] button;

    private void Start()
    {
        button[0].onClick.AddListener(StartMission0);
        button[1].onClick.AddListener(StartMission7);
    }

    void StartMission0()
    {
        //MissionsController.Instance.mission_1.Init();
        MissionsController.Instance.mission_1.MissionStart(0);
    }

    void StartMission7()
    {
        MissionsController.Instance.mission_7.MissionStart(7);
    }
}
