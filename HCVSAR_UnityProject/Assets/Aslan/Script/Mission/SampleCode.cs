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
        button[1].onClick.AddListener(StartMission1);
        button[2].onClick.AddListener(StartMission2);
        button[3].onClick.AddListener(StartMission3);
        button[4].onClick.AddListener(StartMission4);
        button[5].onClick.AddListener(StartMission5);
        button[6].onClick.AddListener(StartMission6);
        button[7].onClick.AddListener(StartMission7);
        button[8].onClick.AddListener(StartMission8);
        button[9].onClick.AddListener(StartMission9);
    }

    void StartMission0()
    {
        MissionsController.Instance.Missions(0);
    }

    void StartMission1()
    {
        MissionsController.Instance.Missions(1);
    }

    void StartMission2()
    {
        MissionsController.Instance.Missions(2);
    }

    void StartMission3()
    {
        MissionsController.Instance.Missions(3);
    }

    void StartMission4()
    {
        MissionsController.Instance.Missions(4);
    }

    void StartMission5()
    {
        MissionsController.Instance.Missions(5);
    }

    void StartMission6()
    {
        MissionsController.Instance.Missions(6);
    }

    void StartMission7()
    {
        MissionsController.Instance.Missions(7);
    }

    void StartMission8()
    {
        MissionsController.Instance.Missions(8);
    }

    void StartMission9()
    {
        MissionsController.Instance.Missions(9);
    }
}
