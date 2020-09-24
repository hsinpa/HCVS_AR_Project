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

        public bool IsConnected => (_socket != null && _socket.IsOpen);

        public System.Action<OnSocketEventDelegate> OnSocketEvent;

        public SocketIOManager()
        {
            _socketManager = new SocketManager(new Uri(StringAsset.GetFullAPIUri(StringAsset.API.Socket)));
            _socket = _socketManager.Socket;

            _socket.On(TypeFlag.SocketEvent.OnConnect, OnConnectEvent);
            //_socket.On(TypeFlag.SocketEvent.UserJoined, OnConnectEvent);
        }

        void OnConnectEvent(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
        {
            if (args.Length > 0) {
                Debug.Log(args[0].ToString());
            }
        }

        public void Emit(string event_id, string raw_json = "{}") {
            if (!IsConnected || string.IsNullOrEmpty(event_id)) return;

            _socket.Emit(event_id, raw_json);
        }

    }
}