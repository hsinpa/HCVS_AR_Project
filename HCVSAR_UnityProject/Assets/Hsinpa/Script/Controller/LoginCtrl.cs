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

namespace Hsinpa.Controller {
    public class LoginCtrl : ObserverPattern.Observer
    {
        LoginModal _loginModal;
        SocketIOManager _socketIOManager;

        public TypeFlag.SocketDataType.LoginDatabaseType userDataInfo;

        public void SetUp(LoginModal loginModal, SocketIOManager socketIOManager) {
            _loginModal = loginModal;
            _socketIOManager = socketIOManager;

            _loginModal.SetUp(OnLoginEvent);
        }

        public void ProcessLogin() {
            Modals.instance.OpenModal<LoginModal>();
        }

        private bool ValidInputText(string user_id) {

            var match = Regex.Match(user_id, GeneralFlag.Regex.UniversalSyntaxRex);

            return match.Success;
        }

        private void OnLoginEvent(Button uiButton, TypeFlag.UserType type, string user_id, string password) {
            uiButton.enabled = false;

            if (type == TypeFlag.UserType.Guest) {
                uiButton.enabled = true;
                NextStage(type);
                return;
            }

            bool matchUserID = ValidInputText(user_id);
            bool matchPassword = ValidInputText(password);


            if (!matchUserID) {
                _loginModal.SetWarningMsg(StringAsset.Login.UserIDInputError);
                uiButton.enabled = true;
                return;
            }

            if (type == TypeFlag.UserType.Teacher && !matchPassword) {
                _loginModal.SetWarningMsg(StringAsset.Login.PasswordInputError);
                uiButton.enabled = true;
                return;
            }

            _loginModal.SetWarningMsg(null);

            //If all validation pass
            var loginDataStruct = new TypeFlag.SocketDataType.LoginDataStruct();
            loginDataStruct.account = user_id;
            loginDataStruct.password = password;
            loginDataStruct.type = type;
            
            Debug.Log(JsonUtility.ToJson(loginDataStruct));

            APIHttpRequest.Curl(StringAsset.GetFullAPIUri(StringAsset.API.Login), BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(loginDataStruct),
                (bool isSuccess, string rawData) => {
                    Debug.Log("IsSuccess " + isSuccess.ToString() + ", " + rawData);

                    if (isSuccess && !string.IsNullOrEmpty(rawData))
                    {
                        userDataInfo = JsonUtility.FromJson<TypeFlag.SocketDataType.LoginDatabaseType>(rawData);

                        if (userDataInfo.status)
                        {
                            //Modals.instance.Close();
                            UpdateSocketUserInfo(type, userDataInfo);

                            NextStage(type);
                        }
                    }

                    uiButton.enabled = true;
                });
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
            switch (type) {
                case TypeFlag.UserType.Guest:
                    Debug.Log("To Guest Stage");
                    break;

                case TypeFlag.UserType.Student:
                    Debug.Log("To Guest Student");
                    break;

                case TypeFlag.UserType.Teacher:
                    Debug.Log("To Guest Teacher");

                    MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.HostRoomShowUI, userDataInfo);

                    break;
            }
        }
    }
}
