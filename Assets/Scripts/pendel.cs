using UnityEngine;

public class Pendulum3D : MonoBehaviour
{
    public GameObject craneTip;
    public LineRenderer lineRenderer;

    public float length = 5f;
    public float damping = 0.98f;
    public float gravity = 9.82f;

    private float angle = 0f;
    private float angularVelocity = 0f;
    private float angularAcceleration = 0f;

    public float liftSpeed = 1f;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        lineRenderer.positionCount = 2;  

        Vector3 craneTipPosition = craneTip.transform.position;
        transform.position = craneTipPosition - Vector3.up * length;
        angle = 0f;
        angularVelocity = 0f;
    }

    void Update()
    {
        Vector3 craneTipPosition = craneTip.transform.position;

        Vector3 direction = transform.position - craneTipPosition;
        float distance = direction.magnitude;

        if (distance > length)
        {
            direction.Normalize();
            transform.position = craneTipPosition + direction * length;
            distance = length;
        }
        float deltaX = transform.position.x - craneTipPosition.x;
        float deltaZ = transform.position.z - craneTipPosition.z;
        float horizontalDistance = Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ);

        float ratio = horizontalDistance / length;
        ratio = Mathf.Clamp(ratio, -1f, 1f);

        angle = Mathf.Asin(ratio);

        angularAcceleration = -(gravity / length) * Mathf.Sin(angle);

        angularVelocity += angularAcceleration * Time.deltaTime;

        angularVelocity *= damping;

        angle += angularVelocity * Time.deltaTime;

        angle = Mathf.Clamp(angle, -Mathf.PI / 2f, Mathf.PI / 2f);

        float x = Mathf.Sin(angle) * length + craneTipPosition.x;
        float y = -Mathf.Cos(angle) * length + craneTipPosition.y;
        float z = Mathf.Sin(angle) * length + craneTipPosition.z;
        transform.position = new Vector3(x, y, z);

        lineRenderer.SetPosition(0, craneTipPosition);
        lineRenderer.SetPosition(1, transform.position);

        if (Input.GetKey(KeyCode.W))
        {
            craneTip.transform.Translate(Vector3.up * liftSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            craneTip.transform.Translate(Vector3.down * liftSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            craneTip.transform.Translate(Vector3.forward * liftSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            craneTip.transform.Translate(Vector3.back * liftSpeed * Time.deltaTime);
        }
    }
}
