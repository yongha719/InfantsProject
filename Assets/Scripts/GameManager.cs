using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum EStageState
{
    One, Two, Three, Four, Five, Six, End
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public EStageState Estagestate;

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


    public void StartStage1()
    {
        randomInstBox(Boxs, 1);
    }

    void randomInstBox(List<Button> buttons, int selectnum)
    {
        Instantiate(ParentRt, GameObject.Find("Canvas").transform);

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

    

    public void Invoke(string name)
    {
        Invoke(name, 0);
    }
}
