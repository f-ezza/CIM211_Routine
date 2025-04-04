using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Capabilites")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;

    [Header("Movement Variables")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float crouchSpeed = 5f;
    [SerializeField] private float sprintSpeed = 5f;
    private float moveSpeed = 0f;

    [Header("Jumping Variables")]
    [SerializeField] private bool shouldJump = false;
    [SerializeField] private float jumpForce = 8f;

    [Header("State Tracker")]
    [SerializeField] private bool isMoving;

    [Header("Input Action Maps")]
    [SerializeField] private IA_Main movementAction;

    [Header("External Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D capsuleCollider;

    [Header("Vectors")]
    [SerializeField] private Vector2 moveDirection;


    private float moveHorizontal;
    private float moveVertical;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnEnable()
    {
        movementAction.Movement.Enable();
    }

    private void OnDisable()
    {
        movementAction.Movement.Disable();
    }

    private void Update()
    {
        moveHorizontal = movementAction.Movement.Move.ReadValue<Vector2>().x;
        moveVertical = movementAction.Movement.Move.ReadValue<Vector2>().y;
    }

    private void FixedUpdate()
    {
        if(rb.linearVelocity.x > 0 || rb.linearVelocity.y > 0)
        {
            isMoving = true;
            rb.AddForce(new Vector2(moveHorizontal * walkSpeed, 0), ForceMode2D.Force);
        }
    }

}
