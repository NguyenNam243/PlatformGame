using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -100f;
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private float smoothTime = 0.01f;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private float bendDamp = 0.1f;

    [Header("Object Reference")]
    [SerializeField] private Transform groundCheckPoint = null;

    public bool IsJump { get; private set; }

    private bool grounded = false;
    private Vector2 refVelocity = Vector2.zero;

    private Rigidbody2D rgBody = null;
    private Animator ator = null;

    public float horizontal { get; private set; }
    private Vector2 targetVelocity = Vector2.zero;


    private void Awake()
    {
        rgBody = GetComponent<Rigidbody2D>();
        ator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayerMask) != null;

        if (grounded)
            IsJump = false;

        rgBody.AddForce(Vector2.down * gravity);

        Move();

        Jump();

        Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ator.SetTrigger("Attack");
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            IsJump = true;
            rgBody.velocity = new Vector2(rgBody.velocity.x, 0);
            rgBody.AddForce(Vector2.up * jumpForce);
        }
    }

    private void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        SetAnimationMovement(Mathf.Abs(horizontal));

        bool horizontalDown = horizontal != 0;
        if (horizontalDown)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, horizontal < 0 ? 180 : 0, transform.eulerAngles.z);

        targetVelocity = new Vector2(horizontal * moveSpeed, rgBody.velocity.y);

        rgBody.velocity = Vector2.SmoothDamp(rgBody.velocity, targetVelocity, ref refVelocity, smoothTime);
    }

    private void SetAnimationMovement(float speed)
    {
        ator.SetFloat("Speed", speed, bendDamp, Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
}
