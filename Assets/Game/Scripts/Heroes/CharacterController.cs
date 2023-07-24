using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -100f;
    public float jumpForce = 400f;
    public float smoothTime = 0.01f;

    public LayerMask groundLayerMask;
    public float groundCheckRadius = 0.1f;
    public Transform groundCheckPoint = null;

    private bool grounded = false;
    private Vector2 refVelocity = Vector2.zero;

    private Rigidbody2D rgBody = null;

    private float horizontal = 0f;
    private Vector2 targetVelocity = Vector2.zero;


    private void Awake()
    {
        rgBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayerMask) != null;

        horizontal = Input.GetAxis("Horizontal");

        bool horizontalDown = horizontal != 0;
        if (horizontalDown)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, horizontal < 0 ? 180 : 0, transform.eulerAngles.z);

        rgBody.AddForce(Vector2.down * gravity);

        targetVelocity = new Vector2(horizontal * moveSpeed, rgBody.velocity.y);

        rgBody.velocity = Vector2.SmoothDamp(rgBody.velocity, targetVelocity, ref refVelocity, smoothTime);

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rgBody.velocity = new Vector2(rgBody.velocity.x, 0);
            rgBody.AddForce(Vector2.up * jumpForce);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
}
