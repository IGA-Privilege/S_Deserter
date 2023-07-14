using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanHideBehindObj : MonoBehaviour
{
    private SpriteRenderer _playerSprite;
    private SpriteRenderer _spriteRenderer;
    private float _spriteAlpha;
    private float spriteAlpha { get { return _spriteAlpha; } set { _spriteAlpha = Mathf.Clamp(value, 0.4f, 1f); } }

    private void Awake()
    {
        _playerSprite = FindObjectOfType<PlayerController>().GetComponent<SpriteRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        spriteAlpha = 1f;
    }

    private void Update()
    {
        if (_playerSprite.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
        {
            bool isHiding = Vector2.Distance(sprite.bounds.center, transform.position) < PlayerController.HIDE_DISTANCE;
            if (isHiding) 
            {
                spriteAlpha -= 1f * Time.deltaTime;
            }
        }
        spriteAlpha += 0.5f * Time.deltaTime;
        _spriteRenderer.color = new Color(1, 1, 1, spriteAlpha);
    }

}
