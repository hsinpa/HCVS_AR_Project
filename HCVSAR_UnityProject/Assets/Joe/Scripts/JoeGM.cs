using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class JoeGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        example = GetComponent<Example>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Example example;
    bool CheckDistance = true;
    public static bool AirRaid;

    public string[] missionName;
    int MinNumber = 0;
    double MinDistance = 100;
    int MissionNumber;
    bool b = true;
    int TT = 0;
    List<TypeFlag.SocketDataType.StudentType> studentData;
    public Text logui;
    public List<LogArrayData> logArray = new List<LogArrayData>();
    string uist;
    [System.Serializable]
    public class LogArrayData
    {
        public string t;
        public int v;
    }
    bool tl = true;
    public void textlog(string t)
    {
        tl = true;
        foreach (LogArrayData lad in logArray)
        {
            if (t == lad.t)
            {
                lad.v++;
                tl = false;
            }


        }

        if (tl)
        {
            LogArrayData l = new LogArrayData();
            l.t = t;
            l.v = 1;
            logArray.Add(l);
            tl = false;
        }



    }
    public void logUpd()
    {
        uist = "";
        foreach (LogArrayData lad in logArray)
        {
            uist = lad.t + "v" + lad.v + "/n" + uist;

        }
        logui.text = uist;
        logUpd();
        UpdateIBeacon();
    }

    private void UpdateIBeacon()
    {
        studentData = MainView.Instance.studentData;
        //textlog("01");
        //MainView.Instance.studentScoreData
        if (CheckDistance)
        {
            //textlog("upuupupupupuupup");
            MinDistance = 100;
            for (int i = 0; i < example.mybeacons.Count; i++)
            {
                MissionNumber = example.mybeacons[i].major * 10 + example.mybeacons[i].minor;
                b = true;
                foreach (TypeFlag.SocketDataType.StudentType studentType in studentData)
                {
                    if (studentType.mission_id == missionName[MissionNumber])
                    {
                        b = false;
                        //textlog("MissionNumber" + MissionNumber);
                        break;
                    }
                }

                if (b == true)
                {


                    if (MissionNumber == 3 && example.mybeacons[i].accuracy > 5f)
                    {
                        AirRaid = false;
                    }

                    if (example.mybeacons[i].accuracy < 5f)
                    {

                        if (example.mybeacons[i].accuracy < MinDistance)
                        {
                            MinDistance = example.mybeacons[i].accuracy;
                            MinNumber = MissionNumber;
                            //textlog("MinNumber" + MinNumber+ "MinNumber");
                        }





                        if (MissionNumber == 3)
                        {
                            AirRaid = true;
                        }


                    }
                }
            }
            if (MinDistance < 5)
            {
                //textlog("MinDistance05" + MissionsController.Instance.MissionsObj.Length.ToString()) ;
                textlog("aaaNumber" + MinNumber + "aaaNumber");

                try
                {
                    textlog("Laaaa" + MissionsController.Instance.viewControllers.Length);
                }
                catch
                {
                    textlog("eRROR22");
                }

                try
                {
                    textlog("MinDistance05" + MissionsController.Instance.viewControllers[MinNumber].isEnter);
                }
                catch
                {
                    textlog("eRROR");
                }

                b = true;
                foreach (ViewController vc in MissionsController.Instance.viewControllers)
                {
                    if (vc.isEnter == true)
                    {
                        b = false;
                    }
                }
                if (!MissionsController.Instance.viewControllers[MinNumber].isEnter && b)
                {

                    TT = 0;
                    foreach (TypeFlag.SocketDataType.StudentType studentType in studentData)
                    {
                        if (studentType.mission_id == missionName[0] || studentType.mission_id == missionName[2] || studentType.mission_id == missionName[6])
                        {
                            TT++;
                        }
                        if (studentType.mission_id == missionName[3])
                        {
                            TT = 100;
                        }
                    }
                    if (TT != 0 && TT < 50)
                    {
                        if (TT == 1 && Random.Range(0, 1) == 0)
                        {
                            MissionsController.Instance.Missions(3);
                        }
                        else
                        {
                            MissionsController.Instance.Missions(MinNumber);
                        }
                    }
                    else
                    {
                        MissionsController.Instance.Missions(MinNumber);
                    }


                }
                CheckDistance = false;
                Invoke("timeStop", 30f);
            }
        }

    }

    public void timeStop()
    {
        CheckDistance = true;
    }
}
