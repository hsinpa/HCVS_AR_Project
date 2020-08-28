using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;

public class BagPanel : MonoBehaviour
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
    [SerializeField]
    public Transform Container;
    public Transform Object;
    public Transform Info;

    [Header("Detail Info")]
    [SerializeField]
    public GameObject detailGameObject;
    public Image detailObject;
    public Text detailTxt;

    private float height;
    private string[] objectInfo = { StringAsset.BagObjectInfo.mail, StringAsset.BagObjectInfo.map1, StringAsset.BagObjectInfo.map2 };
    private string[] detailInfo = { StringAsset.BagObjectInfo.mailDetail, StringAsset.BagObjectInfo.map1Detail, StringAsset.BagObjectInfo.map2Detail };

    void Start()
    {
        BagMissionEvent();
    }

    private void BagMissionEvent()
    {

        for (int i = 0; i < eventButtons.Length; i++)
        {
            int closureIndex = i;
            eventButtons[closureIndex].onClick.AddListener(() => AddListener(closureIndex));
        }
    }

    private void AddListener(int index)
    {
        height += 170f;

        Transform objectTransform = Instantiate(Object, Container);
        Transform infoTransform = Instantiate(Info, Container);
        RectTransform objectRectTransform = objectTransform.GetComponent<RectTransform>();
        RectTransform infoRectTransform = infoTransform.GetComponent<RectTransform>();

        infoRectTransform.anchoredPosition = new Vector2(0, -height);
        objectRectTransform.anchoredPosition = new Vector2(0, -height);
        
        objectTransform.Find("Image").GetComponent<Image>().sprite = eventImage[index];
        infoTransform.Find("Text").GetComponent<Text>().text = objectInfo[index];

        objectTransform.Find("Button").GetComponent<Button>().onClick.AddListener(() => AddInfoListener(index));
    }

    private void AddInfoListener(int index)
    {
        Debug.Log("AddInfoListener" + index);
        detailObject.sprite = detailInfoImage[index];
        detailTxt.text = detailInfo[index];
        detailGameObject.SetActive(true);
    }
}
