using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the camera moves
    private float minX, maxX, minY, maxY;  // Boundaries for camera movement
    private float zoomSpeed = 2f;  // Zoom speed (increases/decreases the orthographic size)
    private float minZoom = 5f;    // Minimum zoom level (max zoomed in)
    private float maxZoom = 15f;   // Maximum zoom level (max zoomed out)

    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;

        // Set initial camera position
        Vector3 newPosition = transform.position;
        newPosition.z = -10f;  // Set the camera's Z position to -10 (for 2D)
        transform.position = newPosition;

        // Set the camera's initial orthographic size (zoom level)
        _camera.orthographicSize = 10f; // Adjust the initial zoom level
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    // Initialize the camera boundaries based on the environment size
    public void Initialize(float maxWidth, float maxHeight)
    {
        minX = 0f;
        maxX = maxWidth;
        minY = 0f;
        maxY = maxHeight;
    }

    void HandleMovement()
    {
        // Get horizontal and vertical input (arrow keys or WASD)
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveY = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        // Move the camera
        Vector3 movement = new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Clamp the camera's position within the bounds of the environment
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        _camera.transform.position = clampedPosition;
    }

    void HandleZoom()
    {
        // Get scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            // Modify the camera's orthographic size based on the scroll input
            _camera.orthographicSize -= scrollInput * zoomSpeed;

            // Clamp the orthographic size between minZoom and maxZoom
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);
        }
    }
}
