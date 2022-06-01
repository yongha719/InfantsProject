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

    public Button BackButton;

    private void Start()
    {
        BackButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Title");
        });

        int estagestatenum = (int)GameManager.Estagestate;

        Mat.sprite = MatSprites[estagestatenum];
        SpeechBubbleNum.sprite = SpeechBubbleNumSprites[estagestatenum];
    }
}
