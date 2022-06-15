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

    private void Start()
    {
        var findbutton = Resources.FindObjectsOfTypeAll<Button>();

        SoundManager.AddButtonClick(findbutton);

        print(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name.Equals("Title") == true)
        {
            BGMSlider.onValueChanged.AddListener((volume) => { SoundManager.Instance.BgmVolume = volume; });
            BGMSlider.value = SoundManager.Instance.BgmVolume;
        }
        else
        {
            BackButton.onClick.AddListener(() => { SceneManager.LoadScene("Title"); });

            for (int i = 0; i < LunchBox.childCount; i++)
            {
                lunchBoxChilds.Add(LunchBox.GetChild(i).gameObject);
            }

            int estagestatenum = GameManager.StageNum - 1;

            Mat.sprite = MatSprites[estagestatenum];

            SpeechBubbleNum.sprite = SpeechBubbleNumSprites[estagestatenum];
        }
    }
}
