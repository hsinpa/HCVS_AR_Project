using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.View;

public class ViewController : MonoBehaviour
{
    [HideInInspector]
    public bool isEnter;
    [HideInInspector]
    public int clickCount;

    public SituationMissionView situationMissionView;
    public DialogMissionView dialogMissionView;
    public QuestionMissionView questionMissionView;
    public EndMissionView endMissionView;
    public FingerClickEvent fingerClick;

    public virtual void Enable()
    {
        Debug.Log("Enable mission_id  " + MainView.Instance.studentScoreData.mission_id);
        Debug.Log("enter");
    }

    public virtual void NextAction()
    {
        Debug.Log("NextAction");
    }
}
