using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] private GateDoor linkedDoor;

    public void OnInteract()
    {
        linkedDoor.OnInteract();
    }
}
