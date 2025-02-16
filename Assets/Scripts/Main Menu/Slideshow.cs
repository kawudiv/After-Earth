using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlideshowController : MonoBehaviour
{
    public Image[] slides;
    public TMP_Text slideText;
    public string[] slideTexts = new string[5]; // 5 slides with unique text
    public float slideDuration = 5f;
    public float typingSpeed = 0.05f;

    private int currentSlideIndex = 0;

    void Start()
    {
        if (slides.Length != 5 || slideTexts.Length != 5)
        {
            Debug.LogError("There must be exactly 5 slides and 5 text entries.");
            return;
        }

        foreach (Image slide in slides)
        {
            slide.gameObject.SetActive(false);
        }
        slides[0].gameObject.SetActive(true);

        StartCoroutine(PlaySlideshow());
    }

    IEnumerator PlaySlideshow()
    {
        for (int i = 0; i < 5; i++)
        {
            slideText.text = "";
            yield return StartCoroutine(TypeText(slideTexts[i]));
            yield return new WaitForSeconds(3f);

            slides[i].gameObject.SetActive(false);

            if (i < 4)
            {
                slides[i + 1].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator TypeText(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            slideText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
