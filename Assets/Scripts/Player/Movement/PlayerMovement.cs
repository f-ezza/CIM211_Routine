using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementStates { Walking, Sprinting, Crouching}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Capabilites")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private LayerMask walkableMask;

    [Header("Movement Variables")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float crouchSpeed = 5f;
    [SerializeField] private float sprintSpeed = 5f;
    private float moveSpeed = 0f;

    [Header("Aerial Variables")]
    [SerializeField] private float airDrag;
    [SerializeField] private float groundDrag;

    [Header("Jumping Variables")]
    [SerializeField] private bool shouldJump = false;
    [SerializeField] private float jumpForce = 8f;

    [Header("State Tracker")]
    [SerializeField] private MovementStates movementState;
    [SerializeField] private bool isFacingRight;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isMoving;

    [Header("Player Input Actions")]
    [SerializeField] private IA_Main mainActionMap;
    [SerializeField] private InputAction move;
    [SerializeField] private InputAction sprint;
    [SerializeField] private InputAction jump;

    [Header("Helper Game Objects")]
    [SerializeField] private Transform groundCheck;

    [Header("External Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;


    private float moveHorizontal;
    private float moveVertical;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        mainActionMap = new IA_Main();
    }

    private void OnEnable()
    {
        move = mainActionMap.Player.Move;
        move.Enable();

        sprint = mainActionMap.Player.Sprint;
        sprint.Enable();

        sprint.performed += Sprint;
        sprint.canceled += StopSprint;

        jump = mainActionMap.Player.Jump;
        jump.Enable();

        jump.performed += Jump;
    }

    private void OnDisable()
    {
        move.Disable();
        sprint.Disable();
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        movementState = MovementStates.Sprinting;
    }

    private void StopSprint(InputAction.CallbackContext context)
    {
        movementState = MovementStates.Walking;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(rb.linearVelocityX, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        if (!isGrounded && rb.linearDamping != airDrag)
        {
            rb.linearDamping = airDrag;
        }
        else if(isGrounded && rb.linearDamping != groundDrag)
        {
            rb.linearDamping = groundDrag;
        }


        moveHorizontal = move.ReadValue<Vector2>().x;
        moveVertical = move.ReadValue<Vector2>().y;

        CheckState();
    }

    private void FixedUpdate()
    {
        ApplyFinalMovements();
    }

    private void CheckState()
    {
        switch (movementState)
        {
            case MovementStates.Walking:
                moveSpeed = walkSpeed;
                break;
            case MovementStates.Crouching:
                moveSpeed = crouchSpeed;
                break;
            case MovementStates.Sprinting:
                moveSpeed = sprintSpeed;
                break;

        }
    } 

    private void ApplyFinalMovements()
    {
        rb.linearVelocity = new Vector2(moveHorizontal * moveSpeed, rb.linearVelocityY);
        Flip();
    }

    private void Flip()
    {
        if(isFacingRight && moveHorizontal < 0 || !isFacingRight && moveHorizontal > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.5f, walkableMask))
        {
            return true;
        }

        return false;
    }

}
