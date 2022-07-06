using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peach : MonoBehaviour
{
    public GameObject peach; 
    private void OnEnable()
    {
        peach.SetActive(true);
    }
}
