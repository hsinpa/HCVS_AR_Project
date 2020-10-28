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

        [SerializeField]
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
        private List<string> FilterDeptList = new List<string>();
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

                        RankInfo(studentRankData);
                        //_ = PrepareRankInfo(studentRankData);
                    }
                }, null));
        }

        private void RankInfo(List<TypeFlag.SocketDataType.StudentRankType> studentRankData)
        {
            var sameScore = studentRankData.GroupBy(s => s.total_score).Where(g => g.Count() > 1).ToList();
            var rankData = studentRankData.OrderByDescending(data => data.total_score).ToList();

            float height = 30f;
            float containHeight = 10f; ;
            string rank;
            List<int> rankIndex = new List<int>();
            List<float> sameScoreList = new List<float>();

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
                rankTransform.Find("score").GetComponent<Text>().text = rankData[i].total_score.ToString();

                selectTransformList.Add(rankTransform);
                rankTransform.gameObject.SetActive(true);
            }

        }

        // MARK: Show UI
        private async Task PrepareRankInfo(List<TypeFlag.SocketDataType.StudentRankType> studentRankData)
        {
            var weightScoreList = await Task.Run(() => GetWeightScoreAsync(studentRankData)); // MARK: Get WeightScore
            var reRank = weightScoreList.OrderByDescending(data => data.total_score).ToList();
            float height = 30f;
            float containHeight = 10f; ;
            string rank;
            Debug.Log("================== PrepareRankInfo 1");
            for (int i = 0; i < weightScoreList.Count; i++)
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
                rankTransform.Find("name").GetComponent<Text>().text = weightScoreList[i].student_name;
                rankTransform.Find("number").GetComponent<Text>().text = weightScoreList[i].student_id;
                rankTransform.Find("score").GetComponent<Text>().text = weightScoreList[i].total_score.ToString();

                selectTransformList.Add(rankTransform);
                rankTransform.gameObject.SetActive(true);
            }
        }

        private List<TypeFlag.SocketDataType.StudentRankType> GetWeightScoreAsync(List<TypeFlag.SocketDataType.StudentRankType> studentRankData)
        {
            var sameScore = studentRankData.GroupBy(s => s.total_score).Where(g => g.Count() > 1).ToList();
            var rankData = studentRankData.OrderByDescending(data => data.total_score).ToList();
            List<int> rankIndex = new List<int>();
            List<Task<float>> sameScoreList = new List<Task<float>>();
            //List<float> sameScoreList = new List<float>();

            for (int i = 0; i < rankData.Count; i++)
            {
                // get same score index
                foreach (var s in sameScore)
                {
                    if (rankData[i].total_score == s.Key) { rankIndex.Add(i); }
                    sameScoreList.Add(GetUserScore(rankData[i].student_id));  // TODO: wait return!
                }

                foreach (var r in rankIndex)
                {
                    if (i == r)
                    {
                        var totalScore = rankData[i];
                        //totalScore.total_score = sameScoreList[r]; // TODO: sign sameScore to origin rank data, ERROR?
                        rankData[i] = totalScore;
                    }
                }
                Debug.Log("================== GetWeightScore 3.5");
            }

            return rankData;
        }

        private Task<float> GetUserScore(string id)
        {
            var scoreWeight = Task.Run(() => getStudentScore(id));
            Debug.Log("================== GetUserScore 4" + scoreWeight);
            return scoreWeight;
        }

        // MARK: Get Student Score and ID
        private float getStudentScore(string id)
        {
            string uri = StringAsset.GetFullAPIUri(string.Format(StringAsset.API.GetStudentScore, id));
            float scoreWeight = 0;

            _ = StartCoroutine(
                APIHttpRequest.NativeCurl(uri, UnityWebRequest.kHttpVerbGET, null, (string json) =>
                {
                    var scoreType = JsonHelper.FromJson<TypeFlag.SocketDataType.UserScoreType>(json);

                    scoreWeight = CaculateWeightScore(scoreType);
                    Debug.Log("================== getStudentScore 5");
                }, () =>
                {

                })
            );
            return scoreWeight;
        }

        // MARK: caculate weight score
        private float CaculateWeightScore(TypeFlag.SocketDataType.UserScoreType[] scoreArray)
        {
            var scoreList = scoreArray.ToList();
            float sunScore = 0;
            float sumID = 0;

            foreach (var s in scoreList)
            {
                sumID += s.id + 1;
                sunScore += s.score;
                Debug.Log("id: " + s.id + " score: " + s.score);
                Debug.Log("sumID: " + sumID + " sunScore: " + sunScore);
            }
            Debug.Log("================== GenerateScoreBoard 5");
            return scoreList.Count / (sumID + 1 * 10000) + sunScore;// sameScoreList.Add(scoreList.Count / (sumID+1 * 10000) + sunScore);

        }

        /*
        private Task<float> GetUserScore1(string id)
        {
            string uri = StringAsset.GetFullAPIUri(string.Format(StringAsset.API.GetStudentScore, id));
            float scoreWeight = 0;

            _ = StartCoroutine(
                APIHttpRequest.NativeCurl(uri, UnityWebRequest.kHttpVerbGET, null, (string json) =>
                {
                    var scoreType = JsonHelper.FromJson<TypeFlag.SocketDataType.UserScoreType>(json);

                    scoreWeight = GenerateScoreBoard(scoreType);
                    Debug.Log("================== GetUserScore 4");
                }, () =>
                {

                })
            );

            return scoreWeight;
        }
*/

        private void GetDropdownData()
        {
            StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(StringAsset.API.GetAllClassInfo), UnityWebRequest.kHttpVerbGET, null, (string json) =>
            {
                var classArray = JsonHelper.FromJson<TypeFlag.SocketDataType.ClassroomDatabaseType>(json);

                if (classArray != null) { PerpareDropDownList(classArray); }

            }, null));
        }

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
    }
}
