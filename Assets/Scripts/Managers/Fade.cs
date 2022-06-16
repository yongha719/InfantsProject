using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Fade : MonoBehaviour
{
    public static Fade Instance { get; private set; }

    const float fadeTime = 2f;
    WaitForSeconds waitTime = new WaitForSeconds(fadeTime);

    RectTransform Clouds;

    RectTransform lower_left;
    RectTransform lower_right;
    RectTransform upper_left;
    RectTransform upper_right;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            Clouds = GetComponent<RectTransform>();

            lower_left = Clouds.GetChild(0) as RectTransform;
            lower_right = Clouds.GetChild(1) as RectTransform;
            upper_left = Clouds.GetChild(2) as RectTransform;
            upper_right = Clouds.GetChild(3) as RectTransform;

            DontDestroyOnLoad(this);
        }

    }
    public void FadeIn() => StartCoroutine(EFadeIn());
    public void FadeOut(bool reload = false) => StartCoroutine(EFadeOut(reload));
    private IEnumerator EFadeIn()
    {

        lower_left.DOAnchorPos(Vector2.zero, fadeTime);
        lower_right.DOAnchorPos(Vector2.zero, fadeTime);
        upper_left.DOAnchorPos(Vector2.zero, fadeTime);
        upper_right.DOAnchorPos(Vector2.zero, fadeTime);

        //SoundManager.Instance.Play(SoundName.FADE);

        yield return waitTime;

        SceneManager.LoadScene("Stage");
    }

    private IEnumerator EFadeOut(bool reload = false)
    {
        lower_left.DOAnchorPos(new Vector2(-1100, -700), fadeTime);
        lower_right.DOAnchorPos(new Vector2(1100, -700), fadeTime);
        upper_left.DOAnchorPos(new Vector2(-1100, 700), fadeTime);
        upper_right.DOAnchorPos(new Vector2(1100, 700), fadeTime);

        yield return waitTime;

        if (reload == true)
            SceneManager.LoadScene("Stage");
    }
}
