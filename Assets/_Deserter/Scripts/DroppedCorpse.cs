using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedCorpse : MonoBehaviour
{
    private Rigidbody2D _rb2D;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        Invoke("DisableGravityFall", 0.8f);
    }

    private void DisableGravityFall()
    {
        _rb2D.gravityScale = 0f;
        _rb2D.velocity = Vector3.zero;
    }
}
