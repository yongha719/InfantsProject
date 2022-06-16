using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Image Mat;
    public List<Sprite> MatSprites = new List<Sprite>();

    public Image SpeechBubbleNum;
    public List<Sprite> SpeechBubbleNumSprites = new List<Sprite>();

    public Transform LunchBox;
    private List<GameObject> lunchBoxChilds = new List<GameObject>();

    public Button BackButton;

    public Slider BGMSlider;
    public Slider EffectSlider;

    private void Start()
    {
        var findbutton = Resources.FindObjectsOfTypeAll<Button>();

        SoundManager.AddButtonClick(findbutton);

        if (SceneManager.GetActiveScene().name.Equals("1.Title") == true)
        {
            BGMSlider.onValueChanged.AddListener((volume) => { SoundManager.Instance.BgmVolume = volume; });
            BGMSlider.value = SoundManager.Instance.BgmVolume;
            EffectSlider.onValueChanged.AddListener((volume) => { SoundManager.Instance.SoundVolume = volume; });
            EffectSlider.value = SoundManager.Instance.SoundVolume;
        }
        else
        {
            BackButton.onClick.AddListener(() => Fade.Instance.FadeIn(true));

            for (int i = 0; i < LunchBox.childCount; i++)
            {
                lunchBoxChilds.Add(LunchBox.GetChild(i).gameObject);
            }

            int stagenum = GameManager.StageNum - 1;

            Mat.sprite = MatSprites[stagenum];

            SpeechBubbleNum.sprite = SpeechBubbleNumSprites[stagenum];
        }
    }
}
