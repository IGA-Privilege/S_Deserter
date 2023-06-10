using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D disableCollider;

    private bool isOpened;

    private void Start()
    {
        isOpened = false;
    }

    public void Open()
    {
        if (!isOpened)
        {
            disableCollider.enabled = false;
            animator.SetBool("isOpen", true);
        }
    }
}
