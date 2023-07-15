using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private PlayerController player;
    [SerializeField] private Image transitionImage;
    [SerializeField] private RectTransform titleUI;
    [SerializeField] private Image titleImage;
    [SerializeField] private Color bloody;
    [SerializeField] private Color black;
    [SerializeField] private Transform level2SpawnPoint;
    [SerializeField] private Helicopter helicopter;
    [SerializeField] private Transform playerSuicidePoint;
    [SerializeField] private Collider2D colliderBlockingPlayerFromReturn;
    public static Action OnPlayerReachLevel2;
    public static Action OnPlayerTriggerGameEnding;
    public static Action OnHelicopterDroppedCorpses;
    private GameState _gameState;
    private float _transitionColorAlpha;
    private float transitionColorAlpha { get { return _transitionColorAlpha; } set { _transitionColorAlpha = Mathf.Clamp(value, 0f, 1f); } }


    private void Awake()
    {
        Camera.main.transform.position = new Vector3(playerBody.position.x, playerBody.position.y, -10f);
        _gameState = GameState.Title;
        OnPlayerReachLevel2 += SendPlayerToLevel2;
        OnPlayerTriggerGameEnding += CallHelicopter;
        OnHelicopterDroppedCorpses += PlayerSuicide;
    }

    private void Update()
    {
        if (Vector3.Distance(Camera.main.transform.position, playerBody.position) > 0.2f)
        {
            CameraFollowsPlayer();
            AudioSourceFollowsCamera();
        }

        if (_gameState == GameState.Title)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
            {
                StartGame();
            }
        }
    }

    private void AudioSourceFollowsCamera()
    {
        transform.position = Camera.main.transform.position;
    }

    private IEnumerator PlayGameStartAnimation()
    {
        yield return StartCoroutine(TransitionFadeIn(3f, bloody));

        while (titleImage.color.a > 0f)
        {
            titleImage.color = new Color(Color.white.r, Color.white.g, Color.white.b, Mathf.Max(0f, titleImage.color.a - 0.05f));
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2f);

        titleUI.gameObject.SetActive(false);

        yield return StartCoroutine(TransitionFadeIn(1f, black));
    }

    private IEnumerator TransitionFadeIn(float fadeInSec, Color transitionColor)
    {
        for (float time = 0; time < fadeInSec; time = time + Time.deltaTime)
        {
            transitionColorAlpha = 1f - time / fadeInSec;
            transitionImage.color = new Color(transitionColor.r, transitionColor.g, transitionColor.b, transitionColorAlpha);
            yield return new WaitForSeconds(0);
        }
    }

    private IEnumerator TransitionCrossFade(float fadeOutSec, float fadeInSec, float blackOutSec)
    {
        for (float time = 0; time < fadeOutSec; time = time + Time.deltaTime)
        {
            transitionColorAlpha = time / fadeOutSec;
            transitionImage.color = new Color(black.r, black.g, black.b, transitionColorAlpha);
            yield return new WaitForSeconds(0);
        }

        yield return new WaitForSeconds(blackOutSec);

        for (float time = 0; time < fadeInSec; time = time + Time.deltaTime)
        {
            transitionColorAlpha = 1f - time / fadeInSec;
            transitionImage.color = new Color(black.r, black.g, black.b, transitionColorAlpha);
            yield return new WaitForSeconds(0);
        }
    }

    private void StartGame()
    {
        _gameState = GameState.Playing;
        StartCoroutine(PlayGameStartAnimation());
    }

    private void CameraFollowsPlayer()
    {
        float lerpSpeed = 2f * Time.deltaTime;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(playerBody.position.x, playerBody.position.y, -10f), lerpSpeed);
    }

    private void SendPlayerToLevel2()
    {
        StartCoroutine(PlayAnimationAndGoToLevel2());
    }

    private IEnumerator PlayAnimationAndGoToLevel2()
    {
        float fadeOutSec = 2f;
        float fadeInSec = 2f;
        StartCoroutine(TransitionCrossFade(fadeOutSec, fadeInSec, 2f));
        yield return new WaitForSeconds(fadeOutSec);
        playerBody.transform.position = level2SpawnPoint.position;
    }

    private void CallHelicopter()
    {
        helicopter.gameObject.SetActive(true);
        colliderBlockingPlayerFromReturn.enabled = true;
    }

    private void PlayerSuicide()
    {
        StartCoroutine(PlayPlayerSuicideAnimation());
    }

    private IEnumerator PlayPlayerSuicideAnimation()
    {
        yield return StartCoroutine(TransitionFadeIn(2f, bloody));

        yield return new WaitForSeconds(2.5f);

        yield return StartCoroutine(TransitionFadeIn(2f, bloody));

        player.SetUnableToControl();
        player.StartCoroutine(player.SetConstantlyCrawling(playerBody.position + new Vector3(0, -10, 0)));

        yield return new WaitForSeconds(2.5f);

        while (playerBody.position.y > playerSuicidePoint.position.y)
        {
            yield return new WaitForSeconds(0);
        }

        playerBody.up = (playerBody.position - new Vector3(0, -1f, 0));
        playerBody.position += new Vector3(0, -4f, 0);
        player.StopAllCoroutines();
        player.enabled = false;

        yield return StartCoroutine(TransitionFadeIn(1f, bloody));
        playerBody.GetComponent<Rigidbody2D>().gravityScale = 0.2f;

        yield return new WaitForSeconds(0.8f);

        playerBody.GetComponent<Rigidbody2D>().gravityScale = 0f;
        playerBody.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}

public enum GameState
{
    Title, Playing
}
