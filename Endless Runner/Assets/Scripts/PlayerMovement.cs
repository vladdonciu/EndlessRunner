using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 2;
    public float horizontalSpeed = 3;
    public float rightLimit = 5.5f;
    public float leftLimit = -5.5f;

 
    public float turnAngle = 30f;        
    public float turnSpeed = 5f;         

    private float currentYRotation = 0f; 
    private float targetYRotation = 0f;  

   
    private Transform cameraTransform;
    private Quaternion originalCameraRotation;

    void Start()
    {
    
        Camera mainCamera = GetComponentInChildren<Camera>();
        if (mainCamera != null)
        {
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
        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);

        
        targetYRotation = 0f;

        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (this.gameObject.transform.position.x > leftLimit)
            {
                transform.Translate(Vector3.left * Time.deltaTime * horizontalSpeed);
                targetYRotation = -turnAngle; 
            }
        }

        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (this.gameObject.transform.position.x < rightLimit)
            {
                transform.Translate(Vector3.left * Time.deltaTime * horizontalSpeed * -1);
                targetYRotation = turnAngle; 
            }
        }

       
        currentYRotation = Mathf.Lerp(currentYRotation, targetYRotation, Time.deltaTime * turnSpeed);

     
        Vector3 currentRotation = transform.rotation.eulerAngles;

        
        transform.rotation = Quaternion.Euler(currentRotation.x, currentYRotation, currentRotation.z);

        if (cameraTransform != null)
        {
            
            Quaternion counterRotation = Quaternion.Euler(0, -currentYRotation, 0);
            cameraTransform.localRotation = counterRotation * originalCameraRotation;
        }
    }
}
