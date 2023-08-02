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
    [SerializeField] private float addedSpeed = 3f;
    [SerializeField] private float maxMana = 50f;
    [SerializeField] private float minusFirtSpeedup = 10f;
    [SerializeField] private float minusPerSecond = 1f;
    [SerializeField] private float timeAffterRecoverMana = 3f;
    [SerializeField] private float recoverPerSecond = 1f;

    [Header("Object Reference")]
    [SerializeField] private Transform groundCheckPoint = null;
    [SerializeField] private Image healthBarFill = null;
    [SerializeField] private Image manaFill = null;

    public bool IsJump { get; private set; }
    public bool Alive { get; private set; }

    private bool onJump = false;

    private bool grounded = false;
    private Vector2 refVelocity = Vector2.zero;

    private Rigidbody2D rgBody = null;
    private Animator ator = null;

    public float horizontal { get; private set; }
    private Vector2 targetVelocity = Vector2.zero;

    private float currentHealth;
    private float currentMana;

    private bool isSpeedup = false;
    private bool onSpeedup = false;

    private bool horizontalDown;

    private float countTimeMinusMana = 0f;
    private float countTimeRecoverMana = 0f;
    private float countTimeAddMana = 0f;


    private void Awake()
    {
        rgBody = GetComponent<Rigidbody2D>();
        ator = GetComponent<Animator>();

        Alive = true;
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    private void Update()
    {
        if (!Alive)
            return;

        horizontalDown = horizontal != 0;
        grounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayerMask) != null;
        IsJump = Input.GetKey(KeyCode.Space);
        isSpeedup = Input.GetKey(KeyCode.LeftShift) && horizontalDown;
    }

    private void FixedUpdate()
    {
        if (!Alive)
            return;

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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        float ratio = currentHealth / maxHealth;

        healthBarFill.DOFillAmount(ratio, 0.25f);

        if (currentHealth <= 0)
        {
            Alive = false;
            ator.SetTrigger("Die");
        }
    }

    private void Jump()
    {
        if (IsJump && grounded && !onJump)
        {
            onJump = true;
            rgBody.velocity = new Vector2(rgBody.velocity.x, 0);
            rgBody.AddForce(Vector2.up * jumpForce);
            Debug.Log("Jump");
        }
    }

    private void Move()
    {
        rgBody.AddForce(Vector2.down * gravity);

        horizontal = Input.GetAxis("Horizontal");
        SetAnimationMovement(Mathf.Abs(horizontal));

       
        if (horizontalDown)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, horizontal < 0 ? 180 : 0, transform.eulerAngles.z);

        float applyMoveSpeed = horizontalDown && isSpeedup ? moveSpeed + addedSpeed : moveSpeed;

        targetVelocity = new Vector2(horizontal * applyMoveSpeed, rgBody.velocity.y);

        rgBody.velocity = Vector2.SmoothDamp(rgBody.velocity, targetVelocity, ref refVelocity, smoothTime);

        if (isSpeedup && !onSpeedup)
        {
            onSpeedup = true;
            SetMana(currentMana - minusFirtSpeedup);
        }

        if (onSpeedup)
        {
            countTimeMinusMana += Time.deltaTime;
            if (countTimeMinusMana >= minusPerSecond)
            {
                currentMana -= minusPerSecond;
                SetMana(currentMana);
                countTimeMinusMana = 0;
            }
        }

        if (!isSpeedup)
            onSpeedup = false;

        if (!onSpeedup)
        {
            if (currentMana >= maxMana)
            {
                currentMana = maxMana;
                return;
            }

            countTimeRecoverMana += Time.deltaTime;
            if (countTimeRecoverMana >= timeAffterRecoverMana)
            {
                countTimeAddMana += Time.deltaTime;
                if (countTimeAddMana >= 1)
                {
                    currentMana += recoverPerSecond;
                    countTimeAddMana = 0;
                    SetMana(currentMana);
                }
            }
        }
    }

    private Tween fillTween = null;
    private void SetMana(float newValue)
    {
        if (fillTween != null)
            fillTween.Kill();

        float ratio = newValue / maxMana;
        fillTween = manaFill.DOFillAmount(ratio, 0.25f).SetEase(Ease.OutBack);
        currentMana = newValue;
    }

    private void SetAnimationMovement(float speed)
    {
        ator.SetFloat("Speed", speed, bendDamp, Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            onJump = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
}
