using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class Game05 : MonoBehaviour
{
    public UnityEvent unityEvent;
    public UnityEvent ReEvent;
    public UnityEvent OverEvent;
    
    //public Texture texture;
    public GameObject image;
    int v = 1;
    public GameObject trackPeople;
    

    private void Start()
    {
        trackPeople.SetActive(true);
        image.SetActive(false);
    }

    public void UI_Enter()
    {

        if (Physics.Linecast(Camera.main.transform.position, Camera.main.transform.position + (Camera.main.transform.forward*300)))
        {
            Debug.Log("blocked");
            unityEvent.Invoke();
        }
        else
        {
            StartCoroutine(RenderScreenShot(v));
            
        }
        
    }

    private IEnumerator RenderScreenShot(int times)
    {
        bool isPlayOnce = times == 1 ? true : false;

        yield return new WaitForSeconds(0.1f);

        Camera.main.targetTexture = new RenderTexture(222, 128, 0);//Camera.main.pixelWidth, Camera.main.pixelHeight, 0);

        RenderTexture renderTexture = Camera.main.targetTexture;
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Camera.main.Render();
        RenderTexture.active = renderTexture;
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

        renderResult.ReadPixels(rect, 0, 0);
        renderResult.Apply();

        Sprite screenShot = Sprite.Create(renderResult, rect, Vector2.zero);
        image.GetComponent<Image>().sprite = screenShot;
        image.SetActive(true);

        Camera.main.targetTexture = null;

        if (isPlayOnce)
        {
            ReEvent.Invoke();
            v--;
        }
        else
        {
            OverEvent.Invoke();

        }
    }
    
}
