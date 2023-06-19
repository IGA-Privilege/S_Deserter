using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform palm;
    [SerializeField] private Transform lowerArm;
    [SerializeField] private Transform shoulderPoint;
    [SerializeField] private Rigidbody2D mainBody_RB;
    [SerializeField] private Transform visualSpot1;
    [SerializeField] private Transform visualSpot2;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private Animator palmAnimator;
    [SerializeField] private GameObject dashArrowPref;

    private Vector2 cursorPos;
    private Vector3 bodyVelocity;
    private bool isCharging;
    private float mouseButtonTicker;
    private GameObject instantiatedDashArrow;
    [HideInInspector]
    public bool isHiding;


    private void Awake()
    {
        cursorPos = palm.position;
    }


    private void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            TryInteract();
        }

        float clickThreshold = 0.2f;
        float maxChargeTime = 1.0f; // 实际最大蓄力时间 = maxChargeTime + clickThreshold

        if (!isCharging)
        {
            PalmFollowsCursor();
        }

        if (Input.GetMouseButton(0))
        {
            mouseButtonTicker += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isCharging)
            {
                Vector2 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shoulderPoint.position;
                Dash((mouseButtonTicker - clickThreshold) / maxChargeTime, mouseDir.normalized);
                mouseButtonTicker = 0f;
                isCharging = false;
                palmAnimator.SetBool("isFisting", false);
                Destroy(instantiatedDashArrow);
            }
            else
            {
                Crawl();
                mouseButtonTicker = 0f;
            }
        }

        if (mouseButtonTicker > clickThreshold)
        {
            isCharging = true;
        }

        if (isCharging)
        {
            palmAnimator.SetBool("isFisting", true);

            if (instantiatedDashArrow == null)
            {
                instantiatedDashArrow = Instantiate(dashArrowPref);
                instantiatedDashArrow.transform.position = shoulderPoint.position;

                Vector3 mouseRelativePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - instantiatedDashArrow.transform.position;
                instantiatedDashArrow.transform.right = new Vector3(mouseRelativePos.x, mouseRelativePos.y, 0).normalized;
            }
            else
            {
                Vector3 mouseRelativePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - instantiatedDashArrow.transform.position;
                instantiatedDashArrow.transform.right = new Vector3(mouseRelativePos.x, mouseRelativePos.y, 0).normalized;

                instantiatedDashArrow.transform.position = shoulderPoint.position;

                float scaleSize = Mathf.Min(0.8f, 0.2f + (float)0.4 * (mouseButtonTicker - clickThreshold) / maxChargeTime);
                instantiatedDashArrow.transform.localScale = new Vector3(scaleSize, scaleSize, 1);
            }
        }
    }

    private void FixedUpdate()
    {
        HandleBodyMove();
    }

    private void Crawl()
    {
        palmAnimator.SetTrigger("Crawl");
        float crawlForce = 9f;
        bodyVelocity = (palm.position - shoulderPoint.position) * crawlForce;
    }

    private void Dash(float chargePercentage, Vector2 direction)
    {
        float dashForce = 10f;
        bodyVelocity = (1f + Mathf.Min((chargePercentage / 1), 1f)) * dashForce * direction;

    }

    private void HandleBodyMove()
    {
        if (bodyVelocity != Vector3.zero)
        {
            mainBody_RB.transform.right = -Vector3.Lerp(-mainBody_RB.transform.right, -(bodyVelocity.normalized), 0.05f);

            mainBody_RB.velocity = bodyVelocity;
            float frictionForce = 0.6f;
            bodyVelocity = bodyVelocity - bodyVelocity.normalized * frictionForce;
            if (bodyVelocity.magnitude < 0.35f)
            {
                bodyVelocity = Vector3.zero;
            }
        }
        else
        {
            mainBody_RB.velocity = Vector3.zero;
        }
    }

    private void PalmFollowsCursor()
    {
        // （为了实现手掌的有限触碰距离，需要限制指示器的活动范围）
        // 
        // 首先获取鼠标位置
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 从肩膀位置往鼠标方向发射射线
        Vector2 shoulderPos = shoulderPoint.position;
        RaycastHit2D raycastHit1 = Physics2D.Raycast(shoulderPos, mousePos - shoulderPos, 100f, playerLayerMask);
        // 从鼠标位置往肩膀方向发射射线
        RaycastHit2D raycastHit2 = Physics2D.Raycast(mousePos, shoulderPos - mousePos, 100f, playerLayerMask);
        // 如果撞击点一致，说明鼠标在玩家碰撞箱的外面（超出范围了）；如果不一致，说明鼠标此刻在玩家碰撞箱的里面

        if (raycastHit1.point == raycastHit2.point)
        {
            // 鼠标太远了，将指示器设置为碰撞点（实际的范围会是碰撞箱的边界）
            cursorPos = raycastHit1.point;
        }
        else
        {
            // 没有超出指示范围，直接将指示器设置为鼠标位置
            cursorPos = mousePos;
        }


        Vector2 palmPos = palm.position;

        // Debug
        //visualSpot1.position = palmPos;
        //visualSpot2.position = cursorPos;

        // 根据指示器位置和当前手掌的位置确定要移动的方向和距离
        Vector3 moveVelocity = new Vector3((cursorPos - palmPos).x, (cursorPos - palmPos).y, 0f);


        // 移动手掌
        float moveSpeed = 4f;
        palm.position += moveVelocity * moveSpeed * Time.deltaTime;


        // 旋转手掌到小臂所在的角度
        palm.right = (Vector2)Vector3.Lerp(palm.right, lowerArm.right, 0.05f);
    }


    private void TryInteract()
    {
        Collider2D interactable = Physics2D.OverlapCircle(shoulderPoint.position, 1f, interactableLayerMask);
        if (interactable)
        {
            if (interactable.TryGetComponent<Door>(out Door door))
            {
                door.Open();
            }
        }
    }

}
