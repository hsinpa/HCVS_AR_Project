using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using UnityEngine.Networking;
using System.Linq;

public class RankInfoView : MonoBehaviour
{
        
    [SerializeField]
    private Dropdown ClassDeptSelection;

    [SerializeField]
    private Dropdown ClassNameSelection;

    /*
    [Header("Button")]
    [SerializeField]
    private Button ConfirnBtn;
    */

    [Header("RankInfo")]
    [SerializeField]
    public Transform rankContainer;
    public Transform rankInfo;

    [System.Serializable]
    public struct StudentRankType
    {
        public int total_score;
        public string student_id;
        public string student_name;
    }

    private List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet;
    private List<StudentRankType> studentRankData;
    private List<string> FilterClassDeptList = new List<string>();
    private List<string> FilterClassNameList = new List<string>();

    private new string class_id;

    void Start()
    {
        GetClassRoomData();
        //PrepareScoreData(loginData.user_id);
    }

    private void GetClassRoomData()
    {
        StartCoroutine(
        APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(StringAsset.API.GetAllClassInfo), UnityWebRequest.kHttpVerbGET, null, (string json) =>
        {
            var classArray = JsonHelper.FromJson<TypeFlag.SocketDataType.ClassroomDatabaseType>(json);

            if (classArray != null)
            {
                classroomDataSet = classArray.ToList();
                DecorateSelectView(classroomDataSet);
                Debug.Log("classroomDataSet: " + classroomDataSet);
            }

        }, null));

        //ConfirnBtn.onClick.RemoveAllListeners();
    }

    private void DecorateSelectView(List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet)
    {
        if (classroomDataSet == null) return;

        string ClassDeptSelect;
        

        List<string> deptNameList = new List<string>();
        var FilterClassDept = classroomDataSet.GroupBy(c => c.class_name.Substring(0, 3)).Distinct();

        FilterClassDeptList = classroomDataSet.GroupBy(c => c.class_name).Select(grp => grp.First().class_name.Substring(0, 3).ToString()).Distinct().ToList();
        FilterClassNameList = classroomDataSet.GroupBy(c => c.class_name).Select(grp => grp.First().class_name.Substring(3, 1).ToString()).Distinct().ToList();

        ClassDeptSelection.ClearOptions();
        ClassDeptSelection.AddOptions(FilterClassDeptList);

        ClassNameSelection.ClearOptions();
        ClassNameSelection.AddOptions(FilterClassNameList);

        ClassDeptSelection.onValueChanged.AddListener((UnityEngine.Events.UnityAction<int>)delegate
        {
            ClassDeptSelect = ClassDeptSelection.options[ClassDeptSelection.value].text;

            if (ClassDeptSelect.Count() <= 0) return;

            foreach (var dept in FilterClassDept)
            {
                if (dept.Key.ToString() == ClassDeptSelect)
                {
                    FilterClassNameList = dept.Select(grp => grp.class_name.Substring(3, 1)).ToList();
                    deptNameList = dept.Select(grp => grp.class_name).ToList();

                    //if (dept.Count() <= 1)
                    //{   
                        
                    //}

                    ClassNameSelection.ClearOptions();
                    ClassNameSelection.AddOptions(FilterClassNameList);

                    Debug.Log("****** dept" + dept.Count());
                }
            }

            var selectClassDept = from c in classroomDataSet where c.class_name == ClassDeptSelect select c;

            foreach (var s in selectClassDept)
            {
                class_id = s.class_id;
                //Debug.Log("=====v: " + s.class_id);
                Debug.Log("=====select: " + s.class_id);
            }

            if (class_id == null) return;

            PrepareRankData(class_id);
        });


        ClassNameSelection.onValueChanged.AddListener((UnityEngine.Events.UnityAction<int>)delegate
        {
            string selectClass = deptNameList[ClassNameSelection.value];
            
            var selectClassName = from c in classroomDataSet where c.class_name == selectClass select c;
            //if (ClassNamelect.Count() <= 0) return;

            foreach (var s in selectClassName) {
                class_id = s.class_id;
                Debug.Log("v: " + s.class_id);
            }

            if (class_id == null) return;

            PrepareRankData(class_id);

        });
    }

    private void PrepareRankData(string class_id)
    {
        string getRankURI = string.Format(StringAsset.API.GetStudentRank, 57838582);//class_id);
        Debug.Log(getRankURI);
        Debug.Log("getRankURI " + getRankURI);

        StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(getRankURI), UnityWebRequest.kHttpVerbGET, null, (string json) => {
                if (string.IsNullOrEmpty(json))
                {
                    return;
                }

                var tempStudentRankData = JsonHelper.FromJson<StudentRankType>(json);

                if (tempStudentRankData != null)
                {
                    studentRankData = tempStudentRankData.ToList();
                    var rankData = studentRankData.OrderByDescending(data => data.total_score).ToList();
                    RankInfo(rankData);
                    Debug.Log("getRankData");
                }
            }, null));
    }

    private void RankInfo(List<StudentRankType> studentRankData)
    {
        float height = 100f;
        float containHeight = 0f; ;
        string rank;

        for (int i = 0; i < studentRankData.Count; i++)
        {
            if (i < 10)
                rank = "0" + (i + 1).ToString();
            else
                rank = (i+1).ToString();

            Transform rankTransform = Instantiate(rankInfo, rankContainer);
            RectTransform rankRectTransform = rankTransform.GetComponent<RectTransform>();
            RectTransform containRect = rankContainer.GetComponent<RectTransform>();

            containHeight = containHeight + height;
            containRect.sizeDelta = new Vector2(0, containHeight+height);
            rankRectTransform.anchoredPosition = new Vector2(0, -height * (i+1));
            rankTransform.Find("rank").GetComponent<Text>().text = rank;
            rankTransform.Find("name").GetComponent<Text>().text = studentRankData[i].student_name;
            rankTransform.Find("number").GetComponent<Text>().text = studentRankData[i].student_id;
            rankTransform.Find("score").GetComponent<Text>().text = studentRankData[i].total_score.ToString();
            rankTransform.gameObject.SetActive(true);
        }

        
    }
}
