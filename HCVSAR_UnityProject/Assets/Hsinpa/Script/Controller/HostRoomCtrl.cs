using BestHTTP.SocketIO;
using Expect.StaticAsset;
using Expect.View;
using Hsinpa.Socket;
using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Hsinpa.Controller
{
    public class HostRoomCtrl : ObserverPattern.Observer
    {
        private HostRoomModal _hostRoomModal;
        private SocketIOManager _socketIOManager;

        private List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet;
        private TypeFlag.SocketDataType.LoginDatabaseType userDataInfo;
        private TypeFlag.SocketDataType.ClassroomDatabaseType selectedRoomData;

        public void SetUp(HostRoomModal hostRoomModal, SocketIOManager socketIOManager)
        {
            this._hostRoomModal = hostRoomModal;
            this._socketIOManager = socketIOManager;

            _socketIOManager.socket.On(TypeFlag.SocketEvent.CreateRoom, OnSocketHostRoomEvent);
            classroomDataSet = new List<TypeFlag.SocketDataType.ClassroomDatabaseType>();

            hostRoomModal.SetUp(OnHostRoomEvent);
            _hostRoomModal.ResetAll();

            GetClassRoomData();
        }

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event) {
                case GeneralFlag.ObeserverEvent.ShowHostRoomUI:
                    userDataInfo = (TypeFlag.SocketDataType.LoginDatabaseType) p_objects[0];
                    Process();
                    break;
            }
        }

        public void Process()
        {
            //_hostRoomModal.ResetAll();
            Modals.instance.OpenModal<HostRoomModal>();

            if (classroomDataSet.Count <= 0)
                GetClassRoomData();
        }

        private void GetClassRoomData() {
            StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(StringAsset.API.GetAllClassInfo), UnityWebRequest.kHttpVerbGET, null, (string json) =>
            {
     
                var classArray = JsonHelper.FromJson<TypeFlag.SocketDataType.ClassroomDatabaseType>(json);

                if (classArray != null)
                {
                    classroomDataSet = classArray.ToList();
                    this._hostRoomModal.DecorateSelectionView(classroomDataSet);
                }

            }, null));
        }

        private void OnHostRoomEvent(TypeFlag.SocketDataType.ClassroomDatabaseType roomData) {

            if (string.IsNullOrEmpty(roomData.class_id) || !string.IsNullOrEmpty(userDataInfo.user_id)) return;

            selectedRoomData = roomData;

            var teacherCreateMsgRoomType = new TypeFlag.SocketDataType.TeacherCreateMsgRoomType();
            teacherCreateMsgRoomType.room_id = roomData.class_id;
            teacherCreateMsgRoomType.user_id = userDataInfo.user_id;

            _socketIOManager.Emit(TypeFlag.SocketEvent.CreateRoom, JsonUtility.ToJson(teacherCreateMsgRoomType));
        }

        private void OnSocketHostRoomEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args) {
            if (args.Length > 0) {
                Debug.Log(args[0].ToString());

                var HostResult = JsonUtility.FromJson<TypeFlag.SocketDataType.GeneralDatabaseType>(args[0].ToString());

                if (HostResult.status) {
                    MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.ShowMonitorUI, selectedRoomData);
                    Modals.instance.CloseAll();
                    Debug.Log("Show Teacher Navigation Screen");
                }
                else
                    Debug.Log("Show Toast");
            }
        }

    }
}