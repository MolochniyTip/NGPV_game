using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 100f;

    [Header("Recoil")]
    public float recoilReturnSpeed = 8f;   // скорость возврата камеры после кика
    float recoilPitch;                     // текущий подъЄм ствола (в градусах)

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // обычное управление
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // отдача по вертикали: вычитаем Ђреальнуюї отдачу (камера задираетс€ вверх)
        recoilPitch = Mathf.Lerp(recoilPitch, 0f, recoilReturnSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(xRotation - recoilPitch, 0f, 0f);

        // горизонталь Ч как обычно (по мыши)
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // вызывать из Gun, когда стрел€ем
    public void AddRecoil(float upDegrees, float rightDegrees)
    {
        recoilPitch += Mathf.Max(0f, upDegrees);        // подброс вверх (накапливаем)
        playerBody.Rotate(Vector3.up * rightDegrees);    // мгновенный сдвиг по горизонту (игрок компенсирует мышью)
    }
}
