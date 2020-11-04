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
        private BestHTTP.SocketIO.Socket _socket;
        public BestHTTP.SocketIO.Socket socket => _socket;

        public string originalSocketID = null;

        public bool IsConnected => (_socket != null && _socket.IsOpen);

        public System.Action<OnSocketEventDelegate> OnSocketEvent;
        public System.Action<BestHTTP.SocketIO.Socket> OnSocketReconnected;

        Queue<EmitStruct> emitStructsQueue;

        public SocketIOManager()
        {
            emitStructsQueue = new Queue<EmitStruct>();
        }

        public void SetUpSocketIoManager() {
            _socketManager = new SocketManager(new Uri(StringAsset.GetFullAPIUri(StringAsset.API.Socket)));
            RegisterSocket(_socketManager);
        }

        private void RegisterSocket(SocketManager manager) {
            _socket = _socketManager.Socket;
            _socket.On(TypeFlag.SocketEvent.OnConnect, OnConnectEvent);

        }

        void OnConnectEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0) {
                Debug.Log(args[0].ToString());

                if (string.IsNullOrEmpty(originalSocketID))
                    originalSocketID = socket.Id;
                else
                    Reconnect(socket.Id);
            }

            SendQueueItem();
        }

        private void Reconnect(string newSocketID) {
            TypeFlag.SocketDataType.ReconnectRequestType reconnectRequestType = new TypeFlag.SocketDataType.ReconnectRequestType();
            reconnectRequestType.reconnect_sid = newSocketID;
            reconnectRequestType.target_sid = originalSocketID;

            Emit(TypeFlag.SocketEvent.Reconnect, JsonUtility.ToJson(reconnectRequestType));

#if UNITY_EDITOR
            if (OnSocketReconnected != null)
                OnSocketReconnected(_socketManager.Socket);
#endif
        }

        public void Emit(string event_id, string raw_json = "{}") {
            if (!IsConnected || string.IsNullOrEmpty(event_id)) {
                emitStructsQueue.Enqueue(new EmitStruct(event_id, raw_json));
                _socketManager.Open();
                return;
            };

            _socket.Emit(event_id, raw_json);
        }

        public void ManulControlConnection(bool isOpen) {

            if (isOpen) {
                _socketManager.Open();
                RegisterSocket(_socketManager);
            }

            else
                _socketManager.Close();
        }

        private void SendQueueItem() {
            for (int i = 0; i < emitStructsQueue.Count; i++) {
                var emitStruct = emitStructsQueue.Dequeue();
                Emit(emitStruct.event_id, emitStruct.json);
            }
        }

        public bool CheckIfIsConnect() {
            if (!IsConnected)
                _socketManager.Open();

            return IsConnected;
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