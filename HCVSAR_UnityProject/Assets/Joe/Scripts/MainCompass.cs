using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCompass : MonoBehaviour
{
    public static MainCompass main;
    //public Transform camera;
    private ARLocation.ARLocationProvider locationProvider;
    float dushu = 0;
    float tempdushu = 0;
    private Camera _camera;
    // Start is called before the first frame update
    public void Awake()
    {
        main = this;
    }

    void Start()
    {
        //locationProvider = ARLocation.ARLocationProvider.Instance;
        //Input.location.Start();
        //Input.compass.enabled = true;
        _camera = MissionsController.Instance.isARsupport ? MissionsController.Instance.ARcamera : MissionsController.Instance.MainCamera;

        if (MissionsController.Instance.isARsupport) {
            //gameObject.SetActive(false);
        }
        else
        {
            //Input.location.Start();
            //Input.compass.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = camera.position+new Vector3(0,-2,0);
        //var currentHeading = locationProvider.CurrentHeading.heading;
        //var currentMagneticHeading = locationProvider.CurrentHeading.magneticHeading;
        //var currentAccuracy = locationProvider.Provider.CurrentHeading.accuracy;


        //transform.rotation = Quaternion.Euler(0, 0, (float)currentMagneticHeading);
        //transform.rotation = Quaternion.Euler(0, (float)currentHeading, 0);
        //Input.location.Start();

        //dushu = Input.compass.trueHeading;
        transform.eulerAngles = new Vector3(0, (_camera.transform.rotation.eulerAngles.y - (float)ARLocation.ARLocationProvider.Instance.CurrentHeading.heading), 0);
       
        //transform.eulerAngles = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);

    }
}
