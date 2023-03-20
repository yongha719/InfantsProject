using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Fade : MonoBehaviour
{
    public static Fade Instance { get; private set; }

    const float fadeTime = 2f;
    WaitForSeconds waitTime = new WaitForSeconds(fadeTime);

    RectTransform Clouds;
    [SerializeField] private RectTransform lower_left;
    [SerializeField] private RectTransform lower_right;
    [SerializeField] private RectTransform upper_left;
    [SerializeField] private RectTransform upper_right;

    private void Awake()
    {
        Clouds = GetComponent<RectTransform>();

        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
    }
    /// <summary>
    /// Fade In
    /// </summary>
    /// <param name="GotoTitle"> if true go title scene else go stage scene </param>
    public void FadeIn(bool GotoTitle = false) => StartCoroutine(CFadeIn(GotoTitle));
    private IEnumerator CFadeIn(bool goToTItle = false)
    {
        lower_left.DOAnchorPos(Vector2.zero, fadeTime);
        lower_right.DOAnchorPos(Vector2.zero, fadeTime);
        upper_left.DOAnchorPos(Vector2.zero, fadeTime);
        upper_right.DOAnchorPos(Vector2.zero, fadeTime);

        //SoundManager.Instance.Play(SoundName.FADE);

        yield return waitTime;

        SceneManager.LoadScene(goToTItle == true ? SsceneName.TITLESCENE : SsceneName.STAGESCENE);
        UIManager.ShouldFade = goToTItle;
    }

    public void FadeOut() => StartCoroutine(CFadeOut());

    private IEnumerator CFadeOut()
    {
        lower_left.DOAnchorPos(new Vector2(-1100, -700), fadeTime);
        lower_right.DOAnchorPos(new Vector2(1100, -700), fadeTime);
        upper_left.DOAnchorPos(new Vector2(-1100, 700), fadeTime);
        upper_right.DOAnchorPos(new Vector2(1100, 700), fadeTime);

        yield return waitTime;
    }


}
