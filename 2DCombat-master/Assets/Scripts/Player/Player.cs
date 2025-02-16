using CustomUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float jumpTime = 0.5f;
    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] private LayerMask whatIsGround;

    Rigidbody2D rigid;
    Animator anim;
    Collider2D coll;

    private bool isFacingRight = false;
    private bool isJumping;
    private bool isFalling;
    private RaycastHit2D groundHit;
    private float jumpTimeCounter;

    private Coroutine resetTrigger;

    [Header("Dash")]
    private bool isDashing;
    private TrailRenderer trailRenderer;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();

        DashSkill.OnSkillUsed += Dashing;
    }

    private void Update()
    {
        if (isDashing) return;

        Move();
        Jump();
    }

    private void Dashing(float DashPower, float DashTime)
    {
        StartCoroutine(DashEnable(DashPower, DashTime));
    }
    private IEnumerator DashEnable(float DashPower, float DashTime)
    {
        float faceValue = isFacingRight ? 1 : -1;

        isDashing = true;
        float originGravity = rigid.gravityScale;
        rigid.gravityScale = 0;
        rigid.velocity = new Vector2(faceValue * DashPower , 0);
        trailRenderer.emitting = true;

        yield return new WaitForSeconds(DashTime); // �뽬 ����
        trailRenderer.emitting = false;
        rigid.gravityScale = originGravity;
        isDashing = false;
    }


    private void Move()
    {
        // �Է� Control
        float horizontal = InputUser.Instance.moveInput.x;

        // �Է°��� �־��°�?
        if(Math.Abs(horizontal) > 0) // �Է°��� ���� ��
        {
            anim.SetBool("isWalking", true);
            TurnCheck();
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        // rigidbody2D
        rigid.velocity = new Vector2( horizontal * moveSpeed, rigid.velocity.y);
    }

    #region Flip

    private void TurnCheck()
    {
        if(InputUser.Instance.moveInput.x > 0 && !isFacingRight) // ����
        {
            Turn();
        }
        else if(InputUser.Instance.moveInput.x < 0 && isFacingRight) // �ĸ�
        {
            Turn();
        }
    }

    private void Turn()
    {
        if(isFacingRight) // ���� -> �ĸ�
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else // �ĸ� -> ����
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }
    #endregion

    private void Jump()
    {
        // �÷��̾��� ������
        if(InputUser.Instance.control.Jumping.Jump.WasPerformedThisFrame() && IsGrounded())
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }
        // �÷��̾��� ������
        if (InputUser.Instance.control.Jumping.Jump.IsPressed())
        {
           if(jumpTimeCounter > 0 && isJumping)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
                jumpTimeCounter -= Time.deltaTime;
            }
            else if(jumpTimeCounter == 0) // ������ ��
            {
                isJumping = false;
                isFalling = true;
            }
            else // ���� ����
            {
                isJumping = false;
            }
        }

        if(InputUser.Instance.control.Jumping.Jump.WasReleasedThisFrame())
        {
            isJumping = false;
            isFalling = true;
        }

        DrawGroundCheck();
    }

    private bool CheckForLand()
    {
        if(isFalling)
        {
            if(IsGrounded())
            {
                isFalling = false;

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private bool IsGrounded()
    {
        groundHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down,
            extraHeight, whatIsGround);

        if(groundHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    #region Debug Function

    private void DrawGroundCheck()
    {
        Color rayColor;

        if(IsGrounded())
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(coll.bounds.center +
            new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);

        Debug.DrawRay(coll.bounds.center -
          new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);

        Debug.DrawRay(coll.bounds.center - new Vector3(coll.bounds.extents.x, coll.bounds.extents.y + extraHeight),
            Vector2.right * (coll.bounds.extents.x * 2), rayColor);
    }

    #endregion
}

