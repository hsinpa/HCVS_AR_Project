using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using Hsinpa.View;

namespace Expect.View
{
    public class BagPanel : BaseView
    {
        [Header("Buttons")]
        [SerializeField]
        private Button[] eventButtons;

        [Header("Sprite")]
        [SerializeField]
        private Sprite[] eventImage;

        [Header("Sprite")]
        [SerializeField]
        private Sprite[] detailInfoImage;

        [Header("Transform")]
        public Transform Container;
        public Transform Info;

        [Header("Detail Info")]
        public GameObject detailGameObject;
        public Image detailObject;
        public Text detailTxt;
        public GameObject mailInfo;

        [Header("SiwtchPanel")]
        public Button close;
        public MainBaseVIew mainBaseVIew;

        private bool isCheck;
        private bool isPressMail;
        private bool missionF;
        private bool missionB;

        private List<int> countIndex = new List<int>();
        private float height;
        private bool isMailClick;
        private bool isMapClick;
        private List<Transform> mapTransformList = new List<Transform>();
        private List<Transform> selectTransformList = new List<Transform>();
        private string[] objectInfo = { StringAsset.BagObjectInfo.mail, StringAsset.BagObjectInfo.map1, StringAsset.BagObjectInfo.map2, StringAsset.BagObjectInfo.mapAll };
        private string[] detailInfo = { StringAsset.BagObjectInfo.mailDetail, StringAsset.BagObjectInfo.map1Detail, StringAsset.BagObjectInfo.map2Detail, StringAsset.BagObjectInfo.mapAlDetail };

        void Start()
        {
            BagMissionEvent();
        }

        private void BagMissionEvent()
        {

            for (int i = 0; i < eventButtons.Length; i++)
            {
                int closureIndex = i;
                eventButtons[closureIndex].onClick.AddListener(() => AddContentInfo(closureIndex));
            }

            SwitchPanelController();

        }

        private void SwitchPanelController()
        {
            close.onClick.AddListener(() => {
                this.Show(false);
                mainBaseVIew.PanelController(false);
            });
        }

        public void AddContentInfo(int index)
        {
            height += 90f;

            Transform infoTransform = Instantiate(Info, Container);
            RectTransform infoRectTransform = infoTransform.GetComponent<RectTransform>();

            infoRectTransform.anchoredPosition = new Vector2(0, 90 - height);

            infoTransform.Find("Text").GetComponent<Text>().text = objectInfo[index];
            infoTransform.Find("Button").GetComponent<Image>().sprite = eventImage[index];
            infoTransform.Find("Button").GetComponent<Button>().onClick.AddListener(() => AddDetailInfo(index));

            countIndex.Add(index);

            if (index == 0) { JoeGM.joeGM.hasMail = true; isPressMail = true; } // has mail show in JoeGM Usebutton
            if (index == 1 || index == 2) { isMapClick = true; }
            if (index == 0 && !isMapClick) { isMailClick = true; }
            if (index == 0 && isMapClick) { isMailClick = false; }
            if (index > 0 ) { mapTransformList.Add(infoTransform); }

            selectTransformList.Add(infoTransform);
            infoTransform.gameObject.SetActive(true);
        }

        public void AddAllMapInfo()
        {
            int index = 3;
            int currentHeight = isMailClick? 180 : 80;

            if (!isMailClick) { height = 90; }

            Transform infoTransform = Instantiate(Info, Container);
            RectTransform infoRectTransform = infoTransform.GetComponent<RectTransform>();

            infoRectTransform.anchoredPosition = new Vector2(0, 90 - currentHeight);

            infoTransform.Find("Text").GetComponent<Text>().text = objectInfo[index];
            infoTransform.Find("Button").GetComponent<Image>().sprite = eventImage[index];
            infoTransform.Find("Button").GetComponent<Button>().onClick.AddListener(() => AddDetailInfo(index));

            infoTransform.gameObject.SetActive(true);
            
            countIndex.Remove(1);
            countIndex.Remove(2);
            countIndex.Add(3);

            selectTransformList.Clear();

            if (countIndex.Count > 1)
            {
                selectTransformList.Add(infoTransform);
                selectTransformList.Add(infoTransform);
            }
            else
            {
                selectTransformList.Add(infoTransform);
            }
        }

        public void UseMail()
        {
            RemoveShowData();
            countIndex.Remove(0);

            if (countIndex.Count == 0) return;
            Debug.Log("countIndex " + countIndex.Count);

            height = 90f;

            Transform infoTransform = Instantiate(Info, Container);
            RectTransform infoRectTransform = infoTransform.GetComponent<RectTransform>();

            for (int index = 0; index < countIndex.Count; index++)
            {                
                infoRectTransform.anchoredPosition = new Vector2(0, -height * index);
            }

            foreach (var index in countIndex)
            {
                infoTransform.Find("Text").GetComponent<Text>().text = objectInfo[index];
                infoTransform.Find("Button").GetComponent<Image>().sprite = eventImage[index];
                infoTransform.Find("Button").GetComponent<Button>().onClick.AddListener(() => AddDetailInfo(index));
            }

            infoTransform.gameObject.SetActive(true);
        }

        private void AddDetailInfo(int index)
        {
            if (index == 0)
            {
                mailInfo.SetActive(true);
            }
            else
            {
                detailObject.sprite = detailInfoImage[index];
                detailTxt.text = detailInfo[index];
                detailGameObject.SetActive(true);
            }
        }

        public void CheckGetMail()
        {
            if (isPressMail) return;

            List<TypeFlag.SocketDataType.StudentType> studentData = MainView.Instance.studentData;

            for (int i = 0; i < studentData.Count; i++)
            {
                if (studentData[i].mission_id == "F") { missionF = true; Debug.Log("missionF: " + missionF); }
                if (studentData[i].mission_id == "B") { missionB = true; Debug.Log("missionB: " + missionB); }
                if (missionF && !missionB && !isCheck) { AddContentInfo(0); isCheck = true; missionB = true; Debug.Log("isCheck: " + isCheck); }
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

        public void RemoveMapChip()
        {
            foreach (var t in mapTransformList) { Destroy(t.gameObject); }
            foreach (var t in mapTransformList) { Destroy(t.GetChild(0).gameObject); }
            mapTransformList.Clear();
        }
    }
}
