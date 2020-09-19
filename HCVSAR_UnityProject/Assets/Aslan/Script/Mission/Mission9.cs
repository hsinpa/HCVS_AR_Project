using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;

public class Mission9 : ViewController
{
    [SerializeField]
    private Sprite dog;

    // Message
    private string dogName = StringAsset.MissionsDialog.Person.dog;
    private string dogMessage1 = StringAsset.MissionsDialog.Nine.d1;
    private string[] historyMessage = { StringAsset.MissionsDialog.Eight.history1, StringAsset.MissionsDialog.Eight.history2 };

    [HideInInspector]
    public bool isEnterMission;
    public GameObject hideBG;
    public GameObject video;
    public GameObject leaveButton;

    public override void Enable()
    {
        base.Enable();

        isEnterMission = true;
        hideBG.SetActive(false);
        video.SetActive(true);

        JoeMain.Main.Start360Video(0);

        fingerClick.boxCollider.enabled = true; //open fingerClick trigger
        fingerClick.Click += ClickCount; // Add fingerClick event
    }

    void ClickCount()
    {
        clickCount++;

        if (clickCount >= 0)
        {
            StarHistory();
        }

        Debug.Log("9 clickCount: " + clickCount);
    }

    private void StarHistory()
    {
        int historyCount = historyMessage.Length + 1;

        if (clickCount == 0)
        {
            JoeMain.Main.Play360Video();
        }

        if (clickCount >= 1 && clickCount < historyCount)
        {
            dialogMissionView.Show(true);
            dialogMissionView.DialogView(dogName, historyMessage[clickCount - 1], dog);
        }

        if (clickCount == historyCount)
        {
            dialogMissionView.DialogView(dogName, dogMessage1, dog);
        }

        if (clickCount == historyCount + 1)
        {
            //TODO: End Video
            leaveButton.SetActive(true);
            leaveButton.GetComponent<Button>().onClick.AddListener(LeaveEvent);
        }
    }

    private void LeaveEvent()
    {
        dialogMissionView.Show(false);
        InitFingerClick();
        hideBG.SetActive(true);
        MissionsController.Instance.ReSetMissions();
        Debug.Log("Mission 9 Leave");
    }

    private void InitFingerClick()
    {
        fingerClick.boxCollider.enabled = false;
        fingerClick.Click -= ClickCount;
        clickCount = 0; // initial
    }
}
