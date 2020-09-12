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
        button[1].onClick.AddListener(StartMission2);
        button[2].onClick.AddListener(StartMission4);
        button[3].onClick.AddListener(StartMission5);
        button[4].onClick.AddListener(StartMission6);
        button[5].onClick.AddListener(StartMission7);
    }

    void StartMission0()
    {
        MissionsController.Instance.mission_0.MissionStart(0);
    }

    void StartMission2()
    {
        MissionsController.Instance.mission_2.MissionStart(2);
    }

    void StartMission4()
    {
        MissionsController.Instance.mission_4.MissionStart(4);
    }

    void StartMission5()
    {
        MissionsController.Instance.mission_5.MissionStart(5);
    }

    void StartMission6()
    {
        MissionsController.Instance.mission_6.MissionStart(6);
    }

    void StartMission7()
    {
        MissionsController.Instance.mission_7.MissionStart(7);
    }
}
