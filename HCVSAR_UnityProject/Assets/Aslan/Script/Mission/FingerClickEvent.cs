using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerClickEvent : MonoBehaviour
{
    public delegate void OnFingerClick();
    public event OnFingerClick Click;

    [HideInInspector]
    public BoxCollider2D boxCollider;
    [HideInInspector]
    public int count;

    private void Awake()
    {
        boxCollider = this.GetComponent<BoxCollider2D>();
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            Click();
        }
    }
}
