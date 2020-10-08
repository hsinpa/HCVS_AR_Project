using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{

    private LocationService locationServer;
    private LocationServiceStatus locationServerStatus;
    private LocationInfo locationInfo;

    private bool isCould;  //是否開啟定位服務，即開啟GPS定位
    private float altitude; //海拔高度
    private float horizontalAccuracy; //水平精度
    private float verticalAccuracy;  //垂直精度
    private float latitude;       //緯度
    private float longitude;      //經度
    private double timestamp;     //最近一次定位的時間戳，從 1970年開始
    public Transform obj;
    public Transform obj1;
    Vector2 NE = new Vector2(22.615582f, 120.282428f);
    // Use this for initialization  
    void Start()
    {
        locationServer = Input.location;
        isCould = locationServer.isEnabledByUser; //使用者是否可以設定定位服務      
        locationServerStatus = locationServer.status; //返回裝置服務狀態
        //引數1 服務所需的精度，以米為單位，引數2 最小更新距離
        locationServer.Start(1, 1);//開始位置更新服務，最後的位置座標
        //locationServer.Stop();//停止位置服務更新，節省電池壽命
    }

    void Update()
    {
        //呼叫該方法之前確保呼叫了 Input.location.Start()
        locationInfo = locationServer.lastData; //裝置最後檢測的位置
        altitude = locationInfo.altitude;//裝置高度
        horizontalAccuracy = locationInfo.horizontalAccuracy; //水平精確度
        verticalAccuracy = locationInfo.verticalAccuracy; //垂直精確度
        latitude = locationInfo.latitude; //裝置緯度
        longitude = locationInfo.longitude;//裝置緯度
        obj.transform.position = new Vector3((NE.x - latitude) * 111000, obj.position.y, (NE.y - longitude) * 111000);

        timestamp = locationInfo.timestamp;//時間戳(自1970年以來以秒為單位)位置時最後一次更新。
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 40;

        GUI.Label(new Rect(50, 0, 500, 80), "isCould : " + isCould);
        GUI.Label(new Rect(50, 100, 800, 80), "locationInfo : " + locationInfo);
        GUI.Label(new Rect(50, 200, 500, 80), "海拔     :   " + altitude);
        GUI.Label(new Rect(50, 300, 500, 80), "水平精度 :   " + horizontalAccuracy);
        GUI.Label(new Rect(50, 400, 500, 80), "垂直經度 :   " + verticalAccuracy);
        GUI.Label(new Rect(50, 500, 500, 80), "緯度     :   " + latitude);
        GUI.Label(new Rect(50, 600, 500, 80), "經度     :   " + longitude);
        GUI.Label(new Rect(50, 700, 500, 80), "時間戳   :   " + timestamp);
    }
}
