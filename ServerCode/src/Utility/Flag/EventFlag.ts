export const TeacherSocketEvent = {
    CreateRoom : "event@create_room",
    StartGame : "event@start_game",
    ForceEndGame : "event@force_end_game",
    KickFromGame : "event@kick_from_game",
    RefreshUserStatus : "event@refresh_user_status",
    Rally : "event@rally"
}

export const UniversalSocketEvent = {
    UpdateUserInfo : "event@update_userInfo", // Can be used as Login
    UserJoined : "event@user_joined",
    UserLeaved : "event@user_leave",
    Reconnect : "event@reconnect",
    Disconnect : "event@disconnect"

}

