using System.Collections;
using System.Collections.Generic;
using Expect.StaticAsset;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.View;

namespace Expect.View
{
    public class LoginModal : Modal
    {
        #region Inspector
        [Header("Tab")]
        [SerializeField]
        Button StudentTab;

        [SerializeField]
        Button TeacherTab;

        [Header("InputField")]
        [SerializeField]
        LoginInputComponent userIDInput;

        [SerializeField]
        LoginInputComponent passwordInput;

        [Header("Button")]
        [SerializeField]
        Button loginBtn;

        [SerializeField]
        Button guestBtn;

        [SerializeField]
        Text warningMsg;
        #endregion

        #region Parameter
        private TypeFlag.UserType _currentTab;
        private System.Action<Button, TypeFlag.UserType, string, string> _loginEvent;
        #endregion

        private void Start()
        {
            RegisterTabEvent();
            RegisterButtonEvent();

            SetTabEvent(StudentTab, TypeFlag.UserType.Student, StringAsset.Login.StudentInputLabel);
        }

        public void SetUp(System.Action<Button, TypeFlag.UserType, string, string> loginEvent) {
            this._loginEvent = loginEvent;
        }

        private void RegisterTabEvent()
        {
            passwordInput.SetTitle(StringAsset.Login.PasswordInputLabel);

            StudentTab.onClick.AddListener(() =>
            {
                SetTabEvent(StudentTab, TypeFlag.UserType.Student, StringAsset.Login.StudentInputLabel);
            });

            TeacherTab.onClick.AddListener(() =>
            {
                SetTabEvent(TeacherTab, TypeFlag.UserType.Teacher, StringAsset.Login.TeacherInputLabel);
            });
        }

        private void RegisterButtonEvent() {
            loginBtn.onClick.AddListener(() =>
            {
                if (this._loginEvent != null)
                    this._loginEvent(loginBtn, _currentTab, userIDInput._inputField.text, passwordInput._inputField.text);
            });

            guestBtn.onClick.AddListener(() =>
            {
                if (this._loginEvent != null)
                    this._loginEvent(guestBtn, TypeFlag.UserType.Guest, "","");
            });
        }

        public void SetWarningMsg(string message) {
            warningMsg.text = (string.IsNullOrEmpty(message)) ? "" : message;
            warningMsg.gameObject.SetActive(!string.IsNullOrEmpty(message));
        }

        private void SetTabEvent(Button tab, TypeFlag.UserType usertype, string userIDLabel)
        {
            SetWarningMsg(null);
            passwordInput.gameObject.SetActive(usertype == TypeFlag.UserType.Teacher);
            passwordInput.Erase();
            userIDInput.Erase();
            userIDInput.SetTitle(userIDLabel);
            _currentTab = usertype;

            HighlightTab(tab);
        }

        private void HighlightTab(Button onClickTab) {

            foreach (Transform childTab in onClickTab.transform.parent.transform) {
                Button tabBtn = childTab.GetComponent<Button>();
                tabBtn.targetGraphic.color = Color.white;
            }

            onClickTab.targetGraphic.color = Color.gray;
        }
    }
}