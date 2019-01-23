using UnityEngine;

public class CameraController : MonoBehaviour
{
    private enum MovementState { None, Positive, Negative };

    [Tooltip("Time between movement steps (in seconds)")]
    public float timeBetweenSteps = 0.05f;

    public float cameraRotationSpeed = 2;

    private float movementTimer = 0,
                  movementSpeed = 1;
    private MovementState forwardMovementState = MovementState.None,
                          sidewaysMovementState = MovementState.None;
    private float cameraYaw = 0,
                  cameraPitch = 0;

    private void Start()
    {
        Screen.SetResolution(1280, 720, false);
    }

    void Update()
    {
        // Set speed to 1 m/s; 10 m/s if shift key held.
        movementSpeed = (Input.GetKey(KeyCode.LeftShift)
            || Input.GetKey(KeyCode.RightShift)) ? 10 : 1;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            forwardMovementState = MovementState.Positive;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            forwardMovementState = MovementState.Negative;
        }
        else
        {
            forwardMovementState = MovementState.None;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            sidewaysMovementState = MovementState.Negative;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            sidewaysMovementState = MovementState.Positive;
        }
        else
        {
            sidewaysMovementState = MovementState.None;
        }

        movementTimer += Time.deltaTime;
        if (movementTimer > timeBetweenSteps)
        {
            PerformCameraMovements();
            movementTimer = 0f;
        }
    }

    private void PerformCameraMovements()
    {
        // Adjust the position of the camera.
        Vector3 newPosition = transform.localPosition;
        switch (forwardMovementState)
        {
            case MovementState.Negative:
                transform.position -= movementSpeed * timeBetweenSteps * Camera.main.transform.forward;
                break;

            case MovementState.Positive:
                transform.position += movementSpeed * timeBetweenSteps * Camera.main.transform.forward;
                break;

            case MovementState.None:
            default:
                break;
        }

        switch (sidewaysMovementState)
        {
            case MovementState.Negative:
                transform.position -= movementSpeed * timeBetweenSteps * Camera.main.transform.right;
                break;

            case MovementState.Positive:
                transform.position += movementSpeed * timeBetweenSteps * Camera.main.transform.right;
                break;

            case MovementState.None:
            default:
                break;
        }

        // Adjust the rotation of the camera.
        cameraYaw += cameraRotationSpeed * Input.GetAxis("Mouse X");
        cameraPitch -= cameraRotationSpeed * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(cameraPitch, cameraYaw);
    }
}