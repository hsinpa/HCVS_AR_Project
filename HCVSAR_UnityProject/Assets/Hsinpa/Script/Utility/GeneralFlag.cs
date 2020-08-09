using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFlag
{
    public class Regex {
        public const string UniversalSyntaxRex = @"^.{2,20}$";
    }

    public class ObeserverEvent {
        public const string ShowHostRoomUI = "event@show_create_hostroom";
        public const string ShowMonitorUI = "event@prepare_monitor";
        public const string ShowUserInfo = "event@show_user_info";
        public const string ShowClassScore = "event@show_class_score";
    }
}
