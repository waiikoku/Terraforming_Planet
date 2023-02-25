using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneObject(sceneName));
    }

    public IEnumerator LoadSceneObject(string sceneName)
    {
        //https://thaiunitydev.weebly.com/36103607358836233634361736263635362736193633361036093633358536143633360236093634364835853617/-load-scene-scenemanagerloadsceneasync
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);

            if (progress == 1f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}