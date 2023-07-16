using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class MapFog : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private float _spriteAlpha = 1f;
    private float spriteAlpha { get { return _spriteAlpha; } set { _spriteAlpha = Mathf.Clamp(value, 0f, 1f); } }

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            StartCoroutine(SetFogDispelled());
        }
    }

    private IEnumerator SetFogDispelled()
    {
        while (_sprite.color.a > 0.35f)
        {
            float dispelSpeed = 0.6f;
            spriteAlpha = Mathf.Lerp(spriteAlpha, 0f, dispelSpeed * Time.deltaTime);
            _sprite.color = new Color(0, 0, 0, spriteAlpha);
            yield return new WaitForSeconds(0);
        }

        transform.position += new Vector3(1000f, 0, 0);
    }
}
