using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleManager : MonoBehaviour
{
    public List<Button> LunchBoxBtns;
    public Button LunchBoxBtn;
    public RectTransform BoxsParent;
    Scrollbar scrollbar;

    void Start()
    {
        int temp = 1;
        //foreach (var lunchboxbtn in LunchBoxBtns)
        //{
        //    Button lunchBoxBtn = Instantiate(lunchboxbtn, BoxsParent);
        //    lunchBoxBtn.name = temp++.ToString();
        //    lunchBoxBtn.onClick.AddListener(() =>
        //    {
        //        print($"Stage{lunchBoxBtn.name}");
        //    });
        //}
    }

    void Update()
    {
        
    }


}
