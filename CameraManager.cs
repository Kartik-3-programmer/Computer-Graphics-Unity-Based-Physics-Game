using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform maze; // Reference to the maze
    public float offset = 5f; // Offset for the camera's height
    public float dragSpeed = 2f; // Speed of camera movement when dragging

    private Vector3 dragOrigin; // To store the mouse drag origin position
    private bool isDragging = false; // To track if dragging is active

    void Start()
    {
        AdjustCamera();
    }

    public void AdjustCamera() // Change from private to public
    {
        if (maze != null)
        {
            // Calculate the center of the maze
            Vector3 mazeCenter = maze.position;
            mazeCenter.y = 0; // Keep the camera level

            // Set camera position above the center of the maze
            transform.position = mazeCenter + new Vector3(0, offset, 0);

            // Adjust camera's orthographic size or field of view based on maze size
            Camera cam = GetComponent<Camera>();
            if (cam.orthographic) // If using orthographic camera
            {
                float mazeWidth = maze.localScale.x; // Assuming maze is scaled properly
                float mazeLength = maze.localScale.z;

                cam.orthographicSize = Mathf.Max(mazeWidth, mazeLength) / 2 + offset; // Adjust size
            }
            else // If using perspective camera
            {
                float mazeWidth = maze.localScale.x;
                float mazeLength = maze.localScale.z;

                float maxDimension = Mathf.Max(mazeWidth, mazeLength);
                cam.fieldOfView = 60; // Default FOV, you can adjust based on needs
                transform.position = new Vector3(mazeCenter.x, offset, mazeCenter.z - maxDimension); // Adjust for perspective view
            }

            transform.LookAt(mazeCenter); // Look at the center of the maze
        }
    }

    void Update()
    {
        HandleMouseDrag();
    }

    private void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0)) // On left mouse button press
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging) // While the left mouse button is held
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 dragDelta = dragOrigin - currentMousePos;

            // Move the camera opposite to the drag direction
            Camera cam = GetComponent<Camera>();
            if (cam.orthographic)
            {
                transform.Translate(new Vector3(dragDelta.x, 0, dragDelta.y) * dragSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                Vector3 movement = new Vector3(dragDelta.x, 0, dragDelta.y) * dragSpeed * Time.deltaTime;
                transform.Translate(movement, Space.Self);
            }

            dragOrigin = currentMousePos; // Update drag origin
        }

        if (Input.GetMouseButtonUp(0)) // On left mouse button release
        {
            isDragging = false;
        }
    }
}
