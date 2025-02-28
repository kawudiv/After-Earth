using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class Mainmenuroute : MonoBehaviour
{

    public TextMeshProUGUI tapanywheretoplay;
    public int nextSceneIndex = 1;
    public float fadeDuration = 1.5f;

    private void Start()
    {
        if(tapanywheretoplay != null)
        {
            StartCoroutine(FadeText());
        }   
    }

    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButton(0))
        {
            SceneManager.LoadScene(nextSceneIndex);
        }   
    }


    private IEnumerator FadeText()
    {
        while (true)
        {
            for (float t = 0; t < fadeDuration; t+= Time.deltaTime)
            {
                float alpha = Mathf.Lerp(1, 0, t / fadeDuration);
                tapanywheretoplay.color = new Color(tapanywheretoplay.color.r, tapanywheretoplay.color.g, tapanywheretoplay.color.b, alpha);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
                tapanywheretoplay.color = new Color(tapanywheretoplay.color.r, tapanywheretoplay.color.g, tapanywheretoplay.color.b, alpha);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Continue()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void NewGame()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void Back()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
