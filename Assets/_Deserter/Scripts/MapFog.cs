using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MapFog : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private float _spriteAlpha = 1f;
    private float spriteAlpha { get { return _spriteAlpha; } set { _spriteAlpha = Mathf.Clamp(value, 0f, 1f); } }
    private SpriteRenderer _playerSprite;
    private bool _hasDispelled;


    private void Awake()
    {
        _hasDispelled = false;
        _sprite = GetComponent<SpriteRenderer>();
        _playerSprite = FindObjectOfType<PlayerController>().GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float dispelDistance = 5f;
        if (_playerSprite.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
        {
            if (Vector2.Distance(sprite.bounds.center, transform.position) < dispelDistance)
            {
                if (!_hasDispelled)
                {
                    _hasDispelled = true;
                    StartCoroutine(SetFogDispelled());
                }
            }
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
