using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    public static Animator animator;

    void Awake() =>animator = GetComponent<Animator>();
    public void StopAnimator() => animator.speed = 0;
}
