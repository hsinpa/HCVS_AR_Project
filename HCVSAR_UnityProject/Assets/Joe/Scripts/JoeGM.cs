using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class JoeGM : MonoBehaviour
{
    Example example;
    public static JoeGM joeGM;
    TypeFlag.UserType GameType;
    //public List<Beacon> beacons;
    bool CheckDistance = false;
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
    bool[] Missioned = new bool[20];
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
            uist = uist + "\n" + lad.t + "~~~~~~~~~~~~~" + lad.v  ;

        }
        logui.text = uist;
       
    }
    bool R = true;
    public bool testgame;
    public void UpdateIBeacon()
    {
        switch (GameType)
        {
            case TypeFlag.UserType.Guest:
                UpdateIBeaconGuest();
                break;

            case TypeFlag.UserType.Student:
                UpdateIBeaconStudent();             
                break;
        }

    }

    private void UpdateIBeaconStudent()
    {

        studentData = MainView.Instance.studentData;
        //textlog("01");
        //MainView.Instance.studentScoreData
        if (CheckDistance && MissionsController.Instance.isEnter != true)
        {

            textlog("studentData" + studentData.Count);
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

                    if (MissionNumber == 11 || MissionNumber == 12 || MissionNumber == 3)
                    {
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
                        else if (example.mybeacons[i].accuracy <= 5f)
                        {
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
                        goto OverLoop;
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


                    if (example.mybeacons[i].accuracy < 5f)
                    {

                        if (example.mybeacons[i].accuracy < MinDistance)
                        {
                            MinDistance = example.mybeacons[i].accuracy;
                            MinNumber = MissionNumber;
                            textlog("OVERMin" + MinNumber + "oVERMin");
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


            if (MinDistance < 5)
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
                        goto Missioning;
                    }
                }
                if (TT != 0)
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
                    textlog("StartNumber" + MinNumber);
                    MissionsController.Instance.Missions(MinNumber);
                    CheckDistance = false;
                    
                }


            Missioning:
                textlog("Missioning");

            }
        }



    }
    private void UpdateIBeaconGuest()
    {

        //studentData = MainView.Instance.studentData;
        //textlog("01");
        //MainView.Instance.studentScoreData
        if (CheckDistance && MissionsController.Instance.isEnter != true)
        {

            //textlog("studentData" + studentData.Count);
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

                    if (MissionNumber == 11 || MissionNumber == 12 || MissionNumber == 3)
                    {
                        CheckMission(example.mybeacons[i].accuracy);                    
                        goto OverLoop;
                    }
                    else
                    {
                        if (Missioned[MissionNumber] == true)
                        {                            
                            goto OverLoop;
                        }
                    }


                    if (example.mybeacons[i].accuracy < 5f)
                    {

                        if (example.mybeacons[i].accuracy < MinDistance)
                        {
                            MinDistance = example.mybeacons[i].accuracy;
                            MinNumber = MissionNumber;
                            
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


            if (MinDistance < 5)
            {
                textlog("OVERMin" + MinNumber );
               

                
                if ((MinNumber == 0|| MinNumber == 2|| MinNumber == 6)&& !Missioned[3])
                {
                    TT = 0;
                    TT += (Missioned[0] ? 0 : 1) + (Missioned[2] ? 0 : 1) + (Missioned[6] ? 0 : 1);
                    
                    if (Random.Range(0, TT) == 0)
                    {
                        MissionsController.Instance.Missions(3);
                        LeaveMission();
                    }
                    else
                    {
                        MissionsController.Instance.Missions(MinNumber);
                    }
                }
                else
                {
                    textlog("StartNumber" + MinNumber);
                    MissionsController.Instance.Missions(MinNumber);
                    

                }
                CheckDistance = false;



           //Missioning:
                //textlog("Missioning");

            }
        }



    }
  
    private void CheckMission(double accuracy)
    {
        if (MissionNumber == 3)
        {
            AirRaid = accuracy < 5f;
        }

        if (MissionNumber == 11)
        {
            RightBotton.SetActive(accuracy < 5f);
        }

        if (MissionNumber == 12)
        {
            RightBotton.SetActive(accuracy < 5f);
        }
        
    }
    public void StartMission(int number)
    {
        Missioned[number] = true;
        Invoke("timeStop", 10f);
    }

    public void LeaveMission()
    {
        Invoke("timeStop", 10f);
    }
    public void timeStop()
    {
        CheckDistance = true;
    }

    public void StartBeacom(TypeFlag.UserType type)
    {
        GameType = type;
        CheckDistance = true;
    }
}
