using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToMainScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CountToMain());
    }

    private IEnumerator CountToMain()
    {
        yield return new WaitForSeconds(0.5f);

        ChangeScene("Main");
    }

    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
