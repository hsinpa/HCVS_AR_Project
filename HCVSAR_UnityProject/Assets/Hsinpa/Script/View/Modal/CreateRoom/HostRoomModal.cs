using Hsinpa.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class HostRoomModal : Modal
    {
        [Header("Selections")]
        [SerializeField]
        private Dropdown YearSelection;

        [SerializeField]
        private Dropdown GradeSelection;

        [SerializeField]
        private Dropdown ClassSelection;

        [SerializeField]
        private Button ConfirnBtn;

        private List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet;
        private List<string> FilterYearList = new List<string>();
        private List<string> FilterGradeList = new List<string>();

        private List<TypeFlag.SocketDataType.ClassroomDatabaseType> FilterDatabaseList;

        private System.Action<TypeFlag.SocketDataType.ClassroomDatabaseType> OnHostRoomEvent;

        public void SetUp(System.Action<TypeFlag.SocketDataType.ClassroomDatabaseType>  OnHostRoomEvent) {
            this.OnHostRoomEvent = OnHostRoomEvent;
            YearSelection.onValueChanged.RemoveAllListeners();
            ClassSelection.onValueChanged.RemoveAllListeners();
            GradeSelection.onValueChanged.RemoveAllListeners();

            ConfirnBtn.onClick.RemoveAllListeners();

            YearSelection.onValueChanged.AddListener(OnYearGradeSelectionChange);
            GradeSelection.onValueChanged.AddListener(OnYearGradeSelectionChange);
            ClassSelection.onValueChanged.AddListener(OnClassSelectionChange);

            ConfirnBtn.onClick.AddListener(OnHostRoomBtnClick);
        }

        public void DecorateSelectionView(List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet) {
            if (classroomDataSet == null) return;

            this.classroomDataSet = classroomDataSet;

            //FilterYearList = classroomDataSet.GroupBy(test => test.year).Select(grp => grp.First().year.ToString()).ToList();
            //FilterGradeList = classroomDataSet.GroupBy(test => test.grade).Select(grp => grp.First().grade.ToString()).ToList();

            //YearSelection.ClearOptions();
            //YearSelection.AddOptions(FilterYearList);

            //GradeSelection.ClearOptions();
            //GradeSelection.AddOptions(FilterGradeList);

            //OnYearGradeSelectionChange(0);

            var classIDArray = classroomDataSet.Select(x => new UnityEngine.UI.Dropdown.OptionData($"{x.class_name} {x.class_id}")).ToList();

            ClassSelection.ClearOptions();
            ClassSelection.AddOptions(classIDArray);

            OnClassSelectionChange(0);
        }

        public void ResetAll() {
            YearSelection.ClearOptions();
            ClassSelection.ClearOptions();
            ConfirnBtn.interactable = false;
        }

        private void OnYearGradeSelectionChange(int index) {
            if (index < 0 || FilterYearList.Count <= 0) return;

            FilterDatabaseList = FindFilterClassList();

            var classIDArray = FilterDatabaseList.Select(x => new UnityEngine.UI.Dropdown.OptionData(x.class_name)).ToList();

            ClassSelection.ClearOptions();
            ClassSelection.AddOptions(classIDArray);
        }

        private void OnClassSelectionChange(int index) {
            if (index < 0) return;

            ConfirnBtn.interactable = (index >= 0 && classroomDataSet.Count >= 0);
        }

        private List<TypeFlag.SocketDataType.ClassroomDatabaseType> FindFilterClassList() {
            List<TypeFlag.SocketDataType.ClassroomDatabaseType> dataTypeList = new List<TypeFlag.SocketDataType.ClassroomDatabaseType>();
            try
            {
                int selectYear = int.Parse(FilterYearList[YearSelection.value]);
                int selectGrade = int.Parse(FilterGradeList[GradeSelection.value]);

                dataTypeList = classroomDataSet.FindAll(x => x.year == selectYear && x.grade == selectGrade);
            }
            catch {
                Debug.LogWarning("FindFilterClassList Error");   
            }

            return dataTypeList;
        }

        private void OnHostRoomBtnClick() {

            Debug.Log("OnHostRoomBtnClick " + ClassSelection.value);

            if(ClassSelection.value >= 0)
                OnHostRoomEvent(classroomDataSet[ClassSelection.value]);
        }


    }
}