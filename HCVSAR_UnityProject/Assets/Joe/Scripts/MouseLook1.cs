using System;
using UnityEngine;
using System.Collections;

public class MouseLook1 : MonoBehaviour
{

    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public float MinimumY = -90F;
    public float MaximumY = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;


    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private bool m_cursorIsLocked = true;

    public Transform C;
    //public Transform T;
    Gyroscope m_Gyro;
    void Start()
    {
        //Init(T,C);
        m_Gyro = Input.gyro;
        m_Gyro.enabled = true;

    }

    void Update()
    {
        //LookRotation(T,C);
        GyroModifyCamera();
    }

    // The Gyroscope is right-handed.  Unity is left handed.
    // Make the necessary change to the camera.
    void GyroModifyCamera()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0) * GyroToUnity(m_Gyro.attitude);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    public void Init(Transform character, Transform camera)
    {
        m_CharacterTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;
    }


    public void LookRotation(Transform character, Transform camera)
    {
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        //if(yRot)
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot,MinimumX,MaximumX);
            m_CharacterTargetRot = ClampRotationAroundYAxis(m_CharacterTargetRot, MinimumY,MaximumY);
        /* if (smooth)
         {
             character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                 smoothTime * Time.deltaTime);
             camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                 smoothTime * Time.deltaTime);
         }
         else
         {
             character.localRotation = m_CharacterTargetRot;
             camera.localRotation = m_CameraTargetRot;
         }*/

        character.localRotation = m_CharacterTargetRot;
        camera.localRotation = m_CameraTargetRot;

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q , float Minimum, float Maximum)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, Minimum, Maximum);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    Quaternion ClampRotationAroundYAxis(Quaternion q, float Minimum, float Maximum)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);

        angleX = Mathf.Clamp(angleX, Minimum, Maximum);

        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }


}
