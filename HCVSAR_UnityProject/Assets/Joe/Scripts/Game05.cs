using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Game05 : MonoBehaviour
{
    
    public UnityEvent unityEvent;
    public UnityEvent ReEvent;
    public UnityEvent OverEvent;
    
    public Texture texture;
    public Image image;
    int v = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UI_Enter()
    {
        if (Physics.Linecast(Camera.main.transform.position, Camera.main.transform.position + (Camera.main.transform.forward*300)))
        {
            Debug.Log("blocked");
            unityEvent.Invoke();
        }
        else if(v == 1 )
        {
            ReEvent.Invoke();
            v--;
        }
        else
        {
            OverEvent.Invoke();
}
        RenderTexture rt = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 0);
        Camera.main.targetTexture = rt;
        //texture = Camera.main.activeTexture;
        image.material.SetTexture("_MainTex", rt);
        Camera.main.targetTexture = null;
    }
    void Update()
    {
      
    }
}
