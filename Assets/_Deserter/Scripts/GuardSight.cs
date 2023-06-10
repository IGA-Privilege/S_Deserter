using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            if (!player.isHiding)
            {
                Debug.Log("You are detected!");
            }
        }
    }
}
