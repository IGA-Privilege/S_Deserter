using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    [SerializeField] private Transform level2SpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            SendPlayerToLevel2(player);
        }
    }

    private void SendPlayerToLevel2(PlayerController player)
    {
        player.mainBody_RB.transform.position = level2SpawnPoint.position;
    }
}
