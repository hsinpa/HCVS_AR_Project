using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Game07 : MonoBehaviour
{
    public UnityEvent StartEvent;
    public UnityEvent unityEvent;
    public UnityEvent OverEvent;
    
    public Transform movieCubeTransform;
    //public Transform t2;
    //public Transform t3;
    //public Image image;
    //int v = 1;
    public Animator ani;
    public float speed = -2;

    //public GameObject sg;
    //public GameObject eg;

    //public PlayableDirector director;
    public GameObject gameMan;
    public GameObject speakMan;
    public GameObject arGameObject;
    public GameObject targetStone;
    private bool isARStoneFollowCamera;

    void Start()
    {
        //t.gameObject.SetActive(true);
        //t2.gameObject.SetActive(true);
        //gameMan.SetActive(true);
        speakMan.SetActive(false);
        targetStone.SetActive(false);
    }

    // Update is called once per frame
    public void UI_Start()
    {
        StartEvent.Invoke();
        //t.transform.parent = null;
        isARStoneFollowCamera = true;
        targetStone.SetActive(true);
    }
    public void UI_Enter()
    {
        //t3.position = t2.position;
        //ani.speed = 0;
        if (Mathf.Abs(movieCubeTransform.localPosition.x)<0.5)
        {
            Invoke("right", 2);
            ani.speed = 0;
            ani.SetBool("OK", false);
            gameMan.transform.localPosition = new Vector3(-0.45f, -0.56f, 0f); //new Vector3(-0.3568177f, -0.46f, 0f);

        }
        else
        {
            OverEvent.Invoke();
            ani.speed = 0;
            //ani.enabled = false;
            //t3.GetComponent<test07>().enabled = false;
            gameMan.GetComponent<Rigidbody>().isKinematic = false;
        }
      
    }

    void right()
    {
        unityEvent.Invoke();
    }

    public void StarSuccessAnimation()
    {
        gameMan.SetActive(false);

        //ani.SetBool("OK", true);
        speakMan.SetActive(true);
        //director.Play();
    }

    void Update()
    {
        if (!isARStoneFollowCamera)
        {
            var _camera = Camera.main;
            var _cameraFront = _camera.transform.forward;
            var _frontPos = _cameraFront * 5;

            _cameraFront.y = 0;
            arGameObject.transform.position = _frontPos;
            arGameObject.transform.rotation = Quaternion.LookRotation(_cameraFront);
        }
    }
}
