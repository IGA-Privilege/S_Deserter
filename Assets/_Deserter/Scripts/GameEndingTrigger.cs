using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndingTrigger : MonoBehaviour
{
    [SerializeField] private Helicopter helicopter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            TriggerGameEnding();
        }
    }

    private void TriggerGameEnding()
    {
        helicopter.gameObject.SetActive(true);
    }
}
