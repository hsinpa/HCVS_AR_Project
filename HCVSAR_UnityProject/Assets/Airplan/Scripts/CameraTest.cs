using UnityEngine;
using System.Collections;

public class CameraTest : MonoBehaviour
{
    float[] r = new float[] { 0, 90, 180, 270 };
    int ir = 0;

    
    void Start()
    {
        transform.localRotation = Quaternion.Euler(90, -90, 90);
        /*
#if UNITY_ANDROID
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
            
        }
#endif
*/
        WebCamTexture c = new WebCamTexture();
        gameObject.GetComponent<Renderer>().material.mainTexture = c; // 將目前物體貼圖換成攝影機貼圖
        c.Play();
    }
    public void xr()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void zr()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
    }

    public void rr()
    {
        transform.localEulerAngles =new Vector3(transform.localEulerAngles.x, r[ir], transform.localEulerAngles.z);
        ir++;
        if (ir == 4)
        {
            ir = 0;
        }
    }
}