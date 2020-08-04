using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFlag
{
    public class Regex {
        public const string UniversalSyntaxRex = @"[0-9a-zA-Z@,-_+]{6,30}";
    }

    public class ObeserverEvent {
        public const string ShowHostRoomUI = "event@show_create_hostroom";
        public const string ShowMonitorUI = "event@prepare_monitor";
        public const string ShowUserInfo = "event@show_user_info";
    }
}
