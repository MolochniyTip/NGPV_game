using UnityEngine;

public class SimpleScreenShake : MonoBehaviour
{
    [Header("Shake")]
    public float posAmplitude = 0.05f;   // �����
    public float rotAmplitude = 1.5f;    // �������
    public float decay = 3f;             // �������� ���������
    public float frequency = 25f;        // ��

    float trauma;                        // 0..1
    Vector3 basePos;                     // ������� ��������� ������� (��� �����)

    void Awake() { basePos = transform.localPosition; }

    void LateUpdate() // ����������� ����� MouseLook.Update()
    {
        // ������� �������, ������� ��� ��������� MouseLook
        Quaternion mouseRot = transform.localRotation;

        // ���������
        trauma = Mathf.Max(0f, trauma - decay * Time.deltaTime);
        if (trauma <= 0f)
        {
            transform.localPosition = basePos;   // ������� ����������
            transform.localRotation = mouseRot;  // �� ������� ������� MouseLook
            return;
        }

        float t = trauma * trauma;               // ������ ������
        float time = Time.time * frequency;

        float px = (Mathf.PerlinNoise(time, 0f) * 2 - 1) * posAmplitude * t;
        float py = (Mathf.PerlinNoise(0f, time) * 2 - 1) * posAmplitude * t;
        float pz = (Mathf.PerlinNoise(time, time) * 2 - 1) * posAmplitude * t;

        float rx = (Mathf.PerlinNoise(time + 10f, 0f) * 2 - 1) * rotAmplitude * t;
        float ry = (Mathf.PerlinNoise(0f, time + 20f) * 2 - 1) * rotAmplitude * t;
        float rz = (Mathf.PerlinNoise(time + 30f, time + 40f) * 2 - 1) * rotAmplitude * t;

        // �������: ������� + ���; �������: ������� MouseLook * ���
        transform.localPosition = basePos + new Vector3(px, py, pz);
        transform.localRotation = mouseRot * Quaternion.Euler(rx, ry, rz);
    }

    public void Punch(float amount) { trauma = Mathf.Clamp01(trauma + amount); }
}
