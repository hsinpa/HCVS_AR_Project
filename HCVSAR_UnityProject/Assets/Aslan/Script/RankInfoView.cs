using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using UnityEngine.Networking;
using System.Linq;
using Hsinpa.View;

namespace Expect.View
{
    public class RankInfoView : BaseView
    {
        [SerializeField]
        private Dropdown ClassDeptSelection;

        [SerializeField]
        private Dropdown ClassNameSelection;

        [Header("RankInfo")]
        [SerializeField]
        public Transform rankContainer;
        public Transform rankInfo;

        [Header("SiwtchPanel")]
        public Button close;
        public MainBaseVIew mainBaseVIew;

        private List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet;
        private List<TypeFlag.SocketDataType.StudentRankType> studentRankData;
        private List<string> FilterClassDeptList = new List<string>();
        private List<string> FilterClassNameList = new List<string>();
        private List<Transform> selectTransformList = new List<Transform>();

        private string class_id;
        private string student_id;

        private void GetData()
        {
            class_id = MainView.Instance.loginData.room_id;
            student_id = MainView.Instance.loginData.user_id;
        }

        public void RankInfoStart()
        {
            GetData();

            PrepareRankData(class_id);
            GetClassRoomData1();
            SwitchPanelController();
        }

        private void SwitchPanelController()
        {
            close.onClick.AddListener(() => {
                this.Show(false);
                mainBaseVIew.ClosePanel();
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

                        var rankData = studentRankData.OrderByDescending(data => data.total_score).ToList();
                        RankInfo(rankData);
                    }
                }, null));
        }

        private void RankInfo(List<TypeFlag.SocketDataType.StudentRankType> studentRankData)
        {
            float height = 30f;
            float containHeight = -50f; ;
            string rank;

            for (int i = 0; i < studentRankData.Count; i++)
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
                rankTransform.Find("name").GetComponent<Text>().text = studentRankData[i].student_name;
                rankTransform.Find("number").GetComponent<Text>().text = studentRankData[i].student_id;
                rankTransform.Find("score").GetComponent<Text>().text = studentRankData[i].total_score.ToString();

                selectTransformList.Add(rankTransform);
                rankTransform.gameObject.SetActive(true);
            }
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


        private void GetClassRoomData1()
        {
            StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(StringAsset.API.GetAllClassInfo), UnityWebRequest.kHttpVerbGET, null, (string json) =>
            {
                var classArray = JsonHelper.FromJson<TypeFlag.SocketDataType.ClassroomDatabaseType>(json);

                if (classArray != null)
                {
                    List<string> studentClass = new List<string>();
                    string deptName = "";
                    string className = "";

                    classroomDataSet = classArray.ToList();

                    FilterClassDeptList = classroomDataSet.GroupBy(c => c.class_name).Select(grp => grp.First().class_name.Substring(0, 3).ToString()).Distinct().ToList();
                    FilterClassNameList = classroomDataSet.GroupBy(c => c.class_name).Select(grp => grp.First().class_name.Substring(3, 1).ToString()).Distinct().ToList();

                    var FilterClassDept = classroomDataSet.GroupBy(c => c.class_name.Substring(0, 3)).Distinct();
                    var FilterStudentDept = classroomDataSet.Where(c => c.class_id == class_id).ToList();

                    foreach (var dept in FilterStudentDept)
                    {
                        string studentFullClassName = dept.class_name;
                        deptName = studentFullClassName.Substring(0, 3);
                        className = studentFullClassName.Substring(3, 1);
                        Debug.Log("dept.class_name" + className);
                    }

                    foreach (var dept in FilterClassDept)
                    {
                        if (dept.Key.ToString() == deptName)
                        {
                            FilterClassNameList = dept.Select(grp => grp.class_name.Substring(3, 1)).ToList();
                        //deptNameList = dept.Select(grp => grp.class_name).ToList();
                            ClassNameSelection.ClearOptions();
                            ClassNameSelection.AddOptions(FilterClassNameList);

                            Debug.Log("****** dept" + dept.Count());
                        }
                    }

                    ClassDeptSelection.ClearOptions();
                    ClassDeptSelection.AddOptions(FilterClassDeptList);
                    ClassDeptSelection.value = FilterClassDeptList.IndexOf(deptName);
                /*
                ClassNameSelection.ClearOptions();
                ClassNameSelection.AddOptions(FilterClassNameList);
                ClassNameSelection.value = FilterClassDeptList.IndexOf(className);*/
                }

            }, null));
        }

        private void DecorateSelectView(List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet)
        {
            if (classroomDataSet == null) return;

            string ClassDeptSelect;


            List<string> deptNameList = new List<string>();
            var FilterClassDept = classroomDataSet.GroupBy(c => c.class_name.Substring(0, 3)).Distinct();

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

        }
    }
}



    /*
    void DecorateClassName()
    {
        // Get All Class Room Data
        StartCoroutine(
            APIHttpRequest.NativeCurl(StringAsset.GetFullAPIUri(StringAsset.API.GetAllClassInfo), UnityWebRequest.kHttpVerbGET, null, (string json) => {
                if (string.IsNullOrEmpty(json))
                {
                    return;
                }

                var classArray = JsonHelper.FromJson<TypeFlag.SocketDataType.ClassroomDatabaseType>(json);

                if (classArray != null)
                {
                    Debug.Log("studentFullClassName111");
                    classroomDataSet = classArray.ToList();
                    FilterClassDeptList = classroomDataSet.GroupBy(test => test.year).Select(grp => grp.First().year.ToString()).ToList();
                    FilterClassNameList = classroomDataSet.GroupBy(c => c.class_name).Select(grp => grp.First().class_name.Substring(3, 2).ToString()).Distinct().ToList();

                    List<string> studentDept = new List<string> { "vghv", "ytfiuhj"};
                    
                    var FilterStudentDept = classroomDataSet.Where(c => c.class_id == class_id).ToList();
                    
                    List<string> studentClass = new List<string>();
                    string studentFullClassName = "";
                    Debug.Log("studentFullClassName111");
                    foreach (var dept in FilterStudentDept)
                    {
                        studentFullClassName = dept.class_name;
                        Debug.Log("dept.class_name" + dept.class_name);
                        studentDept.Add(studentFullClassName.Substring(0, 3));
                        studentClass.Add(studentFullClassName.Substring(3, 2));
                    }
                    
                    ClassDeptSelection.ClearOptions();
                    ClassDeptSelection.AddOptions(FilterClassDeptList);

                    ClassNameSelection.ClearOptions();
                    ClassNameSelection.AddOptions(FilterClassDeptList);
                }

            }, null));

    }
*/
/*
    
    //DecorateSelectView(classroomDataSet);
    // DropDown
    var StudentDept = classroomDataSet.Where(c => c.class_id == class_id).ToList();

    FilterClassDeptList = classroomDataSet.GroupBy(c => c.class_name).Select(grp => grp.First().class_name.Substring(0, 3).ToString()).Distinct().ToList();

    foreach (var d in StudentDept)
    {
        Debug.Log("class_id: " + d.class_id + " class_name" + d.class_name);
        var selectClassDept = classroomDataSet.IndexOf(d);
        Debug.Log("selectClassDept " + selectClassDept);
        ClassDeptSelection.ClearOptions();
        ClassDeptSelection.options[1].text = d.class_name;
    }
    */
    //ClassDeptSelection.ClearOptions();
    //ClassDeptSelection.AddOptions(FilterClassDeptList);

    //foreach (var s in selectClassDept) { Debug.Log("s class_id: " + s.class_id + "s class_name" + s.class_name); }


    
    /*
    private void DecorateSelectView(List<TypeFlag.SocketDataType.ClassroomDatabaseType> classroomDataSet)
    {
        if (classroomDataSet == null) return;

        string ClassDeptSelect;

        List<string> deptNameList = new List<string>();
        var FilterClassDept = classroomDataSet.GroupBy(c => c.class_name.Substring(0, 3)).Distinct();

        FilterClassDeptList = classroomDataSet.GroupBy(c => c.class_name).Select(grp => grp.First().class_name.Substring(0, 3).ToString()).Distinct().ToList();
        //FilterClassNameList = classroomDataSet.GroupBy(c => c.class_name).Select(grp => grp.First().class_name.Substring(3, 1).ToString()).Distinct().ToList();

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

                    ClassNameSelection.ClearOptions();
                    ClassNameSelection.AddOptions(FilterClassNameList);

                    Debug.Log("****** dept" + dept.Count());
                }
            }

            var selectClassDept = from c in classroomDataSet where c.class_name == ClassDeptSelect select c;

            foreach (var s in selectClassDept)
            {
                class_id = s.class_id;
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

            // change 
            PrepareRankData(class_id);

        });
    }
    */

