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
        private Text ConnectText;

        [SerializeField]
        private Button callPhoneButton;

        [Header("SiwtchPanel")]
        public Button close;
        public MainBaseVIew mainBaseVIew;

        private string PhoneText = "tel:0911066866";
        private string schoolName = StringAsset.ConnectTeacherInfo.school;
        private string schoolAdd = StringAsset.ConnectTeacherInfo.add;
        private string schoolPhone = StringAsset.ConnectTeacherInfo.schoolPhone;
        private string teacherName = StringAsset.ConnectTeacherInfo.teacherName;
        private string teacherPhone = StringAsset.ConnectTeacherInfo.teacherPhone;

        public void ConnectStart()
        {
            GetConnectInfo();
            SwitchPanelController();
        }

        private void GetConnectInfo()
        {
            ConnectText.text = string.Format("{0}\n{1}\n{2}\n\n{3}\n{4}", schoolName, schoolAdd, schoolPhone, teacherName, teacherPhone);
            callPhoneButton.onClick.AddListener(CallPhone);
        }

        private void CallPhone()
        {
            Application.OpenURL(PhoneText);
        }

        private void SwitchPanelController()
        {
            close.onClick.AddListener(() => {
                this.Show(false);
                mainBaseVIew.ClosePanel();
            });
        }
    }
}
