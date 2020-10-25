using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLocator : MonoBehaviour
{
    public Vector2 BeaconNumber;
    //public Transform point1;
    public Transform point2;
    JoeGM gM;
    float p1;
    float p2;
    public UnityEngine.UI.Text text;
    // Start is called before the first frame update
    void Start()
    {
        gM = JoeGM.joeGM;
        transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        //p1 = (float)gM.IBeaconDistances[(int)BeaconNumber.x];
        //p2 = (float)gM.IBeaconDistances[(int)BeaconNumber.y];
        //float v =p1/(p1 + p2);
        //transform.position += (transform.position - point2.position) * v;
        transform.rotation = MainCompass.main.transform.rotation;
        //text.text = Camera.main.transform.position.ToString() + "pos" + transform.position.ToString() + "v" + v.ToString();
        //SewitchModel(isARsupport);
    }
    private void Update()
    {

       
        if (MissionsController.Instance.isARsupport)
        {
            if (JoeMain.Main.isIOS)
            {
                transform.rotation = MainCompass.main.transform.rotation;
            }
        }
        else
        {
            transform.rotation = MainCompass.main.transform.rotation;
        }

    }
}
