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
    public Animator ani;
    public float speed = -2;
    
    public GameObject gameMan;
    public GameObject speakMan;
    public GameObject arGameObject;
    public GameObject targetStone;
    private bool isARStoneFollowCamera;

    void Start()
    {
        speakMan.SetActive(false);
        targetStone.SetActive(false);
    }

    public void UI_Start()
    {
        StartEvent.Invoke();
        isARStoneFollowCamera = true;
        targetStone.SetActive(true);
    }
    public void UI_Enter()
    {
        if (Mathf.Abs(movieCubeTransform.localPosition.x)<0.5)
        {
            Invoke("right", 1);
            ani.speed = 0;
            ani.SetBool("OK", false);
            gameMan.transform.localPosition = new Vector3(-0.03f, -0.56f, 0f);

        }
        else
        {
            OverEvent.Invoke();
            ani.speed = 0;
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
        speakMan.SetActive(true);
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
