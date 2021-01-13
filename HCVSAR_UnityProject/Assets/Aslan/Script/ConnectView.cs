using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using Hsinpa.View;

namespace Expect.View
{
    public class ConnectView : BaseView
    {
        [SerializeField]
        private Text InfoText;

        //[SerializeField]
        //private Button callPhoneButton;

        [Header("SiwtchPanel")]
        public Button close;
        public MainBaseVIew mainBaseVIew;

        private string PhoneText = "tel:0911066866";
        private string schoolName = StringAsset.ConnectTeacherInfo.school;
        private string schoolAdd = StringAsset.ConnectTeacherInfo.add;
        //private string schoolPhone = StringAsset.ConnectTeacherInfo.schoolPhone;
        private string teacher = StringAsset.ConnectTeacherInfo.teacher;

        private string teacherName;

        public void ConnectStart()
        {
            GetConnectInfo();
            SwitchPanelController();
        }

        private void GetConnectInfo()
        {
            //HostText.text = MainView.Instance.teacherName;
            teacherName = MainView.Instance.teacherName;
            InfoText.text = string.Format("{0}\n{1}\n\n\n{2}{3} 老師", schoolName, schoolAdd, teacher, teacherName);
            //callPhoneButton.onClick.AddListener(CallPhone);
        }

        private void CallPhone()
        {
            Application.OpenURL(PhoneText);
        }

        private void SwitchPanelController()
        {
            close.onClick.AddListener(() => {
                this.Show(false);
                mainBaseVIew.PanelController(false);
            });
        }
    }
}
