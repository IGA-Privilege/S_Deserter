using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AutoDoor : MonoBehaviour
{
    [SerializeField] private Transform doorLeftPart;
    [SerializeField] private Transform doorRightPart;
    [SerializeField] private Transform leftPartOpenPoint;
    [SerializeField] private Transform rightPartOpenPoint;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private AudioClip sound;
    private Vector3 _leftPartClosePosition;
    private Vector3 _rightPartClosePosition;
    private AudioSource _audioSource;

    private bool isOpen = false;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _leftPartClosePosition = doorLeftPart.position;
        _rightPartClosePosition = doorRightPart.position;
    }

    private void Update()
    {
        TickPlayerDetection();
    }

    private void TickPlayerDetection()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, 1f, playerLayerMask);
        if (playerCollider != null)
        {
            OnInteract();
        }
    }

    public void OnInteract()
    {
        if (!isOpen)
        {
            StartCoroutine(DoorOpen());
        }
    }

    private IEnumerator DoorOpen()
    {
        _audioSource.PlayOneShot(sound);

        isOpen = true;

        float openSpeed = 1f;
        while (doorLeftPart.position != leftPartOpenPoint.position)
        {
            doorLeftPart.position = Vector3.Lerp(doorLeftPart.position, leftPartOpenPoint.position, openSpeed * Time.deltaTime);
            doorRightPart.position = Vector3.Lerp(doorRightPart.position, rightPartOpenPoint.position, openSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0);
        }
    }
}
