using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Control Settings")]
    public bool canMove = false; // Flag pentru a controla dacă jucătorul se poate mișca

    [Header("Speed Settings")]
    public float initialPlayerSpeed = 2f;
    public float targetPlayerSpeed = 5f;
    public float accelerationDistance = 1000f;

    [Header("Horizontal Movement Settings")]
    public float horizontalSpeed = 3f;
    public float rightLimit = 5.5f;
    public float leftLimit = -5.5f;

    [Header("Rotation Settings")]
    public float turnAngle = 30f;
    public float turnSpeed = 5f;

    private float currentYRotation = 0f;
    private float targetYRotation = 0f;
    private Transform cameraTransform;
    private Quaternion originalCameraRotation;

    // Proprietate publică pentru a permite accesul din alte scripturi
    public float CurrentPlayerSpeed { get; internal set; }

    void Start()
    {
        CurrentPlayerSpeed = initialPlayerSpeed;

        Camera mainCamera = GetComponentInChildren<Camera>();
        if (mainCamera != null)
        {
            mainCamera.tag = "MainCamera"; // Forțează tag-ul corect
            cameraTransform = mainCamera.transform;
            originalCameraRotation = cameraTransform.localRotation;
        }
        else
        {
            Debug.LogWarning("No camera found as a child of the player!");
        }
    }

    void Update()
    {
        // Verifică dacă jucătorul poate să se miște

        if (!canMove)
        {
            // Forțează resetarea vitezei în fiecare frame
            CurrentPlayerSpeed = 0f;
            return;
        }

        // Calculăm viteza curentă în funcție de distanța parcursă
        if (DistanceCounter.instance != null)
        {
            float currentDistance = DistanceCounter.instance.TotalDistance;

            if (currentDistance <= accelerationDistance)
            {
                float t = currentDistance / accelerationDistance;
                CurrentPlayerSpeed = Mathf.Lerp(initialPlayerSpeed, targetPlayerSpeed, t);
            }
            else
            {
                CurrentPlayerSpeed = targetPlayerSpeed;
            }
        }

        transform.Translate(Vector3.forward * Time.deltaTime * CurrentPlayerSpeed, Space.World);

        // Mișcare stânga-dreapta și rotație:
        targetYRotation = 0f;

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > leftLimit)
        {
            transform.Translate(Vector3.left * Time.deltaTime * horizontalSpeed);
            targetYRotation = -turnAngle;
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < rightLimit)
        {
            transform.Translate(Vector3.right * Time.deltaTime * horizontalSpeed);
            targetYRotation = turnAngle;
        }

        currentYRotation = Mathf.Lerp(currentYRotation, targetYRotation, Time.deltaTime * turnSpeed);
        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);

        if (cameraTransform != null)
        {
            Quaternion counterRotation = Quaternion.Euler(0, -currentYRotation, 0);
            cameraTransform.localRotation = counterRotation * originalCameraRotation;
        }
    }
}
