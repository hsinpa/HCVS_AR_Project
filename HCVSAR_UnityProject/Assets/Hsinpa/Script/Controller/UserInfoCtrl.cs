using Expect.StaticAsset;
using Expect.View;
using Hsinpa.Socket;
using Hsinpa.View;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Hsinpa.Controller
{
    public class UserInfoCtrl : ObserverPattern.Observer
    {
        private SocketIOManager _socketIOManager;
        private UserInfoModal _userInfoModal;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                //Call from Teacher
                case GeneralFlag.ObeserverEvent.ShowUserInfo: {
                    if (p_objects.Length == 2)
                        // 0 => StudentDatabaseType, 1 = HasConnection
                        ShowUserInfo( (TypeFlag.SocketDataType.StudentDatabaseType) p_objects[0], (bool)p_objects[1], TypeFlag.UserType.Teacher);
                }
                break;
            }
        }

        public void SetUp(SocketIOManager socketIOManager, UserInfoModal userInfoModal)
        {
            _socketIOManager = socketIOManager;
            _userInfoModal = userInfoModal;
        }

        public void ShowUserInfo(TypeFlag.SocketDataType.StudentDatabaseType studentObj, bool isConnect, TypeFlag.UserType ownerType) {

            string uri = StringAsset.GetFullAPIUri(string.Format(StringAsset.API.GetStudentScore, studentObj.id));

            StartCoroutine(
                APIHttpRequest.NativeCurl(uri, UnityWebRequest.kHttpVerbGET, null, (string json) =>
                {
                    var scoreType = JsonHelper.FromJson<TypeFlag.SocketDataType.UserScoreType>(json);

                    var modal = Modals.instance.OpenModal<UserInfoModal>();

                    modal.SetContent(studentObj, isConnect, scoreType, ownerType);

                }, null)
            );
        }


    }
}