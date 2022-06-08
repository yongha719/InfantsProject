using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public static Fade Instance { get; private set; } = null;

    RectTransform[] Clouds;

    private void Awake()//Awake¾îÂ¿Æ¼ºñ
    {
        Clouds = Resources.LoadAll<RectTransform>("Cloud/");
    }
    public void FadeIn()
    {
        StartCoroutine(EFadeIn());
    }

    IEnumerator EFadeIn()
    {
        yield return null;
    }

    public void FadeOut()
    {
        StartCoroutine(EFadeOut());
    }

    IEnumerator EFadeOut()
    {
        yield return null;
    }
}
