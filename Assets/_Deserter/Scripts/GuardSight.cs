using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSight : MonoBehaviour
{
    [SerializeField] private Guard guard;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            if (!player.isHiding)
            {
                guard.SetChasingPlayer(player);
            }
        }
    }
}
