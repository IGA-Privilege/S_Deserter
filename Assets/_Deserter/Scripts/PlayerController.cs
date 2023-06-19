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
        float maxChargeTime = 1.0f; // ʵ���������ʱ�� = maxChargeTime + clickThreshold

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
        // ��Ϊ��ʵ�����Ƶ����޴������룬��Ҫ����ָʾ���Ļ��Χ��
        // 
        // ���Ȼ�ȡ���λ��
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // �Ӽ��λ������귽��������
        Vector2 shoulderPos = shoulderPoint.position;
        RaycastHit2D raycastHit1 = Physics2D.Raycast(shoulderPos, mousePos - shoulderPos, 100f, playerLayerMask);
        // �����λ���������������
        RaycastHit2D raycastHit2 = Physics2D.Raycast(mousePos, shoulderPos - mousePos, 100f, playerLayerMask);
        // ���ײ����һ�£�˵������������ײ������棨������Χ�ˣ��������һ�£�˵�����˿��������ײ�������

        if (raycastHit1.point == raycastHit2.point)
        {
            // ���̫Զ�ˣ���ָʾ������Ϊ��ײ�㣨ʵ�ʵķ�Χ������ײ��ı߽磩
            cursorPos = raycastHit1.point;
        }
        else
        {
            // û�г���ָʾ��Χ��ֱ�ӽ�ָʾ������Ϊ���λ��
            cursorPos = mousePos;
        }


        Vector2 palmPos = palm.position;

        // Debug
        //visualSpot1.position = palmPos;
        //visualSpot2.position = cursorPos;

        // ����ָʾ��λ�ú͵�ǰ���Ƶ�λ��ȷ��Ҫ�ƶ��ķ���;���
        Vector3 moveVelocity = new Vector3((cursorPos - palmPos).x, (cursorPos - palmPos).y, 0f);


        // �ƶ�����
        float moveSpeed = 4f;
        palm.position += moveVelocity * moveSpeed * Time.deltaTime;


        // ��ת���Ƶ�С�����ڵĽǶ�
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
