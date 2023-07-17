using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorButton : MonoBehaviour
{
    [SerializeField] private GateDoor linkedDoor;
    [SerializeField] private AudioClip buttonSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnInteract()
    {
        _audioSource.PlayOneShot(buttonSound);
        linkedDoor.OnInteract();
    }
}
