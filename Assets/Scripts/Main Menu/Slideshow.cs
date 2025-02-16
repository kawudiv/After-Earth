using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideshowController : MonoBehaviour
{
    public Image[] slides;
    public float slideDuration = 5f; 
    private int currentSlideIndex = 0;

    void Start()
    {
        foreach (Image slide in slides)
        {
            slide.gameObject.SetActive(false);
        }
        slides[0].gameObject.SetActive(true);

        StartCoroutine(PlaySlideshow());
    }

    IEnumerator PlaySlideshow()
    {
        while (true)
        {
            yield return new WaitForSeconds(slideDuration);

            slides[currentSlideIndex].gameObject.SetActive(false);

            currentSlideIndex = (currentSlideIndex + 1) % slides.Length;

            slides[currentSlideIndex].gameObject.SetActive(true);

            if (currentSlideIndex == slides.Length - 1)
            {
                yield return new WaitForSeconds(slideDuration);
                break;
            }
        }
    }
}
