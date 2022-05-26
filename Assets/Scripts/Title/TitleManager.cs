using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private List<Button> LunchBoxButtons;

    void Start()
    {
        setButtons();
    }

    private void setButtons()
    {
        foreach (var lbbtn in LunchBoxButtons)
        {
            lbbtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene($"Stage{lbbtn.name}");

                GameManager.Instance.Invoke($"StartStage{lbbtn.name}");  
            });
        }
    }
}
