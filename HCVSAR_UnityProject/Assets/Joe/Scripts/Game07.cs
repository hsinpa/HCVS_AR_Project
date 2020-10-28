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
    public GameObject stoneCurve;

    public Camera ARCamera;
    public Camera MainCamera;
    private Camera _camera;

    private bool isARStoneFollowCamera;

    void Start()
    {
        _camera = MissionsController.Instance.isARsupport ? ARCamera : MainCamera;

        arGameObject.SetActive(false);
        speakMan.SetActive(false);
        gameMan.SetActive(false);
        targetStone.SetActive(true);
    }

    public void UI_Start()
    {
        StartEvent.Invoke();
        stoneCurve.SetActive(false);
        isARStoneFollowCamera = true;
        arGameObject.SetActive(true);
        gameMan.SetActive(true);
    }
    public void UI_Enter()
    {
        if (Mathf.Abs(movieCubeTransform.localPosition.x)<0.8)
        {
            Invoke("right", 1);
            ani.speed = 0;
            ani.SetBool("OK", false);
            gameMan.transform.localPosition = new Vector3(-0.03f, -0.56f, 0f);
            if (MissionsController.Instance.isARsupport) { targetStone.SetActive(false); }
        }
        else
        {
            OverEvent.Invoke();
            ani.speed = 0;
            gameMan.GetComponent<Rigidbody>().isKinematic = false;
            if (MissionsController.Instance.isARsupport) { targetStone.SetActive(false); }
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
            isARStoneFollowCamera = false;

            var _cameraFront = _camera.transform.forward;
            var _frontPos = _cameraFront * 15;

            _cameraFront.y = 0;
            arGameObject.transform.position = _camera.transform.position + _frontPos;
            arGameObject.transform.rotation = Quaternion.LookRotation(_cameraFront);
        }
    }
}
