using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Hsinpa.View
{
    public class MonitorPanelView : BaseView
    {
        public delegate void OnStudentCardClick(int index);
        public delegate void OnBottomBtnClick(Button button);

        [Header("Students")]
        [SerializeField]
        private Transform StudentContainer;

        [SerializeField]
        private GameObject StudentItemPrefab;

        [Header("Buttons")]
        [SerializeField]
        private Button GameStartBtn;

        [SerializeField]
        private Button GameTerminateBtn;

        [SerializeField]
        private Button MoreInfoBtn;

        [Header("Other")]
        [SerializeField]
        private Text ClassTitleTxt;

        private List<TypeFlag.SocketDataType.StudentDatabaseType> allStudentData;
        private Dictionary<string, MonitorItemPrefabView> studentItemDict = new Dictionary<string, MonitorItemPrefabView>();

        public void SetUp(OnBottomBtnClick onGameStartBtnClickEvent, OnBottomBtnClick onTerminateBtnClickEvent, OnBottomBtnClick onMoreInfoBtnClickEvent) {
            GameStartBtn.onClick.AddListener(() => { onGameStartBtnClickEvent(GameStartBtn); });
            GameTerminateBtn.onClick.AddListener(() => { onTerminateBtnClickEvent(GameTerminateBtn); });
            MoreInfoBtn.onClick.AddListener(() => { onMoreInfoBtnClickEvent(MoreInfoBtn); });
        }

        public void SetContent(string titleText, TypeFlag.SocketDataType.StudentDatabaseType[] allStudentData) {

            GameStartBtn.interactable = true;
            GameTerminateBtn.interactable = false;
            MoreInfoBtn.interactable = false;

            ClassTitleTxt.text = titleText;
            this.allStudentData = allStudentData.ToList();

            RenderStudentInfoToScrollView(this.allStudentData);
        }

        public void SetUserConnectionType(bool isConnect, string user_id) {
            if (allStudentData == null || string.IsNullOrEmpty(user_id)) return;

            var findStudentIndex = allStudentData.FindIndex(x => x.id == user_id);

            if (findStudentIndex >= 0 && findStudentIndex < allStudentData.Count) {
                var itemObj = StudentContainer.GetChild(findStudentIndex).GetComponent<MonitorItemPrefabView>();
                itemObj.ChangeStatus(isConnect);
            }
        }

        public void GameStartAndSetTimer() {
            GameStartBtn.interactable = false;
            GameTerminateBtn.interactable = true;
            MoreInfoBtn.interactable = true;


        }

        private void RenderStudentInfoToScrollView(List<TypeFlag.SocketDataType.StudentDatabaseType> allStudentData) {
            int studentCount = allStudentData.Count;

            UtilityMethod.ClearChildObject(StudentContainer);
            studentItemDict.Clear();

            for (int i = 0; i < studentCount; i++) {
                GameObject studentObj = UtilityMethod.CreateObjectToParent(StudentContainer, StudentItemPrefab);

                MonitorItemPrefabView prefabView = studentObj.GetComponent<MonitorItemPrefabView>();

                prefabView.SetNameAndID(allStudentData[i].student_name, allStudentData[i].id);
                studentItemDict.Add(allStudentData[i].id, prefabView);
            }
        }

        public void SyncUserStateByArray(TypeFlag.SocketDataType.UserComponentType[] userCompList) {
            if (userCompList == null) return;

            int compCount = userCompList.Length;

            for (int i = 0; i < compCount; i++) {
                if (studentItemDict.TryGetValue(userCompList[i].user_id, out MonitorItemPrefabView item)) {
                    item.ChangeStatus(true);
                }
            }
        }

        
    }
}