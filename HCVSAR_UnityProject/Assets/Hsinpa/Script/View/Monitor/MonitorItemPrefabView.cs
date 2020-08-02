using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonitorItemPrefabView : MonoBehaviour
{
    [SerializeField]
    private Text InfoText;

    [SerializeField]
    private Image avatar;

    public void SetNameAndID(string studentName, string studentID) {
        InfoText.text = studentName + "\n" + studentID;

        gameObject.name = studentID;

        ChangeStatus(false);
    }

    public void ChangeStatus(bool isOnline) {
        avatar.color = (isOnline) ? Color.white : Color.gray;
    }

}
