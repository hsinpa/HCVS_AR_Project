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

        private float height;
        private bool isMailClick;
        private List<Transform> mapTransformList = new List<Transform>();
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

        private void AddContentInfo(int index)
        {
            height += 80f;

            Transform infoTransform = Instantiate(Info, Container);
            RectTransform infoRectTransform = infoTransform.GetComponent<RectTransform>();

            infoRectTransform.anchoredPosition = new Vector2(0, 90 - height);

            infoTransform.Find("Text").GetComponent<Text>().text = objectInfo[index];
            infoTransform.Find("Button").GetComponent<Image>().sprite = eventImage[index];
            infoTransform.Find("Button").GetComponent<Button>().onClick.AddListener(() => AddDetailInfo(index));

            if (index == 0) { isMailClick = true; }
            if (index > 0 ) { mapTransformList.Add(infoTransform); }
            infoTransform.gameObject.SetActive(true);
        }

        public void RemoveMapChip()
        {
            foreach (var t in mapTransformList) { Destroy(t.gameObject); }
            foreach (var t in mapTransformList) { Destroy(t.GetChild(0).gameObject); }            
        }

        public void AddAllMapInfo()
        {
            int index = 3;
            int currentHeight = isMailClick? 160 : 80;

            Transform infoTransform = Instantiate(Info, Container);
            RectTransform infoRectTransform = infoTransform.GetComponent<RectTransform>();

            infoRectTransform.anchoredPosition = new Vector2(0, 90 - currentHeight);

            infoTransform.Find("Text").GetComponent<Text>().text = objectInfo[index];
            infoTransform.Find("Button").GetComponent<Image>().sprite = eventImage[index];
            infoTransform.Find("Button").GetComponent<Button>().onClick.AddListener(() => AddDetailInfo(index));
            
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
    }
}
