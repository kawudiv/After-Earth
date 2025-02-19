using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class AsyncLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject connew;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;

    [SerializeField] private float minLoadTime = 2f;

    public void LoadingLevelBtn(string levelToLoad)
    {
        connew.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        loadOperation.allowSceneActivation = false;
        float startTime = Time.time;

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;

            if (loadOperation.progress >= 0.9f && Time.time - startTime >= minLoadTime)
            {
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
