using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer mainChopper;
    [SerializeField] private SpriteRenderer tailChopper;
    [SerializeField] private Transform hoverPoint;
    [SerializeField] private Transform extractPoint;
    [SerializeField] private DroppedCorpse droppedCorpsePref;
    [SerializeField] private List<Sprite> corpseSprites = new List<Sprite>();
    private bool _hasReachedHoverPoint = false;
    private bool _hasDroppedCorpses = false;

    private void Update()
    {
        if (!_hasReachedHoverPoint)
        {
            FlyToHoverPoint();
            if (Vector2.Distance(transform.position, hoverPoint.position) < 0.5f)
            {
                _hasReachedHoverPoint = true;
            }
        }
        else if (!_hasDroppedCorpses)
        {
            DropCorpses();
            _hasDroppedCorpses = true;
        }
        else
        {
            FlyToExtractionPoint();
            if (Vector2.Distance(transform.position, extractPoint.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FlyToExtractionPoint()
    {
        float flySpeed = 0.02f;
        transform.position = Vector3.Lerp(transform.position, extractPoint.position, flySpeed * Time.deltaTime);
        transform.right = (extractPoint.position - transform.position);
    }

    private void DropCorpses()
    {
        foreach (var corpseSprite in corpseSprites)
        {
            SpriteRenderer droppedCorpse = Instantiate(droppedCorpsePref, transform.position + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.05f, 0.05f), 0), Quaternion.identity).GetComponent<SpriteRenderer>();
            droppedCorpse.sprite = corpseSprite;
        }
        GameManager.OnHelicopterDroppedCorpses.Invoke();
    }

    private void FlyToHoverPoint()
    {
        float flySpeed = 0.2f;
        transform.position = Vector3.Lerp(transform.position, hoverPoint.position, flySpeed * Time.deltaTime);
        transform.right = (hoverPoint.position - transform.position);
    }

    private void FixedUpdate()
    {
        TickChoppersRotation();
    }

    private void TickChoppersRotation()
    {
        mainChopper.transform.eulerAngles = new Vector3(0, 0, mainChopper.transform.eulerAngles.z + 30f);
        tailChopper.transform.eulerAngles = new Vector3(0, 0, tailChopper.transform.eulerAngles.z + 30f);
    }
}
