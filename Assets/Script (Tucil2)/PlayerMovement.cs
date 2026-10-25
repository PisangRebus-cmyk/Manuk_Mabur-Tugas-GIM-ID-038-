using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    public int FacingDirection { get; private set; } = 1;   // 1 = right, -1 = left

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float moveInput;
    private bool jumpPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        moveInput = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  moveInput -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) moveInput += 1f;

        if (moveInput != 0f)
            FacingDirection = moveInput > 0f ? 1 : -1;

        if (kb.spaceKey.wasPressedThisFrame || kb.wKey.wasPressedThisFrame)
            jumpPressed = true;
        
        if (moveInput != 0f)
        {
            FacingDirection = moveInput > 0f ? 1 : -1;
            sr.flipX = FacingDirection < 0;   // sprite faces right by default
        }
    }

    private void FixedUpdate()
    {
    rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

    if (jumpPressed && IsGrounded())
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        AudioManager.Play(Sfx.Jump);
    }

    jumpPressed = false;   // must stay OUTSIDE the if, at the very end
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }
}