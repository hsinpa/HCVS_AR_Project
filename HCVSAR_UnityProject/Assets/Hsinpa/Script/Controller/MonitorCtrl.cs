using BestHTTP.SocketIO;
using Expect.StaticAsset;
using Hsinpa.Socket;
using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using Expect.View;

namespace Hsinpa.Controller
{
    public class MonitorCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private MonitorPanelView _monitorView;

        [Header("Sprites")]
        [SerializeField]
        private Sprite GameStartSprite;

        [SerializeField]
        private Sprite TerminateStartSprite;

        private TypeFlag.SocketDataType.ClassroomDatabaseType selectedRoomData;
        private TypeFlag.SocketDataType.LoginDatabaseType userDataInfo;

        private List<TypeFlag.SocketDataType.StudentDatabaseType> allStudentData;
        private SocketIOManager _socketIOManager;
        private MissionItemSObj _missionItemSObj;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case GeneralFlag.ObeserverEvent.ShowMonitorUI:
                    userDataInfo = (TypeFlag.SocketDataType.LoginDatabaseType)p_objects[0];
                    selectedRoomData = (TypeFlag.SocketDataType.ClassroomDatabaseType) p_objects[1];

                    _monitorView.Show(true);

                    PrepareStudentData(selectedRoomData);
                    break;
            }
        }

        public void SetUp(SocketIOManager socketIOManager)
        {
            _socketIOManager = socketIOManager;
            RegisterSocketEvent();

            _socketIOManager.OnSocketReconnected += OnReconnect;

            _missionItemSObj = MainApp.Instance.database.MissionItemSObj;

            _monitorView.SetUp(OnGameStartBtnClickEvent, OnTerminateBtnClickEvent, OnMoreInfoBtnClickEvent, 
                            OnLogoutClick, OnStudentObjClick, OnTimeUp);
        }

        private void PrepareStudentData(TypeFlag.SocketDataType.ClassroomDatabaseType selectedRoomData) {
            string getStudentURI = string.Format(StringAsset.API.GetAllStudentByID, selectedRoomData.class_id, selectedRoomData.year);

            _monitorView.ResetContent();

            Debug.Log(getStudentURI);

            StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(getStudentURI), UnityWebRequest.kHttpVerbGET, null, (string json) => {
                if (string.IsNullOrEmpty(json))
                {
                    return;
                }

                var tempStudentData = JsonHelper.FromJson<TypeFlag.SocketDataType.StudentDatabaseType>(json);

                if (tempStudentData != null)
                {
                    allStudentData = tempStudentData.ToList();

                    string fullRoomName = string.Format("{0}年 {1} ({2})", selectedRoomData.year, selectedRoomData.class_name, selectedRoomData.class_id);
                    _monitorView.SetContent(fullRoomName, allStudentData);

                    RequestUsersRefresh();
                }
            }, null));
        }

        #region Button Event
        private void OnGameStartBtnClickEvent(Button btn) {
            var dialogueModal = Modals.instance.OpenModal<DialogueModal>();
            dialogueModal.SetDialogue(StringAsset.UserInfo.GameStartTitle, StringAsset.UserInfo.GameStartDesc,
                new DialogueModal.ButtonType[] { DialogueModal.ButtonType.Accept, DialogueModal.ButtonType.Cancel },
                (x) =>
                {
                    Modals.instance.Close();

                    if (x == DialogueModal.ButtonType.Accept) {
                        string roomJSONString = string.Format("{{\"room_id\" : \"{0}\"}}", selectedRoomData.class_id);
                        _socketIOManager.socket.Emit(TypeFlag.SocketEvent.StartGame, roomJSONString);

                        btn.interactable = false;
                    }
                }
            );

            dialogueModal.DecorateSideImage(GameStartSprite);
        }

        private void OnTerminateBtnClickEvent(Button btn)
        {
            var buttonTypes = new DialogueModal.ButtonType[] { DialogueModal.ButtonType.Accept, DialogueModal.ButtonType.Cancel};

            ShowTerminateModal(btn, StringAsset.UserInfo.GameTerminateDesc, buttonTypes);
        }

        private void OnLogoutClick(Button btn)
        {
            var dialogueModal = Modals.instance.OpenModal<DialogueModal>();

            dialogueModal.SetDialogue(StringAsset.UserInfo.TeacherLeaveTitle, StringAsset.UserInfo.TeacherLeaveDesc,
                new DialogueModal.ButtonType[] { DialogueModal.ButtonType.Accept, DialogueModal.ButtonType.Cancel }, (x) =>
                {
                    if (x == DialogueModal.ButtonType.Accept)
                    {
                        _monitorView.Show(false);
                        _monitorView.ResetContent();

                        EmitTerminateEvent(selectedRoomData.class_id, _missionItemSObj.defaultMission.mission_id);

                        MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.ShowHostRoomUI, userDataInfo);
                    }
                }
            );

            dialogueModal.DecorateSideImage(TerminateStartSprite);


            //MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.ShowUserInfo, studentItem.studentDatabaseType, studentItem.isOnline);
        }


        private void OnMoreInfoBtnClickEvent(Button btn)
        {
            MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.ShowClassScore, selectedRoomData);
        }

        private void OnStudentObjClick(MonitorItemPrefabView studentItem) {
            MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.ShowUserInfo, studentItem.studentDatabaseType, studentItem.isOnline);
        }

        private void OnTimeUp() {
            var buttonTypes = new DialogueModal.ButtonType[] { DialogueModal.ButtonType.Accept };

            ShowTerminateModal(null, StringAsset.UserInfo.TimeUpTerminateDesc, buttonTypes);
        }

        #endregion

        #region Socket Section
        private void OnUserJoinEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0)
            {
                var userComp = JsonUtility.FromJson<TypeFlag.SocketDataType.UserComponentType>(args[0].ToString());
                if (_monitorView.isShow) _monitorView.SetUserConnectionType(true, userComp);
            }
        }

        private void OnUesrLeaveEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0) {
                var userComp = JsonUtility.FromJson<TypeFlag.SocketDataType.UserComponentType>(args[0].ToString());
                if (_monitorView.isShow) _monitorView.SetUserConnectionType(false, userComp);
            }
        }

        private void OnRefreshUserStatusEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0)
            {
                var userComps = JsonHelper.FromJson<TypeFlag.SocketDataType.UserComponentType>(args[0].ToString());
                if (_monitorView.isShow) _monitorView.SyncUserStateByArray(userComps);
            }
        }

        private void OnGameStartSocketEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0)
            {
                var roomComps = JsonUtility.FromJson<TypeFlag.SocketDataType.RoomComponentType>(args[0].ToString());

                if (_monitorView.isShow) _monitorView.SetTimerAndGameStart(roomComps.end_time);
            }
        }

        //For test purpose, since teacher can ignore this message
        private void OnKickFromGameEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0)
            {
                Debug.Log(args[0].ToString());
            }
        }

        private void EmitTerminateEvent(string class_id, string location_id) {
            string jsonString = string.Format("{{\"room_id\" : \"{0}\", \"location_id\" : \"{1}\"}}",
                                                class_id, location_id);

            _socketIOManager.socket.Emit(TypeFlag.SocketEvent.TerminateGame, jsonString);

        }

        private void OnReconnect(BestHTTP.SocketIO.Socket socket) {
            RequestUsersRefresh();
            RegisterSocketEvent();
        }

        private void RequestUsersRefresh() {
            TypeFlag.SocketDataType.AccessTokenType accessTokenType = new TypeFlag.SocketDataType.AccessTokenType();
            accessTokenType.socket_id = _socketIOManager.originalSocketID;
            _socketIOManager.Emit(TypeFlag.SocketEvent.RefreshUserStatus, JsonUtility.ToJson(accessTokenType));
        }

        #endregion

        private void ShowTerminateModal(Button btn, string terminateContentText, DialogueModal.ButtonType[] buttonTypes) {
            var dialogueModal = Modals.instance.OpenModal<DialogueModal>();

            MissionItemSObj missionItemSObj = MainApp.Instance.database.MissionItemSObj;

            dialogueModal.SetDialogue(StringAsset.UserInfo.GameTerminateTitle, terminateContentText,
                new DialogueModal.ButtonType[] { DialogueModal.ButtonType.Accept, DialogueModal.ButtonType.Cancel },
                (x) =>
                {
                    if (x == DialogueModal.ButtonType.Accept)
                    {
                        int dropDownIndex = dialogueModal.dropDownMenu.value;

                        //string location_id = missionItemSObj.missionArray[dropDownIndex].mission_id;

                        string location_id = missionItemSObj.defaultMission.mission_id;

                        if (!string.IsNullOrEmpty(dialogueModal.inputValue) && dialogueModal.inputValue.Length > 1) {
                            location_id = dialogueModal.inputValue;
                        }

                        EmitTerminateEvent(selectedRoomData.class_id, location_id);

                        _monitorView.ResetTime();

                        if (btn != null)
                            btn.interactable = false;
                    }
                }
            );

            dialogueModal.DecorateSideImage(TerminateStartSprite);

            //dialogueModal.SetDropDown(missionItemSObj.missionArray.Select(x => x.mission_name).ToArray());

            dialogueModal.SetInputField("");
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!_monitorView.isShow) return;

            if (Input.GetKeyDown(KeyCode.O))
            {
                _socketIOManager.ManulControlConnection(true);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                _socketIOManager.ManulControlConnection(false);
            }
        }
#endif

        private void RegisterSocketEvent() {
            _socketIOManager.socket.On(TypeFlag.SocketEvent.UserJoined, OnUserJoinEvent);
            _socketIOManager.socket.On(TypeFlag.SocketEvent.UserLeaved, OnUesrLeaveEvent);
            _socketIOManager.socket.On(TypeFlag.SocketEvent.RefreshUserStatus, OnRefreshUserStatusEvent);
            _socketIOManager.socket.On(TypeFlag.SocketEvent.StartGame, OnGameStartSocketEvent);
            _socketIOManager.socket.On(TypeFlag.SocketEvent.KickFromGame, OnKickFromGameEvent);
        }

    }
}