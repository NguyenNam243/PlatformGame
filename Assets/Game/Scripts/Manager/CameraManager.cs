using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private Transform followTarget = null;
    [SerializeField] private float distanceFollow = 10f;

    [Header("Configuration")]
    [SerializeField] private float smoothTime = 0.01f;


    private Vector3 target;
    private Vector3 refVelocity = Vector3.zero;



    private void Update()
    {
        target = new Vector3(followTarget.position.x, followTarget.position.y, -distanceFollow);
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target, ref refVelocity, smoothTime);
    }
}
