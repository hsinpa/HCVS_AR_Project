using Hsinpa.View;
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
        private Dropdown ClassSelection;

        [SerializeField]
        private Button ConfirnBtn;

        private List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet;
        private List<string> FilterYearList = new List<string>();
        private List<TypeFlag.SocketDataType.ClassroomDatabaseType> FilterDatabaseList;


        private System.Action<TypeFlag.SocketDataType.ClassroomDatabaseType> OnHostRoomEvent;

        public void SetUp(System.Action<TypeFlag.SocketDataType.ClassroomDatabaseType>  OnHostRoomEvent) {
            this.OnHostRoomEvent = OnHostRoomEvent;
            YearSelection.onValueChanged.RemoveAllListeners();
            ClassSelection.onValueChanged.RemoveAllListeners();
            ConfirnBtn.onClick.RemoveAllListeners();

            YearSelection.onValueChanged.AddListener(OnYearSelectionChange);
            ClassSelection.onValueChanged.AddListener(OnClassSelectionChange);

            ConfirnBtn.onClick.AddListener(OnHostRoomBtnClick);
        }

        public void DecorateSelectionView(List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet) {
            if (classroomDataSet == null) return;

            this.classroomDataSet = classroomDataSet;

            FilterYearList = classroomDataSet.GroupBy(test => test.year).Select(grp => grp.First().year.ToString()).ToList();

            YearSelection.ClearOptions();
            YearSelection.AddOptions(FilterYearList);

            OnYearSelectionChange(0);
            OnClassSelectionChange(0);
        }

        public void ResetAll() {
            YearSelection.ClearOptions();
            ClassSelection.ClearOptions();
            ConfirnBtn.interactable = false;
        }

        private void OnYearSelectionChange(int index) {
            if (index < 0 || FilterYearList.Count <= 0) return;

            int selectYear = int.Parse(FilterYearList[index]);
            FilterDatabaseList = classroomDataSet.FindAll(x => x.year == selectYear);
            var classIDArray = FilterDatabaseList.Select(x => new UnityEngine.UI.Dropdown.OptionData(x.class_name)).ToList();

            ClassSelection.ClearOptions();
            ClassSelection.AddOptions(classIDArray);
        }

        private void OnClassSelectionChange(int index) {
            if (index < 0) return;

            ConfirnBtn.interactable = (index >= 0 && FilterYearList.Count >= 0);
        }

        private void OnHostRoomBtnClick() {

            Debug.Log("OnHostRoomBtnClick " + ClassSelection.value);

            if(ClassSelection.value >= 0)
                OnHostRoomEvent(FilterDatabaseList[ClassSelection.value]);
        }


    }
}