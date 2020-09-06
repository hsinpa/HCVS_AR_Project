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
        height += 80f;

        Transform infoTransform = Instantiate(Info, Container);
        RectTransform infoRectTransform = infoTransform.GetComponent<RectTransform>();

        infoRectTransform.anchoredPosition = new Vector2(0, 90 - height);

        infoTransform.Find("Text").GetComponent<Text>().text = objectInfo[index];
        infoTransform.Find("Button").GetComponent<Image>().sprite = eventImage[index];
        infoTransform.Find("Button").GetComponent<Button>().onClick.AddListener(() => AddInfoListener(index));

        infoTransform.gameObject.SetActive(true);
    }

    private void AddInfoListener(int index)
    {
        Debug.Log("AddInfoListener" + index);
        detailObject.sprite = detailInfoImage[index];
        //detailObject.SetNativeSize();
        detailTxt.text = detailInfo[index];
        detailGameObject.SetActive(true);
    }
}
