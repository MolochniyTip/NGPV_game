using UnityEngine;

public class SimpleScreenShake : MonoBehaviour
{
    [Header("Shake")]
    public float posAmplitude = 0.05f;   // метры
    public float rotAmplitude = 1.5f;    // градусы
    public float decay = 3f;             // скорость затухани€
    public float frequency = 25f;        // √ц

    float trauma;                        // 0..1
    Vector3 basePos;                     // базова€ локальна€ позици€ (без шейка)

    void Awake() { basePos = transform.localPosition; }

    void LateUpdate() // выполн€етс€ ѕќ—Ћ≈ MouseLook.Update()
    {
        // текущий поворот, который уже установил MouseLook
        Quaternion mouseRot = transform.localRotation;

        // затухание
        trauma = Mathf.Max(0f, trauma - decay * Time.deltaTime);
        if (trauma <= 0f)
        {
            transform.localPosition = basePos;   // позицию возвращаем
            transform.localRotation = mouseRot;  // не трогаем поворот MouseLook
            return;
        }

        float t = trauma * trauma;               // м€гка€ крива€
        float time = Time.time * frequency;

        float px = (Mathf.PerlinNoise(time, 0f) * 2 - 1) * posAmplitude * t;
        float py = (Mathf.PerlinNoise(0f, time) * 2 - 1) * posAmplitude * t;
        float pz = (Mathf.PerlinNoise(time, time) * 2 - 1) * posAmplitude * t;

        float rx = (Mathf.PerlinNoise(time + 10f, 0f) * 2 - 1) * rotAmplitude * t;
        float ry = (Mathf.PerlinNoise(0f, time + 20f) * 2 - 1) * rotAmplitude * t;
        float rz = (Mathf.PerlinNoise(time + 30f, time + 40f) * 2 - 1) * rotAmplitude * t;

        // позици€: базова€ + шум; поворот: текущий MouseLook * шум
        transform.localPosition = basePos + new Vector3(px, py, pz);
        transform.localRotation = mouseRot * Quaternion.Euler(rx, ry, rz);
    }

    public void Punch(float amount) { trauma = Mathf.Clamp01(trauma + amount); }
}
