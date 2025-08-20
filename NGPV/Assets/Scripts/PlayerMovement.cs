using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;

    [Header("Speed")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;

    [Header("Physics")]
    public float gravity = -9.81f;
    public float jumpHeight = 1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public bool IsSprinting { get; private set; }   // ← будем читать в бобинге

    Vector3 velocity;
    bool isGrounded;

    void Reset() { controller = GetComponent<CharacterController>(); }

    void Update()
    {
        // земля
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);
        if (isGrounded && velocity.y < 0f) velocity.y = -2f;

        // ввод
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        IsSprinting = Input.GetKey(KeyCode.LeftShift) && (Mathf.Abs(x) > 0.01f || Mathf.Abs(z) > 0.01f);

        // направление по камере
        Vector3 fwd = cameraTransform.forward; fwd.y = 0; fwd.Normalize();
        Vector3 right = cameraTransform.right; right.y = 0; right.Normalize();
        Vector3 move = right * x + fwd * z;

        float speed = IsSprinting ? sprintSpeed : walkSpeed;
        controller.Move(move * speed * Time.deltaTime);

        // прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // гравитация
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
