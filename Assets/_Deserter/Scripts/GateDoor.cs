using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDoor : MonoBehaviour
{
    [SerializeField] private Transform doorLeftPart;
    [SerializeField] private Transform doorRightPart;
    [SerializeField] private Transform leftPartOpenPoint;
    [SerializeField] private Transform rightPartOpenPoint;
    private Vector3 _leftPartClosePosition;
    private Vector3 _rightPartClosePosition;

    private bool isOpen = false;


    private void Start()
    {
        _leftPartClosePosition = doorLeftPart.position;
        _rightPartClosePosition = doorRightPart.position;
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
