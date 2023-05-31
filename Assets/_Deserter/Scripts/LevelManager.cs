using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject wireFencePref_01;
    [SerializeField] private GameObject wireFencePref_02;

    private List<GameObject> wireFences = new List<GameObject>();

    private void Awake()
    {
        wireFences.Add(Instantiate(wireFencePref_01));
        wireFences.Add(Instantiate(wireFencePref_01));
        wireFences.Add(Instantiate(wireFencePref_01));
        wireFences.Add(Instantiate(wireFencePref_01));
        wireFences.Add(Instantiate(wireFencePref_01));
        wireFences.Add(Instantiate(wireFencePref_01));
        wireFences.Add(Instantiate(wireFencePref_01));
        wireFences.Add(Instantiate(wireFencePref_01));
        wireFences.Add(Instantiate(wireFencePref_02));
        wireFences.Add(Instantiate(wireFencePref_02));
        wireFences.Add(Instantiate(wireFencePref_02));
        wireFences.Add(Instantiate(wireFencePref_02));
        wireFences.Add(Instantiate(wireFencePref_02));
        wireFences.Add(Instantiate(wireFencePref_02));
        wireFences.Add(Instantiate(wireFencePref_02));
        wireFences.Add(Instantiate(wireFencePref_02));
        wireFences.Add(Instantiate(wireFencePref_02));
        wireFences.Add(Instantiate(wireFencePref_02));



        foreach (GameObject wireFence in wireFences)
        {
            wireFence.transform.position = new Vector3(999, 999, 0);
        }
    }


    private void FixedUpdate()
    {
        MoveFarawayWireFencesToPlayersNearby();
    }

    private void MoveFarawayWireFencesToPlayersNearby()
    {
        foreach (GameObject wireFence in wireFences)
        {
            if (Vector3.Distance(wireFence.transform.position, player.position) > 12f)
            {
                bool needsReset = true;
                int resetNum = 1;
                while (needsReset && resetNum < 50)
                {
                    resetNum += 1;
                    RandomlySetFenceNearbyPlayer(wireFence);
                    needsReset = false;
                    foreach (GameObject wireFence_ in wireFences)
                    {
                        if (wireFence_ != wireFence)
                        {
                            if (Vector3.Distance(wireFence.transform.position, wireFence_.transform.position) < 1f)
                            {
                                needsReset = true;
                            }
                        }
                    }
                }
            }
        }
    }

    private void RandomlySetFenceNearbyPlayer(GameObject fenceIn)
    {
        float distanceFromPlayer = 7.5f;
        float randomOffsetWide = UnityEngine.Random.Range(-7.0f, 7.0f);
        float randomOffsetNarrow = UnityEngine.Random.Range(-1.0f, 1.0f);
        int randowDirection = UnityEngine.Random.Range(0, 4);
        switch (randowDirection)
        {
            case 0:// 生成在玩家右侧
                {
                    fenceIn.transform.position = player.position + new Vector3(randomOffsetNarrow + distanceFromPlayer, randomOffsetWide, 0);
                    break;
                }
            case 1: // 生成在下方
                {
                    fenceIn.transform.position = player.position + new Vector3(randomOffsetWide, -distanceFromPlayer + randomOffsetNarrow, 0);
                    break;
                }
            case 2: // 生成在左侧
                {
                    fenceIn.transform.position = player.position + new Vector3(-distanceFromPlayer + randomOffsetNarrow, randomOffsetWide, 0);
                    break;
                }
            case 3:// 生成在上方
                {
                    fenceIn.transform.position = player.position + new Vector3(randomOffsetWide, distanceFromPlayer + randomOffsetNarrow, 0);
                    break;
                }
        }

    }
}
