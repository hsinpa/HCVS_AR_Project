using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using UnityEngine.Networking;
using System.Linq;
using Hsinpa.View;
using System.Threading.Tasks;

namespace Expect.View
{
    public class RankInfoView : BaseView
    {
        [Header("Dropdown")]
        [SerializeField]
        private Dropdown DeptSelection;

        //[SerializeField]
        private Dropdown ClassSelection;

        [Header("RankInfo")]
        public Transform rankContainer;
        public Transform rankInfo;

        [Header("SiwtchPanel")]
        public Button close;
        public MainBaseVIew mainBaseVIew;

        [Header("searchButton")]
        public Button searchButton;

        private List<TypeFlag.SocketDataType.StudentRankType> studentRankData;
        private List<TypeFlag.SocketDataType.ClassroomDatabaseType> _classroomDataSet;
        private List<string> FilterDeptList = new List<string>();
        //private List<float> sameScoreList = new List<float>();
        private List<Transform> selectTransformList = new List<Transform>();

        private string class_id;
        private string student_id;
        private string searchDept;

        private void GetData()
        {
            class_id = MainView.Instance.loginData.room_id;
            student_id = MainView.Instance.loginData.user_id;
        }

        public void RankInfoStart()
        {
            GetData();

            PrepareRankData(class_id);
            GetDropdownData();            
            SwitchPanelController();
        }

        private void SwitchPanelController()
        {
            close.onClick.AddListener(() => {
                this.Show(false);
                mainBaseVIew.PanelController(false);
                RemoveShowData();
                RemoveListeners();
            });
        }

        private void PrepareRankData(string class_id)
        {
            string getRankURI = string.Format(StringAsset.API.GetStudentRank, class_id);

            StartCoroutine(
                APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(getRankURI), UnityWebRequest.kHttpVerbGET, null, (string json) =>
                {
                    if (string.IsNullOrEmpty(json))
                    {
                        return;
                    }

                    var tempStudentRankData = JsonHelper.FromJson<TypeFlag.SocketDataType.StudentRankType>(json);

                    if (tempStudentRankData != null)
                    {
                        studentRankData = tempStudentRankData.ToList();                                                                      
                        PrepareRankInfo(studentRankData);
                    }
                }, null));
        }

        // MARK: Show UI
        private void PrepareRankInfo(List<TypeFlag.SocketDataType.StudentRankType> studentRankData)
        {
            List<int> rankIndex = new List<int>();
            List<string> sameScoreID = new List<string>();
            List<float> weightScoreList = new List<float>();

            var sameScore = studentRankData.GroupBy(s => s.total_score).Where(g => g.Count() > 1).ToList();
            var rankData = studentRankData.OrderByDescending(data => data.total_score).ToList();

            foreach (var score in sameScore)
            {
                foreach(var a in score)
                {
                    sameScoreID.Add(a.student_id);
                }
            }

            for (int i = 0; i < rankData.Count; i++)
            {
                foreach (var id in sameScoreID)
                {
                    if (rankData[i].student_id == id) 
                    {
                        int recordCount = rankData[i].record_count;
                        float weightScore;

                        if (recordCount == 0) { recordCount = 1; }
                        weightScore = (rankData[i].total_score / (recordCount * 1000)) + 0.9f + rankData[i].total_score;
                        weightScoreList.Add(weightScore);
                        rankIndex.Add(i); // get same score index
                    }
                    
                }

                for (int j = 0; j < rankIndex.Count; j++)
                {
                    if (i == rankIndex[j])
                    {
                        var totalScore = rankData[i];
                        totalScore.total_score = weightScoreList[j]; // sign sameScore to origin rank data
                        rankData[i] = totalScore;
                    }
                }
            }

            RankInfo(rankData);
        }

        private void RankInfo(List<TypeFlag.SocketDataType.StudentRankType> rankData)
        {
            var reRank = rankData.OrderByDescending(data => data.total_score).ToList();
            float height = 30f;
            float containHeight = 10f; ;
            string rank;

            for (int i = 0; i < rankData.Count; i++)
            {
                if (i < 10)
                    rank = "0" + (i + 1).ToString();
                else
                    rank = (i + 1).ToString();

                Transform rankTransform = Instantiate(rankInfo, rankContainer);
                RectTransform rankRectTransform = rankTransform.GetComponent<RectTransform>();
                RectTransform containRect = rankContainer.GetComponent<RectTransform>();

                containHeight = containHeight + height;
                containRect.sizeDelta = new Vector2(0, containHeight + height);
                rankRectTransform.anchoredPosition = new Vector2(0, -height * (i + 1));
                rankTransform.Find("rank").GetComponent<Text>().text = rank;
                rankTransform.Find("name").GetComponent<Text>().text = rankData[i].student_name;
                rankTransform.Find("number").GetComponent<Text>().text = rankData[i].student_id;
                rankTransform.Find("score").GetComponent<Text>().text = Mathf.FloorToInt(rankData[i].total_score).ToString();

                selectTransformList.Add(rankTransform);
                rankTransform.gameObject.SetActive(true);
            }
        }

