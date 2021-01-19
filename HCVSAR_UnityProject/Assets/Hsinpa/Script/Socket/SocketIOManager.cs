using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.SocketIO;
using System;
using Expect.StaticAsset;

namespace Hsinpa.Socket {
    public class SocketIOManager
    {
        public delegate void OnSocketEventDelegate(string event_id, string rawData);

        private SocketManager _socketManager;
        public BestHTTP.SocketIO.Socket socket => (_socketManager == null) ? null : _socketManager.Socket;

        public string originalSocketID = null;

        public bool IsConnected => (socket != null && socket.IsOpen);

        public System.Action<OnSocketEventDelegate> OnSocketEvent;
        public System.Action<BestHTTP.SocketIO.Socket> OnSocketReconnected;

        Queue<EmitStruct> emitStructsQueue;

        private bool isUnderReconnectFlag = false;

        public SocketIOManager()
        {
            emitStructsQueue = new Queue<EmitStruct>();
        }

        public void SetUpSocketIoManager() {
            var socketOption = new SocketOptions();
            socketOption.ServerVersion = SupportedSocketIOVersions.v3;
            _socketManager = new SocketManager(new Uri(StringAsset.GetFullAPIUri(StringAsset.API.Socket)), socketOption);
            RegisterSocket(_socketManager);


        }

        private void RegisterSocket(SocketManager manager) {
            socket.On(SocketIOEventTypes.Error, OnError);
            socket.On(TypeFlag.SocketEvent.OnConnect, OnConnectEvent);
            socket.On(TypeFlag.SocketEvent.Reconnect, OnReconnect);
        }

        void OnConnectEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0) {
                Debug.Log(args[0].ToString());

                if (string.IsNullOrEmpty(originalSocketID)) {
                    Debug.Log("Original Socket " + socket.Id);
                    originalSocketID = socket.Id;
                }
                else
                    Reconnect(socket.Id);
            }
        }

        private void OnReconnect(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            Debug.Log("OnReconnect");

            if (OnSocketReconnected != null)
                OnSocketReconnected(this.socket);

            SendQueueItem();
        }

        private void Reconnect(string newSocketID) {
            isUnderReconnectFlag = false;

            TypeFlag.SocketDataType.ReconnectRequestType reconnectRequestType = new TypeFlag.SocketDataType.ReconnectRequestType();
            reconnectRequestType.reconnect_sid = newSocketID;
            reconnectRequestType.target_sid = originalSocketID;

            Emit(TypeFlag.SocketEvent.Reconnect, JsonUtility.ToJson(reconnectRequestType));
        }

        public void Emit(string event_id, string raw_json = "{}") {
            if (!IsConnected || string.IsNullOrEmpty(event_id)) {
                emitStructsQueue.Enqueue(new EmitStruct(event_id, raw_json));
                CheckAndProcessIfUnconnect();
                return;
            };

            Debug.Log("event_id " + event_id + ",raw_json " + raw_json +", Open " + socket.IsOpen);

            socket.Emit(event_id, raw_json);
        }

        public void ManulControlConnection(bool isOpen) {

            //if (isOpen) {
            //    _socketManager.Open();
            //    RegisterSocket(_socketManager);
            //}

            //else
            //    _socketManager.Close();
        }

        private void SendQueueItem() {
            for (int i = 0; i < emitStructsQueue.Count; i++) {
                var emitStruct = emitStructsQueue.Dequeue();
                Emit(emitStruct.event_id, emitStruct.json);
            }
        }

        public bool CheckAndProcessIfUnconnect() {

            if (!IsConnected && !isUnderReconnectFlag && _socketManager != null) {
                isUnderReconnectFlag = true;
                Debug.Log("_socketManager.ReconnectAttempts " + _socketManager.ReconnectAttempts);
            }

            return IsConnected;
        }


        void OnError(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            Error error = args[0] as Error;

            switch (error.Code)
            {
                case SocketIOErrors.User:
                    Debug.Log("Exception in an event handler!");
                    break;
                case SocketIOErrors.Internal:
                    Debug.Log("Internal error! Message: " + error.Message);
                    break;
                default:
                    Debug.Log("Server error! Message: " + error.Message);
                    break;
            }

            // Or just use ToString() to print out .Code and .Message:
            Debug.Log(error.ToString());
        }

        private struct EmitStruct {
            public string event_id;
            public string json;

            public EmitStruct(string p_event, string p_json) {
                this.event_id = p_event;
                this.json = p_json;
            }
        }
    }
}