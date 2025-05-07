using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCamera;  // Reference to the camera transform

    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 3f;
    public float jumpHeight = 2.5f;
    public float gravity = -15f;

    public float crouchHeight = 1.2f;
    public float normalHeight = 2f;
    public float crouchCameraHeight = 0.75f; // Adjusted height for camera when crouching
    public float normalCameraHeight = 1.5f;  // Default camera height

    public float crouchTransitionSpeed = 10f; // Speed at which the camera moves when crouching

    private Vector3 velocity;
    private int jumpCount = 0;
    private bool isGrounded;
    private bool isCrouching = false;

    public bool IsCrouching()
{
    return isCrouching;
}


    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0; // Reset jump count when grounded
        }

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        if (isCrouching) speed = crouchSpeed;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        //Debug.Log("Move Vector"+ move);

        // Jumping
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
        }

        // Crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            controller.height = crouchHeight;
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && CanStandUp())
        {
            controller.height = normalHeight;
            isCrouching = false;
        }


        // Smooth Camera Height Transition
        float targetCameraHeight = isCrouching ? crouchCameraHeight : normalCameraHeight;
        playerCamera.localPosition = Vector3.Lerp(
            playerCamera.localPosition, 
            new Vector3(playerCamera.localPosition.x, targetCameraHeight, playerCamera.localPosition.z),
            Time.deltaTime * crouchTransitionSpeed
        );

        // Apply Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    bool CanStandUp()
{
    float headClearance = normalHeight - crouchHeight;
    Vector3 start = transform.position + Vector3.up * crouchHeight;
    float radius = controller.radius * 0.95f;

    // Check if there's room above the crouching head
    return !Physics.CheckSphere(start, radius, ~0, QueryTriggerInteraction.Ignore);
}

}
