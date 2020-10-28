using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using System.Runtime.InteropServices;

public class IBeaconLocator : MonoBehaviour
{
    JoeGM GM;
    Point point;
    Point p1;
    Point p2;
    Point p3;
    Vector3 targetPosition;
    // Start is called before the first frame update

    public int[] PointNumber = new int[3];
    public float[] PointPosotions = new float[6];
    //public Transform ct;
    //public InputField[] pos;



    public bool st;
    Transform _camera;
    void Start()
    {
        if (MissionsController.Instance.isARsupport)
        {
            _camera = MissionsController.Instance.ARcamera.transform;
            //gameObject.SetActive(false);
        }
        else
        {
            _camera = MissionsController.Instance.MainCamera.transform;
        }
       
        GM = JoeGM.joeGM;
        p1 = new Point();
        p2 = new Point();
        p3 = new Point();
        point = new Point();

    }

    public void UI_start()
    {
        JoeGM.beaconUPD += posUPD;
        st = true;
       
    }

    public void UI_Stop()
    {
        JoeGM.beaconUPD -= posUPD;
        st = false;
       
    }
    // Update is called once per frame
    private Vector3 velocity = Vector3.zero;
    private float Intervals = 0.3f;
    void Update()
    {

        if (st)
        {

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, Intervals);
        }

    }

    public void posUPD()
    {
       

        if (st)
        {

            p1.X = PointPosotions[0] + _camera.position.x;
            p1.Y = PointPosotions[1] + _camera.position.z;
            p2.X = PointPosotions[2] + _camera.position.x;
            p2.Y = PointPosotions[3] + _camera.position.z;
            p3.X = PointPosotions[4] + _camera.position.x;
            p3.Y = PointPosotions[5] + _camera.position.z;

            p1.Distance = GM.IBeaconDistances[PointNumber[0]];
            p2.Distance = GM.IBeaconDistances[PointNumber[1]];
            p3.Distance = GM.IBeaconDistances[PointNumber[2]];
           
            point = GetPiontByThree(p1, p2, p3);

            targetPosition = new Vector3(point.X- +_camera.position.x, transform.position.y, point.Y - _camera.position.z);


        }
    }
    

    public void setprint()
    {

    }

    //public class

    static void Main(string[] args)
    {
        Point p1 = new Point() { X = 0, Y = 2, Distance = Math.Sqrt(5) };
        Point p2 = new Point() { X = 2, Y = 2, Distance = Math.Sqrt(5) };
        Point p3 = new Point() { X = 1, Y = 0, Distance = Math.Sqrt(0) };
        var p = GetPiontByThree(p1, p2, p3);
        Console.WriteLine("Point x:{0}", p.X);
        Console.WriteLine("Point y:{0}", p.Y);
        Console.ReadKey();
    }


    private static Point GetPiontByThree(Point p1, Point p2, Point p3)
    {

        var A = p1.X - p3.X;
        var B = p1.Y - p3.Y;
        var C = Math.Pow(p1.X, 2) - Math.Pow(p3.X, 2) + Math.Pow(p1.Y, 2) - Math.Pow(p3.Y, 2) + Math.Pow(p3.Distance, 2) - Math.Pow(p1.Distance, 2);
        var D = p2.X - p3.X;
        var E = p2.Y - p3.Y;
        var F = Math.Pow(p2.X, 2) - Math.Pow(p3.X, 2) + Math.Pow(p2.Y, 2) - Math.Pow(p3.Y, 2) + Math.Pow(p3.Distance, 2) - Math.Pow(p2.Distance, 2);

        var x = (B * F - E * C) / (2 * B * D - 2 * A * E);
        var y = (A * F - D * C) / (2 * A * E - 2 * B * D);

        Point P = new Point() { X = (float)x, Y = (float)y, Distance = 0 };
        return P;
    }
    [System.Serializable]
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }

        public double Distance { get; set; }
    }
}
