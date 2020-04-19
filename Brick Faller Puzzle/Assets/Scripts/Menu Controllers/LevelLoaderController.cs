using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoaderController : MonoBehaviour
{
    public GameObject loadingPanel;
    public Slider loaderSlider;

    private static LevelLoaderController instance;

    void Start()
    {
        instance = this;
    }

    public static LevelLoaderController Get()
    {
        return instance;
    }

    public void LoadScene(string sSceneName)
    {
        loadingPanel.SetActive(true);

        StartCoroutine(LoadAsync(sSceneName));
    }

    IEnumerator LoadAsync(string sSceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sSceneName);

        while (!operation.isDone)
        {
            //Unity scene loader :
            //0 - 90% = Loading
            //90% - 100% = activation
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loaderSlider.value = progress;

            yield return null;
        }
    }
}
