using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBaseVIew : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup canvasGroup;


    public virtual void ShowOtherPanel()
    {
        this.GetComponent<Image>().enabled = true;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public virtual void ShowMainPanel()
    {
        this.GetComponent<Image>().enabled = false;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
}
