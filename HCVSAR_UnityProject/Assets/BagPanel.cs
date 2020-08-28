using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    private Button[] eventButtons;

    [Header("Sprite")]
    [SerializeField]
    private Sprite[] eventImage;

    [SerializeField]
    public Transform Container;
    public Transform Info;
    public Transform Box;

    float height;

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
        height += 250f;

        Transform infoTransform = Instantiate(Info, Container);
        Transform boxTransform = Instantiate(Box, Container);
        RectTransform boxRectTransform = boxTransform.GetComponent<RectTransform>();
        RectTransform infoRectTransform = infoTransform.GetComponent<RectTransform>();

        boxRectTransform.anchoredPosition = new Vector2(0, 200f - height);
        infoRectTransform.anchoredPosition = new Vector2(0, 200f - height);
        infoTransform.Find("Image").GetComponent<Image>().sprite = eventImage[index];
        infoTransform.Find("Text").GetComponent<Text>().text = "不知道是誰遺落的信件，要送去學校門口的樣子" + index;
    }
}
