using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private RectTransform ParentRt;

    [SerializeField]
    private List<Button> Boxs;

    [SerializeField]
    private List<Button> Bananas;

    //[SerializeField] 
    //private List <Button> Bananas;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void StartStageOne()
    {
        randomInstBox(Boxs, 1);


    }

    void randomInstBox(List<Button> buttons, int selectnum)
    {
        var instbuttons = new List<Button>();

        foreach (var button in buttons)
            instbuttons.Add(button);

        int count = instbuttons.Count;

        for (int i = 0; i < count; i++)
        {
            int num = Random.Range(0, instbuttons.Count);
            Button button = Instantiate(instbuttons[num], ParentRt);

            button.onClick.AddListener(() =>
            {
                if (selectnum == num)
                    CheckBasket(button.gameObject);
            });
            instbuttons.RemoveAt(num);
        }
    }

    void CheckBasket(GameObject button)
    {

    }
}
