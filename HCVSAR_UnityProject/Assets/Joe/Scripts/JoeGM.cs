using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class JoeGM : MonoBehaviour
{
    Example example;
    public static JoeGM joeGM;
    //public List<Beacon> beacons;
    bool CheckDistance = true;
    public static bool AirRaid;

    public string[] missionName ;
    int MinNumber = 0;
    double MinDistance = 100;
    int MissionNumber;
    bool b = true;
    int TT = 0;
    List<TypeFlag.SocketDataType.StudentType> studentData;
    public Text logui;
    public List<LogArrayData> logArray = new List<LogArrayData>();
    string uist;
    public List<IBCCC> mybeacons;

    public GameObject RightBotton;
    public GameObject FullBotton;
    [System.Serializable]
    public class LogArrayData
    {
        public string t;
        public int v;
    }
    [System.Serializable]
    public class IBCCC
    {
        public double accuracy;
        public int major;
        public int minor;
    }
    bool tl = true;
    private void Awake()
    {
        joeGM = this;
        missionName = new string[] { "A", "B", "C", "D", "E", "F", "G","I","J","K","L","M","N"};
    }
    // Start is called before the first frame update
    void Start()
    {
        //example = this;
        example = GetComponent<Example>();
    }

    // Update is called once per frame
    void Update()
    {
        //logUpd();
        //UpdateIBeacon();
        
    }
    
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
            uist = lad.t + "v" + lad.v + "\n" + uist;

        }
        logui.text = uist;
       
    }
    bool R = true;
    public bool testgame;
    public void UpdateIBeacon()
    {
        
        studentData = MainView.Instance.studentData;
        //textlog("01");
        //MainView.Instance.studentScoreData
        if (CheckDistance)
        {
           
            textlog("upuupupupupuupup"+ studentData.Count);
            MinDistance = 100;
            
            try
            {
                for (int i = 0; i < example.mybeacons.Count; i++)
                {

                    MissionNumber = example.mybeacons[i].major * 10 + example.mybeacons[i].minor;
                    if (MissionNumber >= missionName.Length)
                    {
                        continue;
                    }

                    if (MissionNumber == 11 || MissionNumber == 12 || MissionNumber == 3||testgame)
                    {

                    }
                    else
                    {
                        foreach (TypeFlag.SocketDataType.StudentType studentType in studentData)
                        {

                            if (studentType.mission_id == missionName[MissionNumber])
                            {

                                textlog("mission_id" + studentType.mission_id + MissionNumber);
                                goto OverLoop;
                            }

                        }
                    }

                    if (example.mybeacons[i].accuracy > 5f)
                    {
                        if (MissionNumber == 3)
                        {
                            AirRaid = false;
                        }


                        if (MissionNumber == 11)
                        {
                            RightBotton.SetActive(false);
                        }

                        if (MissionNumber == 12)
                        {
                            RightBotton.SetActive(false);
                        }
                    }

                   

                    if (example.mybeacons[i].accuracy < 5f)
                    {

                        if (example.mybeacons[i].accuracy < MinDistance)
                        {
                            MinDistance = example.mybeacons[i].accuracy;
                            MinNumber = MissionNumber;
                            textlog("OVERMin" + MinNumber + "oVERMin");
                        }


                        if (MissionNumber == 11)
                        {
                            RightBotton.SetActive(true);
                        }

                        if (MissionNumber == 12)
                        {
                            RightBotton.SetActive(true);
                        }


                        if (MissionNumber == 3)
                        {
                            AirRaid = true;
                        }


                    }


                OverLoop:
                    textlog("JampLoop");
                }
            }
            catch
            {
                textlog("ErrorLoop");
            }

            try
            {
                //textlog("MinDistance" + MinDistance + "aaaNumber" + MinNumber);
            }
            catch
            {
                textlog("ErrorPrint");
            }
            
            if (MinDistance < 5)
            {
                //textlog("MinDistance05" + MissionsController.Instance.MissionsObj.Length.ToString()) ;
                

                try
                {
                    textlog("Laaaa" + MissionsController.Instance.viewControllers.Length);
                }
                catch
                {
                    textlog("ErrorLenght");
                }

                try
                {
                    textlog("StartMission"+ MinNumber+ MissionsController.Instance.viewControllers[MinNumber].isEnter);
                }
                catch
                {
                    textlog("ErrorEnter");
                }

                if (MissionsController.Instance.isEnter == true)
                {
                    goto Missioning;
                }
                
                TT = 0;
                /*
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
                    
                }*/
                textlog("StartNumber" + MinNumber);
                MissionsController.Instance.Missions(MinNumber);

                CheckDistance = false;
                Invoke("timeStop", 10f);
            Missioning:
                    textlog("Missioning");
                
            }
        }

    }

    public void timeStop()
    {
        CheckDistance = true;
    }
}
