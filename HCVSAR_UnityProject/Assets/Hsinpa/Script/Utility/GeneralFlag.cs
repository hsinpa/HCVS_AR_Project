using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFlag
{
    public class Regex {
        public const string UniversalSyntaxRex = @"[0-9a-zA-Z@,-_+]{6,30}";
    }

    public class ObeserverEvent {
        public const string HostRoomShowUI = "event@show_create_hostroom";
    }
}
