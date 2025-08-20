using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 100f;

    [Header("Recoil")]
    public float recoilReturnSpeed = 8f;   // �������� �������� ������ ����� ����
    float recoilPitch;                     // ������� ������ ������ (� ��������)

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ������� ����������
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // ������ �� ���������: �������� ���������� ������ (������ ���������� �����)
        recoilPitch = Mathf.Lerp(recoilPitch, 0f, recoilReturnSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(xRotation - recoilPitch, 0f, 0f);

        // ����������� � ��� ������ (�� ����)
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // �������� �� Gun, ����� ��������
    public void AddRecoil(float upDegrees, float rightDegrees)
    {
        recoilPitch += Mathf.Max(0f, upDegrees);        // ������� ����� (�����������)
        playerBody.Rotate(Vector3.up * rightDegrees);    // ���������� ����� �� ��������� (����� ������������ �����)
    }
}
