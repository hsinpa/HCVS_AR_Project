using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;

public class ViewController : MonoBehaviour
{
    [HideInInspector]
    public bool isEnter;
    [HideInInspector]
    public int clickCount;
    //[HideInInspector]
    //public FingerClickEvent fingerClick;

    public SituationMissionView situationMissionView;
    public DialogMissionView dialogMissionView;
    public QuestionMissionView questionMissionView;
    public EndMissionView endMissionView;
    public Button nextButton;
    //public FingerClickController fingerClickController;

    public virtual void Enable()
    {
        Debug.Log("Enable mission_id  " + MainView.Instance.studentScoreData.mission_id);
        Debug.Log("enter");
    }

    public virtual void NextAction()
    {
        Debug.Log("NextAction");
    }

    public void ClickNextButton()
    {
        clickCount = 0;
        OnClickButton(true);
        nextButton.onClick.RemoveAllListeners();
    }

    public void OnClickButton(bool isOpne)
    {
        nextButton.GetComponent<Image>().enabled = isOpne;
    }
}
