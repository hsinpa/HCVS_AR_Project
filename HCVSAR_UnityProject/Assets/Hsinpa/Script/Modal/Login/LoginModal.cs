using System.Collections;
using System.Collections.Generic;
using Expect.StaticAsset;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class LoginModal : MonoBehaviour
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
        #endregion

        #region Parameter
        private TypeFlag.UserType _currentTab;
        private System.Action<string, string, TypeFlag.UserType> _loginEvent;
        #endregion

        private void Start()
        {
            RegisterTabEvent();
            RegisterButtonEvent();

            SetTabEvent(StudentTab, TypeFlag.UserType.Student, StringAsset.Login.StudentInputLabel);
        }

        public void SetUp(System.Action<string, string, TypeFlag.UserType> loginEvent) {
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

        }

        private void SetTabEvent(Button tab, TypeFlag.UserType usertype, string userIDLabel)
        {
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
                tabBtn.targetGraphic.color = Color.gray;
            }

            onClickTab.targetGraphic.color = Color.white;
        }
    }
}