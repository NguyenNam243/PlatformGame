using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform point1 = null;
    [SerializeField] private Transform point2 = null;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stopingDistance = 0.5f;
    [SerializeField] private float timeWaitPerWay = 2f;

    private Vector2[] movementPoint;
    private Vector2 groundContact = Vector2.zero;

    private Vector2 nextPoint = Vector2.zero;
    private int nextIndex = 0;
    private float timeOut = 0;

    private bool grounded = false;
    private bool onWait = false;

    private Rigidbody2D rgBody = null;
    private Animator ator = null;


    private void Awake()
    {
        rgBody = GetComponent<Rigidbody2D>();
        ator = GetComponent<Animator>();

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 10, groundLayerMask);
        if (hitInfo.collider != null)
        {
            groundContact = hitInfo.point;
            Debug.Log("Has contact: " + hitInfo.collider.gameObject.name);
        }

        point1.position = new Vector2(point1.position.x, groundContact.y);
        point2.position = new Vector2(point2.position.x, groundContact.y);

        movementPoint = new Vector2[] { point1.position, point2.position };
        nextIndex = 0;
        nextPoint = movementPoint[nextIndex];
    }

    private void FixedUpdate()
    {
        if (!grounded)
            return;

        Vector2 newPos = new Vector2(transform.position.x, nextPoint.y);
        Vector2 moveDirection = nextPoint - newPos;
        if (!onWait)
        {
            rgBody.velocity = moveDirection.normalized * moveSpeed;
        }

        if (Vector2.Distance(newPos, nextPoint) < stopingDistance && !onWait)
        {
            onWait = true;
            rgBody.velocity = Vector2.zero;
        }

        if (onWait)
        {
            timeOut += Time.deltaTime;

            if (timeOut >= timeWaitPerWay)
            {
                if (nextIndex >= movementPoint.Length - 1)
                    nextIndex = 0;
                else
                    nextIndex++;

                nextPoint = movementPoint[nextIndex];
                onWait = false;
                timeOut = 0;
            }
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rgBody.velocity.x < 0 ? 180 : 0, transform.eulerAngles.z);

        ator.SetFloat("Speed", rgBody.velocity.magnitude / moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            grounded = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(point1.position, 0.1f);
        Gizmos.DrawSphere(point2.position, 0.1f);
        Gizmos.DrawSphere(groundContact, 0.1f);
        Gizmos.DrawLine(point1.position, point2.position);
    }
}
