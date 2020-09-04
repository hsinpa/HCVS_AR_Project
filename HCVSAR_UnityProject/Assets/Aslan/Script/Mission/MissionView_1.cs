using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;


public class MissionView_1 : MonoBehaviour
{
    [SerializeField]
    private Button mission_1;
    [SerializeField]
    private Sprite dog;
    [SerializeField]
    private Sprite person;

    [SerializeField]
    EnterMissionView enterMissionView;
    [SerializeField]
    SituationMissionView situationMissionView;
    [SerializeField]
    DialogMissionView dialogMissionView;
    [SerializeField]
    QuestionMissionView questionMissionView;

    FingerClickEvent fingerClick;

    private TypeFlag.InGameType.MissionType[] missionArray;

    int clickCount;

    // Message
    string situationMessage = StringAsset.MissionsSituation.One.s1;
    string dogName = StringAsset.MissionsDialog.Person.dog;
    string peopleName = StringAsset.MissionsDialog.Person.people;
    string dogMessage = StringAsset.MissionsDialog.One.d1;
    string peopleMessage = StringAsset.MissionsDialog.One.d2;
    string[] historyMessage = { StringAsset.MissionsDialog.One.history1, StringAsset.MissionsDialog.One.history2,
                                 StringAsset.MissionsDialog.One.history3, StringAsset.MissionsDialog.One.history4};

    private string qustion = StringAsset.MissionsQustion.One.qustion;
    private string[] answers = { StringAsset.MissionsAnswer.One.ans1, StringAsset.MissionsAnswer.One.ans2,
                                 StringAsset.MissionsAnswer.One.ans3, StringAsset.MissionsAnswer.One.ans4};
    /*
    public enum ConvercestionType
    {
        Dialog_talk1, Dialog_talk2, History
    }
    
    ConvercestionType convercestionType;
    */

    private void MissionArraySetUp()
    {
        missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
    }

    private void Awake()
    {
        MissionArraySetUp();
        fingerClick = situationMissionView.GetComponentInParent<FingerClickEvent>();
    }

    private void Start()
    {
        mission_1.onClick.AddListener(MissionStart);
        
    }

    private void MissionStart()
    {
        enterMissionView.Show(true);
        enterMissionView.EnterMission(missionArray[0].mission_name, missionArray[0].mission_name);
        enterMissionView.OnEnable += StarEnable;
        enterMissionView.OnDisable += Disable;
    }

    // TODO: ibeacon find other mission
    void Disable()
    {
        Debug.Log("other thing");
    }

    void StarEnable()
    {
        enterMissionView.OnEnable -= StarEnable;
        enterMissionView.OnDisable -= Disable;
        enterMissionView.Show(false);

        situationMissionView.Show(true);
        situationMissionView.SituationView(situationMessage);
        
        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += ClickCount; // Add fingerClick event
    }

    void ClickCount()
    {
        clickCount++;
        Debug.Log("clickCount: " + clickCount);
        Convercestion();
        
    }

    void Convercestion()
    {
        if (clickCount == 1)
        {
            Debug.Log("clickCount1: " + clickCount);
            situationMissionView.Show(false);
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(dogName, dogMessage, dog);
        }
        if (clickCount == 2)
        {
            Debug.Log("clickCount2: " + clickCount);
            dialogMissionView.DialogView(peopleName, peopleMessage, person);
        }

        if (clickCount >= 3 && clickCount < historyMessage.Length+3)
        {
            Debug.Log("clickCount3: " + clickCount);
            dialogMissionView.DialogView(dogName, historyMessage[clickCount-3], dog);
        }

        if (clickCount == historyMessage.Length+3)
        {
            Debug.Log("Finish");
            Qusteion();
        }

        /*
        switch (clickCount)
        {
            case (int)ConvercestionType.Dialog_talk1:
                Debug.Log("clickCount2: " + clickCount);
                situationMissionView.Show(false);
                dialogMissionView.Show(true);
                dialogMissionView.DialogView(dogName, dogMessage, dog);
                break;
            case (int)ConvercestionType.Dialog_talk2:
                dialogMissionView.DialogView(peopleName, peopleMessage, person);
                break;
            case (int)ConvercestionType.History:
                dialogMissionView.DialogView(dogName, historyMessage[0], dog);
                break;
        }
        */
    }

    void Qusteion()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        Debug.Log("Finish111");
        dialogMissionView.Show(false);
        questionMissionView.Show(true);
        questionMissionView.QuestionView(qustion, answers);
    }

}
