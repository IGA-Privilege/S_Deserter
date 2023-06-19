using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    [SerializeField] private BoxCollider2D m_BoxCollider;

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            if (player.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
            {
                bool isHiding = Vector2.Distance(sprite.bounds.center, m_BoxCollider.bounds.center) < 0.5f;
                player.isHiding = isHiding;
            }
        }
    }
}
