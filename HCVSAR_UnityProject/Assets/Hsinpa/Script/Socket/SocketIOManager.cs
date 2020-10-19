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

        public SocketIOManager()
        {
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
        }

        private void Reconnect(string newSocketID) {
            TypeFlag.SocketDataType.ReconnectRequestType reconnectRequestType = new TypeFlag.SocketDataType.ReconnectRequestType();
            reconnectRequestType.reconnect_sid = newSocketID;
            reconnectRequestType.target_sid = originalSocketID;

            Emit(TypeFlag.SocketEvent.Reconnect, JsonUtility.ToJson(reconnectRequestType));

            if (OnSocketReconnected != null)
                OnSocketReconnected(_socketManager.Socket);
        }

        public void Emit(string event_id, string raw_json = "{}") {
            if (!IsConnected || string.IsNullOrEmpty(event_id)) return;

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

    }
}