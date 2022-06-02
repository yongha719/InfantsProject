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

    private void Start()
    {
        BackButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Title");
        });

        for (int i = 0; i < LunchBox.childCount; i++)
        {
            lunchBoxChilds.Add(LunchBox.GetChild(i).gameObject);
        }

        int estagestatenum = GameManager.StageNum;

        Mat.sprite = MatSprites[estagestatenum];

        SpeechBubbleNum.sprite = SpeechBubbleNumSprites[estagestatenum];

        //lunchBoxChilds[estagestatenum].gameObject.SetActive(true);
    }
}
