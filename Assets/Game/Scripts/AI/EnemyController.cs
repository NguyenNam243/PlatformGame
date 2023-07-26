using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform point1 = null;
    [SerializeField] private Transform point2 = null;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stopingDistance = 0.5f;

    private Vector2[] movementPoint;
    private Vector2 groundContact = Vector2.zero;

    private Vector2 nextPoint = Vector2.zero;
    private int nextIndex = 0;

    private bool grounded = false;

    private Rigidbody2D rgBody = null;


    private void Awake()
    {
        rgBody = GetComponent<Rigidbody2D>();

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
        rgBody.velocity = moveDirection.normalized * moveSpeed;

        if (Vector2.Distance(newPos, nextPoint) < stopingDistance)
            Time.timeScale = 0;
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
