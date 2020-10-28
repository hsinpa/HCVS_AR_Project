using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBaseVIew : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup canvasGroup;

    public virtual void PanelController(bool isOpen)
    {
        this.GetComponent<Image>().enabled = isOpen;
        canvasGroup.blocksRaycasts = !isOpen;
        canvasGroup.interactable = !isOpen;
    }
}
