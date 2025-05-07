using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 150f;
    public Transform playerBody;

    public float bobbingAmount = 0.05f;
    public float bobbingSpeed = 5f;
    public float sprintBobbingMultiplier = 1.8f;

    public Transform armTransform;  // Reference to the arm/weapon
    private Vector3 defaultArmPos;

    private float xRotation = 0f;
    private float timer = 0f;
    private bool isSprinting = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        xRotation = transform.localRotation.eulerAngles.x;

        if (armTransform != null)
        {
            defaultArmPos = armTransform.localPosition;
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        // Sprinting detection
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        float currentBobbingSpeed = isSprinting ? bobbingSpeed * sprintBobbingMultiplier : bobbingSpeed;

        bool isMoving = Mathf.Abs(Input.GetAxis("Vertical")) > 0 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0;

        // Bobbing effect
        if (isMoving)
        {
            timer += Time.deltaTime * currentBobbingSpeed;
            float bobOffset = Mathf.Sin(timer) * bobbingAmount;

            if (armTransform != null)
            {
                armTransform.localPosition = new Vector3(defaultArmPos.x, defaultArmPos.y + bobOffset, defaultArmPos.z);
            }
        }
        else
        {
            timer = 0;
            if (armTransform != null)
            {
                armTransform.localPosition = Vector3.Lerp(armTransform.localPosition, defaultArmPos, Time.deltaTime * 8f);
            }
        }
    }
}
