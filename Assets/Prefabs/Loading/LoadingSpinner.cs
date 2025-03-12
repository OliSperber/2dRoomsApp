using UnityEngine;

public class LoadingSpinner : MonoBehaviour
{
    public float spinSpeed = 300f; // Rotation speed

    void Update()
    {
        // Rotate the object continuously around the Z-axis (for 2D rotation)
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}
