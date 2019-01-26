using System;
using System.Timers;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private enum MovementState { None, Positive, Negative };

    [Tooltip("Time between movement steps")]
    public float timeBetweenSteps = 0.02f;

    public float cameraRotationSpeed = 6;

    private float movementTimer = 0,
                  movementSpeed = 1;
    private MovementState forwardMovementState = MovementState.None,
                          sidewaysMovementState = MovementState.None;
    private float cameraYaw = 0,
                  cameraPitch = 0;

    private float verticalSpeed = 0,
                  horizontalSpeed = 0;

    private void Start()
    {
        Screen.SetResolution(1280, 720, false);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
    }

    int hCount = 0, vCount = 0;
    private void Update()
    {
        float newH = Input.GetAxis("Mouse X");
        if (newH != 0)
        {
            horizontalSpeed = newH;
        }
        else
        {
            if (hCount++ > 32)
            {
                horizontalSpeed = 0;
                hCount = 0;
            }
        }

        float newV = Input.GetAxis("Mouse Y");
        if (newV != 0)
        {
            verticalSpeed = newV;
        }
        else
        {
            if (vCount++ > 32)
            {
                verticalSpeed = 0;
                vCount = 0;
            }
        }
    }

    private void FixedUpdate()
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
        cameraYaw += cameraRotationSpeed * horizontalSpeed;
        cameraPitch -= cameraRotationSpeed * verticalSpeed;
        transform.eulerAngles = new Vector3(cameraPitch, cameraYaw);
    }
}