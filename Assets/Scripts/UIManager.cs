using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public List<Sprite> MatSprites = new List<Sprite>();

    public Image Mat;
    private void Start()
    {
        Mat.sprite = MatSprites[(int)GameManager.Estagestate];
    }
}
