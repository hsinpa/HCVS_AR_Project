

namespace Expect.StaticAsset {
    public class StringAsset
    {
        public class Login {
            public const string StudentInputLabel = "學生學號";
            public const string TeacherInputLabel = "教師帳號";
            public const string PasswordInputLabel = "登錄密碼";

            public const string UserIDInputError = "帳號格式錯誤 : 字數6-20";
            public const string PasswordInputError = "密碼格式錯誤 : 字數6-20";

            
        }

        public class UserInfo
        {
            public const string HeaderUserInfo = "海青工商\n使用者{0} {1} {2}\n連線狀況 : <color={3}>{4}</color>";
            public const string Online = "正常";
            public const string Offline = "離線";

            public const string OnlineColor = "#52ED66";
            public const string OfflineColor = "#CACACA";
        }

        public class Domain {
            public const string LocalHost = "http://localhost:8020/";
        }

        public class API {
            public const string Socket = "socket.io/";
            public const string Login = "login";
            public const string GetAllClassInfo = "getAllClassInfo";
            public const string GetAllStudentByID = "getAllStudentByID/{0}/{1}";
            public const string GetStudentScore = "getStudentScore/{0}";

        }

        public static string GetFullAPIUri(string apiUrl)
        {
            return Domain.LocalHost + apiUrl;
        }

    }
}
