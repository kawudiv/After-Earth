using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class AsyncLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject connew;

    [Header("UI Elements")]
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TMP_Text loadingText;

    [Header("Loading Settings")]
    [SerializeField] private float minLoadTime = 5f;
    [SerializeField] private float textChangeInterval = 1.5f;
    
    private string[] loadingMessages = {
        "Did you know? Bananas are berries, but strawberries aren't!",
        "Fun fact: Honey never spoils. Archaeologists found 3000-year-old honey still good to eat!",
        "Here's a cool one: Octopuses have three hearts and their blood is blue!",
        "Kawu"
    };

    public void LoadingLevelBtn(string levelToLoad)
    {
        connew.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync(levelToLoad));
        StartCoroutine(CycleLoadingMessages());
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

    IEnumerator CycleLoadingMessages()
    {
        int index = 0;
        while (loadingScreen.activeSelf)
        {
            loadingText.text = loadingMessages[index];
            index = (index + 1) % loadingMessages.Length;
            yield return new WaitForSeconds(textChangeInterval);
        }
    }
}
