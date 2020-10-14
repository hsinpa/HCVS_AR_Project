using Expect.StaticAsset;
using Expect.View;
using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using Hsinpa.Socket;
using UnityEngine.UI;
using Htc.ViveToolkit;
using UnityEngine.Networking;

namespace Hsinpa.Controller {
    public class LoginCtrl : ObserverPattern.Observer
    {
        private LoginModal _loginModal;
        private SocketIOManager _socketIOManager;
        private TypeFlag.SocketDataType.LoginDatabaseType userDataInfo;

        public System.Action<TypeFlag.SocketDataType.LoginDatabaseType, SocketIOManager> OnLoginEvent;

        public void SetUp(LoginModal loginModal, SocketIOManager socketIOManager) {
            _loginModal = loginModal;
            _socketIOManager = socketIOManager;


            _loginModal.SetUp(OnLoginClickEvent, OnGuestClickEvent, OnRegisterClickEvent);
        }


        public void ProcessLogin() {

            AppStartModal appStart = Modals.instance.OpenModal<AppStartModal>();
            appStart.SetConfirmBtnEvent(() =>
            {
                Modals.instance.Close();
                Modals.instance.OpenModal<LoginModal>();
            });
        }

        private bool ValidInputText(string user_id) {

            var match = Regex.Match(user_id, GeneralFlag.Regex.UniversalSyntaxRex);

            return match.Success;
        }
        #region OnLogin Button Events
        private void OnRegisterClickEvent(Button uiButton, List<LoginInputComponent> inputComponents)
        {
            uiButton.enabled = false;
            if (!HandleInputsValidation(inputComponents))
            {
                uiButton.enabled = true;
                return;
            }

            var registerDataStruct = new TypeFlag.SocketDataType.RegisterDataStruct();
            registerDataStruct.account = _loginModal.GetValueFromInputCompArray(StringAsset.Login.AccountInputLabel, inputComponents);
            registerDataStruct.name = _loginModal.GetValueFromInputCompArray(StringAsset.Login.StudentNameInputLabel, inputComponents);
            registerDataStruct.class_id = _loginModal.GetValueFromInputCompArray(StringAsset.Login.ClassIDInputLabel, inputComponents);

            StartCoroutine(APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(StringAsset.API.Register), UnityWebRequest.kHttpVerbPOST, JsonUtility.ToJson(registerDataStruct),
            (string rawData) => {
                _loginModal.SwitchTab(LoginModal.Tab.Student);

                uiButton.enabled = true;
            }, () => {
                _loginModal.SetWarningMsg(StringAsset.Login.ServerRegisterError);

                uiButton.enabled = true;
            }));
        }

        private void OnGuestClickEvent() {
            NextStage(TypeFlag.UserType.Guest);
        }

        private void OnLoginClickEvent(Button uiButton, LoginModal.Tab type, List<LoginInputComponent> inputComponents) {
            uiButton.enabled = false;

            if (!HandleInputsValidation(inputComponents)) {
                uiButton.enabled = true;
                return;
            }


            //If all validation pass
            var loginDataStruct = new TypeFlag.SocketDataType.LoginDataStruct();
            loginDataStruct.account = _loginModal.GetValueFromInputCompArray(StringAsset.Login.AccountInputLabel, inputComponents);
            loginDataStruct.password = _loginModal.GetValueFromInputCompArray(StringAsset.Login.PasswordInputLabel, inputComponents);

            loginDataStruct.type = (type == LoginModal.Tab.Student) ? TypeFlag.UserType.Student : TypeFlag.UserType.Teacher;
            
            Debug.Log(JsonUtility.ToJson(loginDataStruct));

            StartCoroutine(APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(StringAsset.API.Login), UnityWebRequest.kHttpVerbPOST, JsonUtility.ToJson(loginDataStruct),
                (string rawData) => {

                    userDataInfo = JsonUtility.FromJson<TypeFlag.SocketDataType.LoginDatabaseType>(rawData);

                    //Modals.instance.Close();
                    UpdateSocketUserInfo(loginDataStruct.type, userDataInfo);

                    NextStage(loginDataStruct.type);

                    uiButton.enabled = true;
                } , () => {
                    _loginModal.SetWarningMsg(StringAsset.Login.ServerLoginError);

                    uiButton.enabled = true;
                }));
        }
        #endregion

        private bool HandleInputsValidation(List<LoginInputComponent> inputComponents) {
            int inputCount = inputComponents.Count;
            for (int i = 0; i < inputCount; i++)
            {
                bool isValid = ValidInputText(inputComponents[i]._inputField.text);

                if (!isValid)
                {
                    Debug.Log(inputComponents[i].name);
                    string errorMsg = (inputComponents[i].isHash) ? StringAsset.Login.PasswordInputError : StringAsset.Login.UserIDInputError;

                    if (inputComponents[i].name == StringAsset.Login.StudentNameInputLabel)
                        errorMsg = StringAsset.Login.StudentNameInputError;

                    if (inputComponents[i].name == StringAsset.Login.ClassIDInputLabel)
                        errorMsg = StringAsset.Login.ClassIDInputError;

                    _loginModal.SetWarningMsg(errorMsg);
                    return false;
                }
            }

            _loginModal.SetWarningMsg(null);

            return true;
        }

        private void UpdateSocketUserInfo(TypeFlag.UserType type, TypeFlag.SocketDataType.LoginDatabaseType loginInfo) {
            if (!_socketIOManager.IsConnected) return;

            var userDataType = new TypeFlag.SocketDataType.UserDataType();
            userDataType.room_id = loginInfo.room_id;
            userDataType.userType = type;
            userDataType.user_id = loginInfo.user_id;
            userDataType.user_name = loginInfo.username;

            _socketIOManager.Emit(TypeFlag.SocketEvent.UpdateUserInfo, JsonUtility.ToJson(userDataType) );
        }

        private void NextStage(TypeFlag.UserType type) {
            userDataInfo.userType = type;

            Modals.instance.Close();

            switch (type) {
                case TypeFlag.UserType.Guest:
                    Debug.Log("To Guest Stage");
                    break;

                case TypeFlag.UserType.Student:
                    Debug.Log("To Guest Student");

                    break;

                case TypeFlag.UserType.Teacher:
                    Debug.Log("To Guest Teacher");

                    MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.ShowHostRoomUI, userDataInfo);

                    break;
            }

            if (OnLoginEvent != null)
                OnLoginEvent(userDataInfo, _socketIOManager);
        }
    }
}
