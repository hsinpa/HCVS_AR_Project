using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FingerClickEvent : MonoBehaviour
{
    public delegate void OnFingerClick();
    public event OnFingerClick Click;

    private Touch theTouch;
    private float timeTouchEnded;
    public Text phaseDisplayText;

    [HideInInspector]
    public BoxCollider2D boxCollider;
    [HideInInspector]
    public int count;

    private void Awake()
    {
        boxCollider = this.GetComponent<BoxCollider2D>();
    }

    /* TODO: Switch to touch, AR not yet
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            phaseDisplayText.text = theTouch.phase.ToString();

            Debug.Log("Yes");
        }
        else
        {
            phaseDisplayText.text = theTouch.phase.ToString();
            Debug.Log("NO");
        }
    }
    */
    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            Click();
        }
    }
}
