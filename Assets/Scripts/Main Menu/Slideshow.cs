using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SlideshowController : MonoBehaviour
{
    public Image[] slides;
    public TMP_Text slideText;
    public string[] slideTexts = new string[5]; 
    public float typingSpeed = 0.05f;
    public TMP_Text tapToContinueText;
    public string nextSceneName;

    private int currentSlideIndex = 0;
    private bool isTyping = false;

    void Start()
    {
        tapToContinueText.gameObject.SetActive(false);
        StartCoroutine(BlinkTapToContinue());
        
        if (slides.Length != 5 || slideTexts.Length != 5)
        {
            Debug.LogError("There must be 5 slides");
            return;
        }

        foreach (Image slide in slides)
        {
            slide.gameObject.SetActive(false);
        }
        slides[0].gameObject.SetActive(true);

        StartCoroutine(ShowSlide(currentSlideIndex));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTyping)
        {
            NextSlide();
        }
    }

    IEnumerator ShowSlide(int index)
    {
        isTyping = true;
        slideText.text = "";
        tapToContinueText.gameObject.SetActive(false);
        yield return StartCoroutine(TypeText(slideTexts[index]));
        isTyping = false;
        tapToContinueText.gameObject.SetActive(true);
        StartCoroutine(BlinkTapToContinue());
    }

    IEnumerator TypeText(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            slideText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator BlinkTapToContinue()
    {
        while (tapToContinueText.gameObject.activeSelf)
        {
            tapToContinueText.enabled = !tapToContinueText.enabled;
            yield return new WaitForSeconds(2f);
        }
    }

    void NextSlide()
    {
        tapToContinueText.gameObject.SetActive(false);
        slides[currentSlideIndex].gameObject.SetActive(false);
        currentSlideIndex++;

        if (currentSlideIndex < slides.Length)
        {
            slides[currentSlideIndex].gameObject.SetActive(true);
            StartCoroutine(ShowSlide(currentSlideIndex));
        }
        else
        {
            GoToNextScene();
        }
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
