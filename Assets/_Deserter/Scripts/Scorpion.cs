using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Scorpion : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private AudioClip sound;
    private AudioSource _audioSource;
    private bool _isChasingPlayer = false;
    private float _viewDistance = 5f;
    private Rigidbody2D _rb2D;
    private MapFog _mapFog;
    private bool _isHiding { get { return Vector2.Distance(_mapFog.transform.position, transform.position) < 3f; }  }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rb2D = GetComponent<Rigidbody2D>();
        _mapFog = FindObjectOfType<MapFog>();
    }


    private void FixedUpdate()
    {
        if (!_isHiding)
        {
            SearchForPlayer();
        }
    }

    private void SearchForPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, _viewDistance, playerLayerMask);
        if (player != null)
        {
            PlayerController playerController = player.GetComponentInParent<PlayerController>();
            if (!playerController.isHiding)
            {
                if (!_isChasingPlayer)
                {
                    _audioSource.PlayOneShot(sound);
                }
                ChasePlayer(player.transform);
                FacePlayer(player.transform);
                _isChasingPlayer = true;
                return;
            }
        }

        _isChasingPlayer = false;
        _rb2D.velocity = Vector2.zero;
    }

    private void ChasePlayer(Transform player)
    {
        if (Vector3.Distance(player.position, transform.position) < 0.2f)
        {
            return;
        }
        else
        {
            float moveSpeed = 0.9f;
            _rb2D.velocity = (player.position - transform.position).normalized * moveSpeed;
        }
    }

    private void FacePlayer(Transform player)
    {
        float turningSpeed = 0.08f;
        transform.up = Vector3.Lerp(transform.up, (player.position - transform.position).normalized, turningSpeed);
    }
}
