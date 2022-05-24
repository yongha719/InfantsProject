using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    Button button => GetComponent<Button>();

    private void Awake() =>
        button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Title");
        });
}
