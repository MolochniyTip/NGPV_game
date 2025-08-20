using UnityEngine;

public class CameraBobStrafe : MonoBehaviour
{
    public PlayerMovement move;         // перетащи PlayerMovement с капсулы

    [Header("Walk")]
    public float walkFreq = 8f;    // частота шага
    public float walkSideAmp = 0.06f; // ВЛЕВО-ВПРАВО (главное движение)
    public float walkVertAmp = 0.015f;// ВВЕРХ-ВНИЗ (чуть-чуть)
    public float walkRoll = 1.2f;  // крен (°)

    [Header("Sprint")]
    public float sprintFreq = 11f;
    public float sprintSideAmp = 0.11f;
    public float sprintVertAmp = 0.03f;
    public float sprintRoll = 2.2f;

    public float smooth = 12f;   // сглаживание

    Vector3 basePos;
    Quaternion baseRot;
    float t; // фаза шага

    void Awake() { basePos = transform.localPosition; baseRot = transform.localRotation; }

    void LateUpdate()
    {
        // используем ввод, чтобы работать всегда
        float ax = Input.GetAxis("Horizontal");
        float az = Input.GetAxis("Vertical");
        float moveInput = Mathf.Clamp01(new Vector2(ax, az).magnitude);
        bool moving = moveInput > 0.01f;
        bool sprint = move && move.IsSprinting;

        float freq = sprint ? sprintFreq : walkFreq;
        float sideAmp = (sprint ? sprintSideAmp : walkSideAmp) * moveInput;
        float vertAmp = (sprint ? sprintVertAmp : walkVertAmp) * moveInput;
        float rollAmp = (sprint ? sprintRoll : walkRoll) * moveInput;

        if (moving) t += Time.deltaTime * freq;
        else t = Mathf.Lerp(t, 0f, smooth * Time.deltaTime);

        // боковая раскачка — косинус; вертикаль — слабее и в 2 раза чаще (двойной удар шага)
        float x = Mathf.Cos(t) * sideAmp;     // ← влево-вправо
        float y = Mathf.Sin(2f * t) * vertAmp;     // ← немного вверх-вниз
        float roll = Mathf.Sin(t) * rollAmp;     // ← крен (°)

        Vector3 targetPos = basePos + new Vector3(x, y, 0f);
        Quaternion targetRot = baseRot * Quaternion.Euler(0f, 0f, -roll);

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, smooth * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, smooth * Time.deltaTime);
    }
}
