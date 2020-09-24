using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBaseVIew : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup canvasGroup;


    public virtual void ShowPanel()
    {
        this.GetComponent<Image>().enabled = true;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public virtual void ClosePanel()
    {
        this.GetComponent<Image>().enabled = false;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
}
