using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneByIndexAsync : MonoBehaviour
{
    public GameObject loadingScreen;

    private AsyncOperation handle;

    private void OnEnable()
    {
        EventBus<SceneLoadEvent>.Subscribe(LoadScene);
    }
    private void OnDisable()
    {
        EventBus<SceneLoadEvent>.Unsubscribe(LoadScene);
    }
    private void LoadScene(SceneLoadEvent evt)
    {
        handle = SceneManager.LoadSceneAsync(evt.sceneIndex);
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        loadingScreen.SetActive(true);
        handle.allowSceneActivation = false;

        StartCoroutine(WaitUntilLoaded());
    }

    private IEnumerator WaitUntilLoaded()
    {
        while (handle.progress < 0.9f)
        {
            yield return null;
        }
        handle.allowSceneActivation = true;
    }
}
