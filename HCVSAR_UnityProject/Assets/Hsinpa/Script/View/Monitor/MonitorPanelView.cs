using System;
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

        [SerializeField]
        private Button LogoutBtn;

        [Header("Other")]
        [SerializeField]
        private Text ClassTitleTxt;

        [SerializeField]
        private Text TimerText;

        private List<TypeFlag.SocketDataType.StudentDatabaseType> allStudentData;
        private Dictionary<string, MonitorItemPrefabView> studentItemDict = new Dictionary<string, MonitorItemPrefabView>();

        private DateTime startTime = DateTime.MinValue;
        private DateTime endTime = DateTime.MinValue;

        private System.Action<MonitorItemPrefabView> OnStudentItemClickEvent;
        private System.Action OnTimeUpEvent;

        public void SetUp(OnBottomBtnClick onGameStartBtnClickEvent, OnBottomBtnClick onTerminateBtnClickEvent,
            OnBottomBtnClick onMoreInfoBtnClickEvent, OnBottomBtnClick onLogoutClickEvent, System.Action<MonitorItemPrefabView> studentItemClickEvent,
            System.Action OnTimeUpEvent
            ) {

            GameStartBtn.onClick.AddListener(() => { onGameStartBtnClickEvent(GameStartBtn); });
            GameTerminateBtn.onClick.AddListener(() => { onTerminateBtnClickEvent(GameTerminateBtn); });
            MoreInfoBtn.onClick.AddListener(() => { onMoreInfoBtnClickEvent(MoreInfoBtn); });
            LogoutBtn.onClick.AddListener(() => { onLogoutClickEvent(LogoutBtn); });

            this.OnStudentItemClickEvent = studentItemClickEvent;
            this.OnTimeUpEvent = OnTimeUpEvent;
        }

        public void SetContent(string titleText, List<TypeFlag.SocketDataType.StudentDatabaseType>allStudentData) {
            GameStartBtn.interactable = true;
            GameTerminateBtn.interactable = false;
            MoreInfoBtn.interactable = false;

            ClassTitleTxt.text = titleText;
            this.allStudentData = allStudentData;

            RenderStudentInfoToScrollView(this.allStudentData);
        }

        public void SetUserConnectionType(bool isConnect, TypeFlag.SocketDataType.UserComponentType userComp) {
            if (allStudentData == null || string.IsNullOrEmpty(userComp.user_id)) return;

            var findStudentIndex = allStudentData.FindIndex(x => x.id == userComp.user_id);

            if (findStudentIndex >= 0 && findStudentIndex < allStudentData.Count)
            {
                var itemObj = StudentContainer.GetChild(findStudentIndex).GetComponent<MonitorItemPrefabView>();
                itemObj.ChangeStatus(isConnect);
            }
            else {
                //New Student, that is not record
                var studentType = CreateStudentDatabaseType(userComp.user_id, userComp.room_id, userComp.name);
                allStudentData.Add(studentType);
                AddSingleStudent(studentType, isConnect:true);
            }
        }

        private void RenderStudentInfoToScrollView(List<TypeFlag.SocketDataType.StudentDatabaseType> allStudentData) {
            int studentCount = allStudentData.Count;

            ResetContent();

            for (int i = 0; i < studentCount; i++) {
                AddSingleStudent(allStudentData[i], isConnect:false);
            }
        }

        public void AddSingleStudent(TypeFlag.SocketDataType.StudentDatabaseType studentDataType, bool isConnect) {
            MonitorItemPrefabView prefabView;

            if (studentItemDict.TryGetValue(studentDataType.id, out MonitorItemPrefabView prefab))
            {
                prefabView = prefab;
            }
            else {
                GameObject studentObj = UtilityMethod.CreateObjectToParent(StudentContainer, StudentItemPrefab);
                prefabView = studentObj.GetComponent<MonitorItemPrefabView>();
            }

            prefabView.SetNameAndID(studentDataType);

            prefabView.SetClickEvent(this.OnStudentItemClickEvent);

            prefabView.ChangeStatus(isConnect);

            studentItemDict.Add(studentDataType.id, prefabView);
        }

        public void SyncUserStateByArray(TypeFlag.SocketDataType.UserComponentType[] userCompList) {
            if (userCompList == null) return;

            List<TypeFlag.SocketDataType.StudentDatabaseType> students = new List<TypeFlag.SocketDataType.StudentDatabaseType>(allStudentData);
            int compCount = userCompList.Length;

            for (int i = 0; i < compCount; i++) {
                if (studentItemDict.TryGetValue(userCompList[i].user_id, out MonitorItemPrefabView item)) {
                    item.ChangeStatus(true);
                }

                //Filter out user who is online;
                int removeIndex = students.FindIndex(x => x.id == userCompList[i].user_id);
                if (removeIndex >= 0)
                    students.RemoveAt(removeIndex);
            }

            int leaveStudentCount = students.Count;
            for (int i = 0; i < leaveStudentCount; i++)
            {
                if (studentItemDict.TryGetValue(students[i].id, out MonitorItemPrefabView item))
                {
                    item.ChangeStatus(false);
                }
            }
        }

        public void SetTimerAndGameStart(long endTimestamp) {
            startTime = DateTime.UtcNow;
            endTime = DateTimeOffset.FromUnixTimeMilliseconds(endTimestamp).DateTime;


            GameStartBtn.interactable = false;
            GameTerminateBtn.interactable = true;
            MoreInfoBtn.interactable = true;
        }

        public void ResetContent() {
            UtilityMethod.ClearChildObject(StudentContainer);
            studentItemDict.Clear();
            ResetTime();
        }

        public void ResetTime() {
            startTime = DateTime.MinValue;
            endTime = DateTime.MinValue;

            TimerText.text = "00:00";
        }

        private void Update()
        {
            if (endTime == DateTime.MinValue) return;

            TimeSpan t = endTime - DateTime.UtcNow;

            TimerText.text = string.Format("{0}:{1}", t.Minutes, t.Seconds);

            if (t.Seconds < 0) {
                Debug.Log("Teacher : Time up");

                if (OnTimeUpEvent != null) OnTimeUpEvent();

                endTime = DateTime.MinValue;
            }
        }

        private TypeFlag.SocketDataType.StudentDatabaseType CreateStudentDatabaseType(string id, string class_id, string name) {
            var student_type = new TypeFlag.SocketDataType.StudentDatabaseType();

            student_type.id = id;
            student_type.student_name = name;
            student_type.class_id = class_id;

            return student_type;
        }       
    
    }
}