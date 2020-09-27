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

    public class GoogleDocVar {
        public const string Version = "Version";
    }

    public class ARZero {
        public const string TakeOff_Anim = "anim_event@takeoff";

        public const string SpecialSkinObjName = "Skin_Extra_Reward";

        public const string TrackImage_1 = "zero_maker_skin_01";
        public const string TrackImage_2 = "zero_maker_skin_02";
    }

    public class PlayerPrefKey {
        public const string ZeroJet_Skin = "HAS_AIRPLANE_SKIN";
    }
}
