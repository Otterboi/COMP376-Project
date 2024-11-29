using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool fadeIn;
    [SerializeField] private bool fadeOut;
    [SerializeField] private float timeToFade;

    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            if(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += timeToFade * Time.deltaTime;

                if(canvasGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if (canvasGroup.alpha >= 0)
            {
                canvasGroup.alpha -= timeToFade * Time.deltaTime;

                if (canvasGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }

    }

    public void FadeIn()
    {
        fadeIn = true;

    }

    public void FadeOut()
    {
        fadeOut = true;
    }
}
