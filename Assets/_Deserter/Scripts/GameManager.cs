using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform player;

    private void Awake()
    {
        Camera.main.transform.position = new Vector3(player.position.x, player.position.y, -10f);
    }

    private void Update()
    {
        if (Vector3.Distance(Camera.main.transform.position, player.position) > 2.0f)
        {
            CameraFollowsPlayer();
        }
    }


    private void CameraFollowsPlayer()
    {
        float lerpSpeed = 2f * Time.deltaTime;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(player.position.x, player.position.y, -10f), lerpSpeed);
    }
}
