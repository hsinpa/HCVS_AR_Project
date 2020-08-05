using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Expect.View;
using Expect.StaticAsset;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace Hsinpa.Controller {
    public class ClassScoreCtrl : ObserverPattern.Observer
    {

        private TypeFlag.SocketDataType.ClassroomDatabaseType selectedRoomData;
        private MissionItemSObj _missionItemSObj;

        private TypeFlag.InGameType.MissionType[] missionArray;
        private Dictionary<string, TypeFlag.InGameType.MissionType> missionLookupTable;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case GeneralFlag.ObeserverEvent.ShowClassScore:
                    selectedRoomData = (TypeFlag.SocketDataType.ClassroomDatabaseType)p_objects[0];

                    ShowClassScore(selectedRoomData.class_id);

                    break;
            }
        }


        public void SetUp(ClassInfoModal classInfoModal) {
            missionArray = MainApp.Instance.database.MissionShortNameObj.missionArray;
            missionLookupTable = MainApp.Instance.database.MissionShortNameObj.MissionTable;
        }

        public void ShowClassScore(string class_id) {
            string getClassScoreURI = string.Format(StringAsset.API.GetClassScore, class_id);

            var classInfoModal = Modals.instance.OpenModal<ClassInfoModal>();
            classInfoModal.ResetContent();

            StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(getClassScoreURI), UnityWebRequest.kHttpVerbGET, null, (string json) => {

                var classScoreHolder = JsonUtility.FromJson<TypeFlag.SocketDataType.ClassScoreHolderType>(json);

                classInfoModal.SetTitle( string.Format(StringAsset.ClassInfo.Title, selectedRoomData.class_name));

                _ = PrepareOverallChartData(classInfoModal, classScoreHolder);

            }, null));
        }

        private async Task PrepareOverallChartData(ClassInfoModal classScoreModal, TypeFlag.SocketDataType.ClassScoreHolderType classScoreHolder) {
            classScoreModal.SetAxisLabel(missionArray.Select(x=>x.mission_name).ToList());

            var participantSet = await Task.Run(() => PrepareSelectedDataset(classScoreHolder.participant_count.ToList()));

            var avgScoreSet = await Task.Run(() => PrepareSelectedDataset(classScoreHolder.average_score.ToList()));

            classScoreModal.SetChartData(StringAsset.ClassInfo.Participant, StringAsset.ClassInfo.ParticipantColor, participantSet);
            classScoreModal.SetChartData(StringAsset.ClassInfo.AverageScore, StringAsset.ClassInfo.AvgSCoreColor, avgScoreSet);
        }

        private  TypeFlag.SocketDataType.ClassScoreType[] PrepareSelectedDataset(List<TypeFlag.SocketDataType.ClassScoreType> onlineClassScores) {
            int missionLen = missionArray.Length;
            var chartDataset = new TypeFlag.SocketDataType.ClassScoreType[missionLen];

            for (int i = 0; i < missionLen; i++) {
                var classScore = new TypeFlag.SocketDataType.ClassScoreType();

                classScore.mission_id = missionArray[i].mission_id;
                classScore.main_value = 0;

                int onlineScoreIndex = onlineClassScores.FindIndex(x => x.mission_id == classScore.mission_id);

                if (onlineScoreIndex >= 0)
                    classScore.main_value = onlineClassScores[onlineScoreIndex].main_value;

                chartDataset[i]= (classScore);
            }

            return chartDataset;
        }
    }
}
