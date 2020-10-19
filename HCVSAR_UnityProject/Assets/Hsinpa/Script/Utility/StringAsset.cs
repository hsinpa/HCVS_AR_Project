

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
            public const string StudentNameInputError = "姓名格式錯誤 : 字數6-20";
            public const string ClassIDInputError = "班級代碼格式錯誤 : 字數6-20";

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

        public class ConnectTeacherInfo
        {
            public const string school = "高雄市立海青高直工商職業學校";
            public const string add = "813009 高雄市左營區左營大路1號";
            public const string teacherName = "課堂老師：曾敏泰 老師";
            public const string schoolPhone = "學校電話：";
            public const string teacherPhone = "老師電話：";
        }

        public class BagObjectInfo
        {
            public const string mail = "不知道是誰遺落的信件，要送去學校門口的樣子";
            public const string map1 = "一張神秘地圖";
            public const string map2 = "一張神秘地圖";
            public const string mapAll = "地方城牆地圖";

            public const string mailDetail = "不知道誰遺落的信件，上面寫著海軍子弟學校收，如果是重要信件就糟糕了！幫他送過去吧！";
            public const string map1Detail = "一張神秘地圖";
            public const string map2Detail = "一張神秘地圖";
            public const string mapAlDetail = "一張完整的地圖，上面記載了這塊土地上曾經出現過的鳳山舊城城牆的位置，分別有東門、西門、南門、北門。\n有些城牆在後期已經被拆除了，真是歷史悠久又珍貴的一張地圖啊！";
        }

        public class EnterMission
        {
            public const string enterMission = "在附近感測到能量提示\n您即將進入\n確定要進入此處尋找能量嗎？";
            public const string enterEndMission = "您即將進入最終關卡\n您即將進入\n進入後遊戲體驗將會結束";
        }

        public class MissionsSituation
        {
            public class Zero
            {
                public const string s1 = "樂咖和玩家看到了前方有一群人湊上前去觀看。";
            }

            public class Two
            {
                public const string s1 = "樂咖和玩家順著增測儀走到了定點，看到了很多人在拜拜。";
            }

            public class Three
            {
                public const string s1 = "一段時間後，砲火的聲響漸漸退去，樂卡與玩家們離開防空洞，回到戶外。";
            }

            public class Four
            {
                public const string s1 = "樂咖和玩家路過校園，看到在煩惱的學生們。";
            }

            public class Five
            {
                public const string s1 = "狗狗依著指針來到了建築物附近。";
                public const string fault = "路人已經走遠。";
            }

            public class SEVEN
            {
                public const string s1 = "樂咖和玩家看到了前方有一群人湊上前去觀看。";
            }

            public class Tool
            {
                public const string m1 = "拾獲信件一封";
                public const string m2 = "您拾獲一張神秘地圖的缺角";
            }
        }

        public class MissionsDialog
        {
            public class Person
            {
                public const string dog = "樂咖";
                public const string map = "地圖";
                public const string NPC_1 = "村民";
                public const string NPC_2 = "路人";
                public const string NPC_3 = "學生";
                public const string NPC_4 = "老人";
                public const string NPC_5 = "兒玉源太郎";
                public const string NPC_6 = "警衛";
            }

            public class Zero
            {
                public const string d1 = "你們在做什麼啊？為什麼要拉倒城牆呢？";
                public const string d2 = "因為日本海軍要擴大營區，所以要把清朝的城牆拆除。\n我們就被召集來出力了！你沒事的話快來幫忙吧！";

                public const string history1 = "各位同學今天看到的城牆跟城門，道光五年（1825）由清朝官府動員民間捐款興建的，是台灣當時第一座石頭城，至今已經快兩百歲了呢！";
                public const string history2 = "由於清朝沒什麼錢，所以是直接從柴山上面採珊瑚礁岩，也就是俗稱的咾咕石來蓋城牆城門拱則是用來自福建的花崗岩。";
                public const string history3 = "整座城有東（鳳儀）、西（奠海）、南（啟文）、北（拱辰）四個城門，上有三川脊式城樓，城門間有東、西、南、北四座砲台。";
                public const string history4 = "還有排水的水關等設計，在當年可說是相當先進呢！";
            }

            public class One
            {
                public const string d1 = "你好我在路上撿到了這封信，請問是送到這裡嗎？";
                public const string d2 = "等等！這是什麼！";
                public const string d3 = "是一封信件呢！我先幫你收到包包裡喔！";
                public const string correct_1 = "哎呀！這是相當重要的信件呢，今天都在等這封信！真是太感謝你了。\n這裡沒有什麼厲害的東西，不然這個給你吧。";
                public const string fault_1 = "這上面寫的是海軍子弟學校收，這裡是高雄市立海青工商的校門口喔，是不是有什麼事情搞錯了呢？";
                public const string fault_2 = "原來如此啊！\n那這封信我應該…\n阿！信件不見了啊！";

                public const string history1 = "海青工商的地址是：｢高雄市左營區左營大路1號」，為什麼大門口明明在必勝路，地址卻寫左營大路勒？";
                public const string history2 = "因為海青以前舊校門位於左營大路旁，校門設計者是首任校長安世琪，\n他參考了帕德嫩神殿山牆的概念以及希臘雅典的柱頭設計，完成了這座美輪美奐的大門。";
                public const string history3 = "可惜後來為了交通安全考量，將校門遷移至今必勝路位置，舊校門就被拆除了。";
            }

            public class Two
            {
                public const string history1 = "海青工商的校地大部分是為在以前鳳山舊城的城內喔，大門口左前方是西門遺址，後門是南門。";
                public const string history2 = "2014年校園曾經有考古挖掘，我有看到地底下有挖到清代文化層跟建築的地面呢。";
                public const string history3 = "根據我敏銳的嗅覺判斷，這可能是清代廣濟宮的一部份喔！";
            }

            public class Three
            {
                public const string correct_1 = "這裡也有能量！太好了！";
                public const string fault_1 = "阿阿阿阿阿阿，快逃啊！";

                public const string history1 = "各位同學這個防空洞啊，是二次大戰後期「震洋特攻隊」為了躲避美軍轟炸而蓋的防空洞。";
                public const string history2 = "都蓋在大樹下面來躲避美軍偵察，而「震洋特攻隊」任務就是等待有一天美軍要登陸台灣的時候，駕著快艇跟炸藥跟美軍軍艦同歸於盡！";
                public const string history3 = "原本海青工商裡面有六座這樣子的防空洞，但現在只剩下這一座了！";
            }

            public class Four
            {
                public const string p1 = "怎麼辦呢？";
                public const string p2 = "我們是即將從這裡畢業的第一屆畢業學生。\n要在這些木造建築前面拍照拍畢業照呢！不過攝影師剛剛說不舒服去了廁所，然後就不見了！\n再不拍攝我們會趕不上上課時間……";
                public const string d1 = "怎麼了呢？";
                public const string d2 = "我們一起幫他拍照吧！";
                public const string correct_1 = "來！你們看一下。";
                public const string correct_2 = "謝謝您！這個是我們班上的東西，送給你吧！";
                public const string fault_1 = "啊～不是這樣啊，我們還是等攝影師回來吧。";

                public const string history1 = "海青工商的前身海軍子弟學校，1949年剛創校的時候很窮啊，什麼都沒有。";
                public const string history2 = "所以只好把日本海軍震洋特攻隊留下來的十二棟木造房舍當作教室";
                public const string history3 = "學校很多設備都還是跟海軍要來的呢。";
            }

            public class Five
            {
                public const string d1 = "你好，我有事情要找安世琪校長，據說校長室是一間北方宮殿建築風格，您知道在哪裡嗎？";
                public const string correct_1 = "應該是這棟建築物喔。";
                public const string correct_2 = "喔喔！謝謝您。\n這是一點小心意，希望你可以收下。";
                public const string fault_1 = "好像不是噎...好吧，我再去問問其他人。";
                public const string fault_2 = "阿！前方這棟建築物就是他在找的建築物啊！真是糊塗了！";

                public const string history1 = "今日的校史室可說是現在海青工商校園內最老的建築物喔，";
                public const string history2 = "實際建造時間大概在於1961年左右，今天也六十歲了呢，以前它是全校最高的建築物，二樓是校長室，可以鳥瞰全校園喔！";
                public const string history3 = "遠遠看是不是有點像紫禁城呢？";
            }

            public class Six
            {
                public const string d1 = "我們一起幫助他吧！";
                public const string p1 = "同學同學，你有看到那個平台嗎？請問你可以協助我爬到那個平台上嗎？";
                public const string correct_1 = "成功到平台上，真是太好了！";
                public const string fault_1 = "哎呀！不小心摔倒了啊！不過沒關係我還是靠自己吧！";

                public const string history1 = "摳尼基挖！挖搭係挖兒玉源太郎壘斯！";
                public const string history2 = "我是日本時代台灣第四任總督，也是任期第二長的總督，我可是當了八年呢！";
                public const string history3 = "雖然我常常不在台灣，有時候跑去日本，有時候跑去東北打日俄戰爭，但好險我有個好幫手，也就是民政長官後藤新平，協助我完成許多台灣基礎建設。";
                public const string history4 = "1903為了紀念我就職五週年，由高雄好野人陳中和出錢幫我在柴山上立塑像，可惜戰爭打完我就不見了\n剩下的基座在1957年由安世琪校長搬到海青校園內。";
                public const string history5 = "謝謝你的幫助，這是我的一點點心意，你應該用得上吧！\n撒唷哪拉～";
            }

            public class Seven
            {
                public const string d1 = "真的是好壯闊的城牆啊！\n咦地上這是什麼東西！我們上前去看看吧？哇 這裡好多人！是什麼活動呢？\n我們上前去看看吧？";

                public const string history1 = "喔～原來是中華民國總統蔣中正的夫人宋美齡來訪啊！";
                public const string history2 = "她可是｢中華婦女反共抗俄聯合會」，簡稱｢婦聯會」的主任委員喔，台灣各地很多眷村當年都是｢婦聯會」出錢幫忙蓋的呢！";
                public const string history3 = "對當時左營地區海軍軍眷照顧也不餘遺力喔，他們在參觀完海軍子弟學校後待會要前往對面的海強幼稚園繼續訪視喔！";
            }

            public class Eight
            {
                public const string d1 = "真的是好壯闊的城牆啊！\n咦地上這是什麼東西！我們上前去看看吧？";
                public const string d2 = "不知道是哪裡的地圖，而且他破損了，我先幫你把他收到包包裡喔！";

                public const string history1 = "海青工商對面這塊土地，在三千年以前住著一群史前文化人，到清朝統治時曾有官衙署建築群。";
                public const string history2 = "到了日本時期又出租給日本人開墾當果園，二次大戰期間被海軍省徵收做為左營軍港擴建用地。";
                public const string history3 = "戰爭結束1949年後曾有很長一段時間被當眷村，當年學校對面就是早餐街，我沒事就會晃過去討點東西吃呢！";
                public const string history4 = "後來2013年眷村拆除發現地底下跟地上都有很多歷史遺跡，所以就成為全台第一座歷史遺址公園預定地囉！";
            }

            public class Nine
            {
                public const string d1 = "我們終於到了飛船的地方了！\n這些城門們可以完整地繞一個圓啊！";

                public const string history1 = "各位同學看到的是鳳山舊城南門，是目前唯一一座被復原城門樓的城門喔。";
                public const string history2 = "雖然城門樓在民國時候重建時樣子蓋錯了，不過大家還是可以想像一下啦，這座城門會陪伴大家三年喔！";
            }

            public class Map
            {
                public const string d1 = "難道說！";
                public const string d2 = "原來如此，這張地圖是這個地方城牆的地圖，而我們的正前方是城牆啊！";
                public const string map1 = "這裡的城牆後期將被拆除，改成眷村，而最後眷村也會被拆除，變成其他設施吧。\n謝謝你幫我找回了失去的部分，非常謝謝你。";
                public const string map2 = "獲得隱藏手作ＡＲ戰鬥機模型";
            }
        }

        public class MissionsQustion
        {
            public class Zero
            {
                public const string qustion = "Q:請問為什麼好端端清代城牆跟城門要被日本人拆除呢？";
                public const string correct = "恭喜你答對了！正確答案為因為隨著日本海軍軍區擴大，土地不夠。所以現在需要將部分城牆做拆除。";
                public const string fault = "不對喔！正確答案是因為隨著日本海軍軍區擴大，土地不夠。所以現在需要將部分城牆做拆除。";
            }

            public class Two
            {
                public const string qustion = "Q:你們猜猜廣濟宮是拜什麼神明的呢？";
                public const string correct = "恭喜你答對了！正確答案三山國王。";
                public const string fault = "不對喔！正確答案為三山國王。";
            }

            public class Five
            {
                public const string qustion = "Q:學生要尋找的，北方建築是哪一個呢？";
            }

            public class SEVEN
            {
                public const string qustion = "Q:為什麼1950年代左營會有這麼多眷村呢？";
                public const string correct = "恭喜你答對了！正確答案為因左營是海軍的大本營大批軍眷入住。所以1950年代開始了在左營的眷村文化。";
                public const string fault = "不對喔！正確答案為因左營是海軍的大本營大批軍眷入住。所以1950年代開始了在左營的眷村文化。";
            }
        }

        public class MissionsAnswer
        {
            public class Zero
            {
                public const string ans1 = "城牆跟城門年久失修有倒塌疑慮";
                public const string ans2 = "因為隨著日本海軍軍區擴大，土地不夠";
                public const string ans3 = "城門跟城牆擋住日本人參拜天皇的方位";
                public const string ans4 = "因為風水問題被拆除";
            }

            public class Two
            {
                public const string ans1 = "保生大帝";
                public const string ans2 = "三山國王";
                public const string ans3 = "觀音佛祖";
                public const string ans4 = "媽祖";
            }

            public class Five
            {
                public const string ans1 = "校史室";
                public const string ans2 = "紫禁城";
                public const string ans3 = "北港朝天宮";
                public const string ans4 = "教學大樓";
            }

            public class SEVEN
            {
                public const string ans1 = "左營地靈人傑大家都想搬來這住";
                public const string ans2 = "因左營是海軍的大本營大批軍眷入住";
                public const string ans3 = "左營地價便宜房屋買得起";
                public const string ans4 = "因為鄰近高鐵方便去各地玩";
            }
        }

        public class MissionsEnd
        {
            public class End
            {
                public const string message = "獲得得{0}能量，快點搜集更多能量吧！";
            }
        }

        public class Domain {
            public const string LocalHost = "http://localhost:8020/";
            public const string GooglePoorServer = "http://34.82.74.32:81/";
            public const string GoogleYuriServer = "http://35.197.38.83:81/";
        }

        public class API {
            public const string Socket = "socket.io/";
            public const string Login = "login";
            public const string Register = "register";

            public const string GetAllClassInfo = "getAllClassInfo";
            public const string GetAllStudentByID = "getAllStudentByID/{0}/{1}";
            public const string GetStudentScore = "getStudentScore/{0}";
            public const string GetClassScore = "getClassScoreInfo/{0}";
            public const string GetStudentRank = "getStudentRank/{0}";
            public const string ManualDisconnect = "manual_leave/{0}";

            public const string PostStudentScore = "Insert_student_record";
        }

        public static string GetFullAPIUri(string apiUrl)
        {
            return Domain.GoogleYuriServer + apiUrl;
        }

    }
}
