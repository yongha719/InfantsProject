using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public void GoToTitle() => SceneManager.LoadScene("Title");
}
