using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LunchBoxs : MonoBehaviour
{
    public List<Button> Lunchboxsbtn;

    void Start()
    {
        foreach (var lbbtn in Lunchboxsbtn)
            lbbtn.onClick.AddListener(() => { GameManager.StageNum = int.Parse(lbbtn.name); });
    }
}
