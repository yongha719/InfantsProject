using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EStageType
{
    One, Two, Three, Four, Five, Six, End
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public EStageType eStageType;
    [SerializeField]
    private RectTransform BoxParent;

    [SerializeField]
    private List<Button> Boxs;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        randomInstBox();
    }

    void Update()
    {

    }

    public void StartStageOne()
    {
        randomInstBox();
    }

    void randomInstBox()
    {
        var buttons = new List<Button>();

        foreach (var button in Boxs)
            buttons.Add(button);

        int count = buttons.Count;

        for (int i = 0; i < count; i++)
        {
            int num = Random.Range(0, buttons.Count);
            Button button = Instantiate(buttons[num], BoxParent);
            buttons.RemoveAt(num);
        }
    }
}
