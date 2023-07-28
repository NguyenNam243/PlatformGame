using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private float maxHealth = 10f;

    [Header("Object Reference")]
    [SerializeField] private Transform groundCheckPoint = null;
    [SerializeField] private Image healthBarFill = null;

    public bool IsJump { get; private set; }
    public bool Alive { get; private set; }

    private bool grounded = false;
    private Vector2 refVelocity = Vector2.zero;

    private Rigidbody2D rgBody = null;
    private Animator ator = null;

    public float horizontal { get; private set; }
    private Vector2 targetVelocity = Vector2.zero;

    private float currentHealth;


    private void Awake()
    {
        rgBody = GetComponent<Rigidbody2D>();
        ator = GetComponent<Animator>();

        Alive = true;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!Alive)
            return;

        grounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayerMask) != null;
        IsJump = Input.GetKeyDown(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        if (!Alive)
            return;

        rgBody.AddForce(Vector2.down * gravity);

        Move();

        Jump();

        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log("Jump");

        Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ator.SetTrigger("Attack");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        float ratio = currentHealth / maxHealth;

        healthBarFill.DOFillAmount(ratio, 0.25f);

        //healthBarFill.fillAmount = ratio;

        if (currentHealth <= 0)
        {
            Alive = false;
            ator.SetTrigger("Die");
        }
    }

    private void Jump()
    {
        if (IsJump && grounded)
        {
            rgBody.velocity = new Vector2(rgBody.velocity.x, rgBody.velocity.y);
            rgBody.AddForce(Vector2.up * jumpForce);
            Debug.Log("Jump");
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
