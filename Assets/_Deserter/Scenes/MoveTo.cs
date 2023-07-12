using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveTo : MonoBehaviour
{
    public float moveTime;
    public float targetX;
    private bool isOpen = false;
    private float originalX;

    private void Start()
    {
        originalX = transform.position.x;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOpen)
            {
                transform.DOMoveX(originalX, moveTime);
              
            }
            else
            {
                transform.DOMoveX(targetX, moveTime);
            }
            isOpen = !isOpen;
        }
    }
}
