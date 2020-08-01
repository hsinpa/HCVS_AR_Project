

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

        public class Domain {
            public const string LocalHost = "http://localhost:8020/";
        }

        public class API {
            public const string Socket = "socket.io/";
            public const string Login = "login";
            public const string GetAllClassInfo = "getAllClassInfo";
        }

        public static string GetFullAPIUri(string apiUrl)
        {
            return Domain.LocalHost + apiUrl;
        }

    }
}
