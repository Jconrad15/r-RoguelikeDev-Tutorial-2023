using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{

    public void StartGame()
    {
        StartCoroutine(LoadYourAsyncScene(1));
    }

    public void GoToMainMenu()
    {
        StartCoroutine(LoadYourAsyncScene(0));
    }

    private IEnumerator LoadYourAsyncScene(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
