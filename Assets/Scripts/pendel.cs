using UnityEngine;

public class Pendulum3D : MonoBehaviour
{
    public Transform pivotPoint; // Punkten där snöret är fäst (krantippen)
    public LineRenderer lineRenderer; // För att rita snöret
    public float stringLength = 5f; // Längden på snöret (kan ändras)
    public float minStringLength = 2f; // Minsta längd på snöret
    public float maxStringLength = 10f; // Maximal längd på snöret
    public float stringAdjustSpeed = 2f; // Hur snabbt snöret kan förlängas/förkortas
    public float gravity = 9.81f; // Tyngdkraften
    public float dampingFactor = 0.999f; // En mer rimlig dämpning

    private Vector3 velocity = Vector3.zero; // Hastigheten på bobben

    public void Start()
    {
        // Place the bob in the starting position below the pivot point
        transform.position = pivotPoint.position + Vector3.down * stringLength;
    }

    public void Update()
    {
        // Handle string length adjustments using keyboard input
        AdjustStringLength();

        // Calculate direction and distance from the pivot to the bob
        Vector3 direction = transform.position - pivotPoint.position;
        float currentLength = direction.magnitude;

        // Normalize the direction vector
        direction.Normalize();

        // Calculate acceleration due to gravity
        Vector3 acceleration = Vector3.down * gravity;

        // Adjust the acceleration to account for the string's constraint
        Vector3 tension = direction * Vector3.Dot(acceleration, direction);
        acceleration -= tension;

        // Update the velocity based on the calculated acceleration
        velocity += acceleration * Time.deltaTime;

        // Apply damping to slow down the pendulum
        velocity *= dampingFactor;

        // Update the position of the bob based on the velocity
        transform.position += velocity * Time.deltaTime;

        // Correct the position to keep the string length constant (constrain it)
        transform.position = pivotPoint.position + (transform.position - pivotPoint.position).normalized * stringLength;

        // Draw the string using the LineRenderer component
        DrawString();
    }

    void AdjustStringLength()
    {
        // Extend the string
        if (Input.GetKey(KeyCode.E))
        {
            stringLength = Mathf.Clamp(stringLength + stringAdjustSpeed * Time.deltaTime, minStringLength, maxStringLength);
        }

        // Shorten the string
        if (Input.GetKey(KeyCode.Q))
        {
            stringLength = Mathf.Clamp(stringLength - stringAdjustSpeed * Time.deltaTime, minStringLength, maxStringLength);
        }
    }

    void DrawString()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, pivotPoint.position); // Start på linan
            lineRenderer.SetPosition(1, transform.position); // Slut på linan
        }
    }
}
