
public class TypeFlag
{
    public enum UserType {Student, Teacher, Guest };


    public class SocketEvent {
        public const string OnConnect = "OnConnect";
        public const string UpdateUserInfo = "event@update_userInfo";
        public const string UserJoined = "event@user_joined";
        public const string RefreshUserStatus = "event@refresh_user_status";
        public const string UserLeaved = "event@user_leave";
        public const string CreateRoom = "event@create_room";
    }

    public class SocketDataType
    {
        [System.Serializable]
        public struct UserDataType
        {
            public TypeFlag.UserType userType;
            public string user_id;
            public string user_name;
            public string room_id;
        }

        [System.Serializable]
        public struct LoginDatabaseType
        {
            public bool status;
            public string username;
            public string user_id;
            public string room_id;
            public string seat;
        }

        [System.Serializable]
        public struct LoginDataStruct
        {
            public TypeFlag.UserType type;
            public string account;
            public string password;
        }

        [System.Serializable]
        public struct GeneralDatabaseType
        {
            public bool status;
            public string result;
        }

        [System.Serializable]
        public struct ClassroomDatabaseType
        {
            public string class_name;
            public string class_id;
            public int year;
        }

        [System.Serializable]
        public struct TeacherCreateMsgRoomType
        {
            public string user_id;
            public string room_id;
        }

        [System.Serializable]
        public struct StudentDatabaseType
        {
            public string id;
            public int year;
            public int semester;
            public string student_name;
            public string seat;
            public string class_id;
        }

        [System.Serializable]
        public struct UserComponentType
        {
            public string socket_id;
            public string name;
            public string user_id;
            public string room_id;
            public TypeFlag.UserType type;
            public int mobilephone;
        }

        [System.Serializable]
        public struct UserScoreType
        {
            public int id;
            public string user_id;
            public string mission_id;
            public int score;
        }
    }
}
