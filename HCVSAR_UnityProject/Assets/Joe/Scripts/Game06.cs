using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game06 : MonoBehaviour
{
    public GameObject RotateModel;

    private float y;

    void Update()
    {
        y += Time.deltaTime * 20;
        RotateModel.transform.rotation = Quaternion.Euler(0, 90+ y, 0);
    }
}
