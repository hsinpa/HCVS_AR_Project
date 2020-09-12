using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionViewController_3 : MonoBehaviour
{
    public GameObject game04;

    public void MissionStart(int missionNumber)
    {
        TypeFlag.InGameType.MissionType[] missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
        MainView.Instance.studentScoreData.mission_id = missionArray[missionNumber].mission_id;
        game04.SetActive(true);
    }
}
