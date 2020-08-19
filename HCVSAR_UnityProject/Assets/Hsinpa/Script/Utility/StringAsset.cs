

using UnityEngine;

namespace Expect.StaticAsset {
    public class StringAsset
    {
        public class Login {
            public const string AccountInputLabel = "登錄帳號";
            public const string PasswordInputLabel = "登錄密碼";
            public const string StudentNameInputLabel = "學生姓名";
            public const string ClassIDInputLabel = "班級代碼";

            public const string UserIDInputError = "帳號格式錯誤 : 字數6-20";
            public const string PasswordInputError = "密碼格式錯誤 : 字數6-20";
            public const string ServerLoginError = "帳號密碼不符合: 查無此筆資料";
            public const string ServerRegisterError = "帳號已存在, 或班級代號錯誤";
        }

        public class UserInfo
        {
            public const string HeaderUserInfo = "海青工商\n使用者 {0} {1}\n連線狀況 : <color={2}>{3}</color>";
            public const string Online = "正常";
            public const string Offline = "離線";

            public const string OnlineColor = "#52ED66";
            public const string OfflineColor = "#CACACA";

            public const string GameStartTitle = "開始遊戲";
            public const string GameStartDesc = "遊戲開始後, 同學們需要在40分鐘內完成探索";

            public const string GameTerminateTitle = "結束遊戲";
            public const string GameTerminateDesc = "所有同學請到以下地點集合";

            public const string TeacherLeaveTitle = "離開監控頁面";
            public const string TeacherLeaveDesc = "你將回到開房畫面";

            public const string TimeUpTerminateDesc = "遊戲時間已達<color=#eb4f3b>40分鐘</color>, 請同學到以下地點集合";
        }

        public class ClassInfo {
            public const string Title = "班級資料\n{0}";

            public const string Participant = "闖關人數";
            public const string AverageScore = "平均得點";

            public readonly static Color32 ParticipantColor = new Color32(142, 233, 230, 255);
            public readonly static Color32 AvgSCoreColor = new Color32(237, 241, 122, 255);

        }

        public class Domain {
            public const string LocalHost = "http://localhost:8020/";
            public const string TestServer = "http://34.82.74.32:81/";
        }

        public class API {
            public const string Socket = "socket.io/";
            public const string Login = "login";
            public const string Register = "register";

            public const string GetAllClassInfo = "getAllClassInfo";
            public const string GetAllStudentByID = "getAllStudentByID/{0}/{1}";
            public const string GetStudentScore = "getStudentScore/{0}";
            public const string GetClassScore = "getClassScoreInfo/{0}";

        }

        public static string GetFullAPIUri(string apiUrl)
        {
            return Domain.TestServer + apiUrl;
        }

    }
}