        private void GetDropdownData()
        {
            StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(StringAsset.API.GetAllClassInfo), UnityWebRequest.kHttpVerbGET, null, (string json) =>
            {
                var classArray = JsonHelper.FromJson<TypeFlag.SocketDataType.ClassroomDatabaseType>(json);

                if (classArray != null)
                {
                    var classroomDataSet = classArray.ToList();
                    DropDownList(classroomDataSet);
                }

            }, null));
        }

        private void DropDownList(List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet)
        {
            if (classroomDataSet == null) return;

            _classroomDataSet = classroomDataSet;

            var classIDArray = classroomDataSet.Select(x => x.class_name).ToList();
            var studentDept = classroomDataSet.Where(c => c.class_id == class_id).ToList();
            string deptIndex = "";

            foreach (var dept in studentDept) { deptIndex = dept.class_name; }
            
            DeptSelection.ClearOptions();
            DeptSelection.AddOptions(classIDArray);
            DeptSelection.value = classIDArray.IndexOf(deptIndex);

            DeptSelection.onValueChanged.AddListener(OnDeptSelectChange);
            searchButton.onClick.AddListener(SearchClick);
        }

        private void OnDeptSelectChange(int index)
        {
            if (index <= 0) return;

            searchDept = DeptSelection.options[DeptSelection.value].text;

            searchButton.interactable = (index >= 0 && _classroomDataSet.Count >= 0);
        }

        private void SearchClick()
        {
            RemoveShowData();

            var selectClassDept = from c in _classroomDataSet where c.class_name == searchDept select c;
            foreach (var s in selectClassDept) class_id = s.class_id;

            if (class_id == null) return;

            PrepareRankData(class_id);
        }

        public void RemoveShowData()
        {
            if (selectTransformList.Count > 0)
            {
                foreach (var t in selectTransformList) { Destroy(t.gameObject); }
                foreach (var t in selectTransformList) { Destroy(t.GetChild(0).gameObject); }
                selectTransformList.Clear();
            }
        }

        private void RemoveListeners()
        {
            searchButton.onClick.RemoveAllListeners();
            DeptSelection.onValueChanged.RemoveAllListeners();
        }

        /* ======= dept class seperate
        private void PerpareDropDownList(TypeFlag.SocketDataType.ClassroomDatabaseType[] classArray)
        {
            string deptIndex = "";
            string classIndex = "";
            string fullDeptName = "";

            List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet = classArray.ToList();
            FilterDeptList = classroomDataSet.GroupBy(c => c.class_name).Select(grp => grp.First().class_name.Substring(0, 3).ToString()).Distinct().ToList();

            var FilterStudentDept = classroomDataSet.Where(c => c.class_id == class_id).ToList();

            foreach (var dept in FilterStudentDept)
            {
                fullDeptName = dept.class_name;
                deptIndex = fullDeptName.Substring(0, 3);
                classIndex = fullDeptName.Substring(3, 1);
            }

            var filterDept = classroomDataSet.Where(c => c.class_name.Substring(0, 2) == fullDeptName.Substring(0, 2));
            var classNameList = filterDept.Select(c => c.class_name.Substring(3, 1)).ToList();

            ClassSelection.ClearOptions();
            ClassSelection.AddOptions(classNameList);
            ClassSelection.value = FilterDeptList.IndexOf(classIndex);

            DeptSelection.ClearOptions();
            DeptSelection.AddOptions(FilterDeptList);
            DeptSelection.value = FilterDeptList.IndexOf(deptIndex);

            DecorateSelectionView(classroomDataSet);
        }

        private void DecorateSelectionView(List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet)
        {
            if (classroomDataSet == null) return;

            List<string> deptNameList = new List<string>();
            var FilterClassDept = classroomDataSet.GroupBy(c => c.class_name.Substring(0, 3)).Distinct();

            DeptSelection.onValueChanged.AddListener(delegate { OnDeptSelectChange(classroomDataSet); });
            ClassSelection.onValueChanged.AddListener(OnClassSelectChange);
            searchButton.onClick.AddListener(delegate { SearchClick(classroomDataSet); });
        }

        private void OnDeptSelectChange(List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet)
        {
            string DeptSelect = DeptSelection.options[DeptSelection.value].text;
            var fullClassName = classroomDataSet.Where(c => c.class_name.Substring(0,3) == DeptSelect).ToList();

            if (DeptSelect.Count() <= 0) return;

            var filterDept = classroomDataSet.Where(c => c.class_name.Substring(0, 3) == DeptSelect.Substring(0, 3));
            var classNameList = filterDept.Select(c => c.class_name.Substring(3, 1)).ToList();

            searchDept = fullClassName[0].class_name;
            Debug.Log("0searchDept: " + searchDept);

            if (classNameList.Count == 1)
            {
                foreach (var f in filterDept) { searchDept = f.class_name; }
            }

            ClassSelection.ClearOptions();
            ClassSelection.AddOptions(classNameList);
        }

        private void OnClassSelectChange(int index)
        {
            string ClassSelect = ClassSelection.options[ClassSelection.value].text;
            string subSelect = searchDept.Substring(0, 3);

            if (index < 0 || ClassSelect.Count() <= 0) return;

            searchDept = subSelect + ClassSelect;
        }

        private void SearchClick(List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet)
        {
            if (searchDept == null) return;
            RemoveShowData();

            var selectClassDept = from c in classroomDataSet where c.class_name == searchDept select c;
            
            foreach (var s in selectClassDept) class_id = s.class_id;

            if (class_id == null) return;

            PrepareRankData(class_id);
        }

        public void RemoveShowData()
        {
            if (selectTransformList.Count > 0)
            {
                foreach (var t in selectTransformList) { Destroy(t.gameObject); }
                foreach (var t in selectTransformList) { Destroy(t.GetChild(0).gameObject); }
                selectTransformList.Clear();
            }
        }

        private void RemoveListeners()
        {
            searchButton.onClick.RemoveAllListeners();
            DeptSelection.onValueChanged.RemoveAllListeners();
            ClassSelection.onValueChanged.RemoveAllListeners();
        }
        */
    }
}
