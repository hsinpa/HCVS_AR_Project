using UnityEngine;
using UnityEngine.UI;

public class SpeedMove : MonoBehaviour
{
    // Move object using accelerometer
    float speed = 100.0f;
    private AccelerometerUtil accelerometerUtil;
    Vector3 currentInput = new Vector3();
    Vector3 dir;
    public Text t;
    // Use this for initialization
    void Start()
    {
        accelerometerUtil = new AccelerometerUtil();
    }

   
    
    void Update()
    {
        dir = Vector3.zero;
        currentInput = Input.gyro.userAcceleration;//accelerometerUtil.LowPassFiltered();

        // we assume that device is held parallel to the ground
        // and Home button is in the right hand
        if (currentInput.z> 0|| currentInput.y > 0|| currentInput.x > 0)
        {
            //t.text = currentInput*1000 +"/n" +t.text;
        }
        // remap device acceleration axis to game coordinates:
        //  1) XY plane of the device is mapped onto XZ plane
        //  2) rotated 90 degrees around Y axis
        //dir.x = -currentInput.y*10 ;
        //dir.z = currentInput.x*10;

        // clamp acceleration vector to unit sphere
        //if (dir.sqrMagnitude > 1)
        //  dir.Normalize();

        // Make it move 10 meters per second instead of 10 meters per frame...
        //currentInput *= Time.deltaTime;

        // Move object
        dir += currentInput*100*Time.deltaTime;
        transform.position = transform.position + dir;
        //transform.Translate(currentInput * speed);
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(500, 300, 200, 40), "Gyro rotation rate " + currentInput*100);
        GUI.Label(new Rect(500, 100, 200, 40), "Acc " + Input.gyro.userAcceleration);
        GUI.Label(new Rect(500, 400, 200, 40), "X " + currentInput.x);
        GUI.Label(new Rect(500, 500, 200, 40), "Y " + currentInput.y);
        GUI.Label(new Rect(500, 600, 200, 40), "Y " + currentInput.z);
    }
}