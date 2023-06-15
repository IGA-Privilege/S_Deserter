using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorpion : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;


    private void FixedUpdate()
    {
        SearchForPlayer();
    }

    private void SearchForPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, 2.2f, playerLayerMask);
        if (player != null)
        {
            ChasePlayer(player.transform);
            FacePlayer(player.transform);
        }
    }

    private void ChasePlayer(Transform player)
    {
        if (Vector3.Distance(player.position, transform.position) < 0.2f)
        {
            return;
        }
        else
        {
            float moveSpeed = 0.01f;
            transform.position += (player.position - transform.position).normalized * moveSpeed;
        }
    }

    private void FacePlayer(Transform player)
    {
        float turningSpeed = 0.05f;
        transform.up = Vector3.Lerp(transform.up, (player.position - transform.position).normalized, turningSpeed);
    }
}
