using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Hsinpa.View;

public class JoeGM : MonoBehaviour
{
    Example example;
    public GameObject expectObj;
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

    public GameObject useButton;
    public GameObject RightBotton;
    public GameObject FullBotton;
    public double[] MissionMinDistances = new double[15]; 
    public double[] IBeaconDistances = new double[30];
    public GameObject[] RedPoints;
    public delegate void BeaconUPD();
    public static event BeaconUPD beaconUPD;
    public bool isGameStart;
    public bool hasMail;

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
        UpdateIBeaconMain();
        //beaconUPD();
        
    }

    public void UpdateIBeaconStudent()
    {
        studentData = MainView.Instance.studentData;

        for (int i = 0; i < missionName.Length; i++)
        {
            Debug.Log("Student"+ studentData.Count);
            foreach (TypeFlag.SocketDataType.StudentType studentType in studentData)
            {
                Debug.Log("3");
                if (studentType.mission_id == missionName[i])
                {
                    Debug.Log("2");
                    Missioned[i] = true;
                }

            }


        }


    }
    public string c;
    private void UpdateIBeaconMain()
    {
       
        //studentData = MainView.Instance.studentData;
        //textlog("01");
        //MainView.Instance.studentScoreData
        c = "";
        if (isGameStart) {

            //textlog("studentData" + studentData.Count);
            MinDistance = 100;
            RedPoints[MinNumber].SetActive(false);
            try
            {
                for (int i = 0; i < example.mybeacons.Count; i++)
                {

                    MissionNumber = example.mybeacons[i].major * 10 + example.mybeacons[i].minor;
                    IBeaconDistances[MissionNumber] = example.mybeacons[i].accuracy;
                    

                    if (MissionNumber == 11 || MissionNumber == 12 || MissionNumber == 3|| MissionNumber == 1)
                    {
                        CheckMission(example.mybeacons[i].accuracy);
                        goto OverLoop;
                    }
                    else
                    {
                        if (Missioned[MissionNumber] == true)
                        {
                            c += MissionNumber.ToString();
                            goto OverLoop;
                        }
                    }

                    if (CheckDistance && MissionsController.Instance.isEnter != true)
                    {
                        if (example.mybeacons[i].accuracy!=0)
                        {

                            if (example.mybeacons[i].accuracy < MinDistance)
                            {
                                MinDistance = example.mybeacons[i].accuracy;
                                MinNumber = MissionNumber;

                            }

                        }

                    }


                OverLoop:
                    textlog("overMission"+c);
                }
            }
            catch
            {
                textlog("ErrorLoop");
            }

            if (IBeaconDistances[3]<MinDistance)
            {
                RedPoints[3].SetActive(true);
            }else if (IBeaconDistances[1] < MinDistance)
            {
                RedPoints[1].SetActive(true);
            }
            else
            {
                RedPoints[MinNumber].SetActive(true);
            }
            
            if (MinDistance < MissionMinDistances[MinNumber])
            {
                textlog("OVERMin" + MinNumber);

                if (MissionNumber != 1)
                {
                    RightBotton.SetActive(false);
                    useButton.SetActive(false);
                }

                if ((MinNumber == 0 || MinNumber == 2 || MinNumber == 6) && !Missioned[3])
                {
                    TT = 0;
                    TT += (Missioned[0] ? 0 : 1) + (Missioned[2] ? 0 : 1) + (Missioned[6] ? 0 : 1);

                    if (Random.Range(0, TT) == 0)
                    {
                        Handheld.Vibrate();
                        MissionsController.Instance.Missions(3);
                        
                        LeaveMission();
                    }
                    else
                    {
                        Handheld.Vibrate();
                        MissionsController.Instance.Missions(MinNumber);
                    }
                }
                else if (MinNumber == 9)
                {
                    if (MainView.Instance.isEndMissionOpen == true)
                    {
                        Handheld.Vibrate();
                        MissionsController.Instance.Missions(MinNumber);
                    }
                    else
                    {
                        Invoke("timeStop", 10f);
                    }
                }
                else
                {
                    textlog("StartNumber" + MinNumber);
                    Handheld.Vibrate();
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
            AirRaid = accuracy < MissionMinDistances[3];
        }

        if (MissionNumber == 1)
        {
            RightBotton.SetActive(accuracy < MissionMinDistances[1]);
            useButton.SetActive(accuracy < MissionMinDistances[1] && hasMail);
            //FullBotton.SetActive(!(accuracy < 5f));
        }

        if (MissionNumber == 12)
        {
            //RightBotton.SetActive(!(accuracy < 5f));
            //FullBotton.SetActive(accuracy < 5f);
        }
        
    }
    public void StartMission(int number)
    {
        Missioned[number] = true;
        Invoke("timeStop", 10f);
    }

    public void LeaveMission()
    {
        Missioned[3] = true;
        Invoke("timeStop", 10f);
    }
    public void timeStop()
    {
        CheckDistance = true;
    }

    public void StartBeacom(TypeFlag.UserType type)
    {
        Debug.Log("1");
        GameType = type;
        CheckDistance = true;
        example = expectObj.GetComponent<Example>();
        switch (GameType)
        {
            case TypeFlag.UserType.Guest:
                
                break;

            case TypeFlag.UserType.Student:
                Debug.Log("Student");
                //UpdateIBeaconStudent();
                
                break;
        }
        
        //Invoke("startIbeacon", 1);
    }

    public void startIbeacon()
    {
        example.btn_StartStop();
        isGameStart=true;
    }
}
