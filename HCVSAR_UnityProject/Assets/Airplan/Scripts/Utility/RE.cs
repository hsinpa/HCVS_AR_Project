using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class RE : MonoBehaviour {
    //測試用重新
 
    void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Player", LoadSceneMode.Additive);
        }

    }
}
