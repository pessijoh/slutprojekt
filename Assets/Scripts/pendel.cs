using UnityEngine;

public class Pendulum3D : MonoBehaviour
{
    public Transform pivotPoint;
    public LineRenderer lineRenderer;
    public float stringLength = 5f;
    public float minStringLength = 2f;
    public float maxStringLength = 10f;
    public float stringAdjustSpeed = 2f;
    public float gravity = 9.81f; 
    public float dampingFactor = 0.999f;

    private Vector3 velocity = Vector3.zero;

    public void Start()
    {
        transform.position = pivotPoint.position + Vector3.down * stringLength;
    }

    public void Update()
    {
        AdjustStringLength();

        Vector3 direction = transform.position - pivotPoint.position;
        float currentLength = direction.magnitude;

        direction.Normalize();

        Vector3 acceleration = Vector3.down * gravity;

        Vector3 tension = direction * Vector3.Dot(acceleration, direction);
        acceleration -= tension;

        velocity += acceleration * Time.deltaTime;

        velocity *= dampingFactor;

        transform.position += velocity * Time.deltaTime;

        transform.position = pivotPoint.position + (transform.position - pivotPoint.position).normalized * stringLength;

        DrawString();
    }

    void AdjustStringLength()
    {
        if (Input.GetKey(KeyCode.E))
        {
            stringLength = Mathf.Clamp(stringLength + stringAdjustSpeed * Time.deltaTime, minStringLength, maxStringLength);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            stringLength = Mathf.Clamp(stringLength - stringAdjustSpeed * Time.deltaTime, minStringLength, maxStringLength);
        }
    }

    void DrawString()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, pivotPoint.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }
}
