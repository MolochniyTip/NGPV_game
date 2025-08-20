using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public Light flashLight;
    public float duration = 0.05f;
    float t;

    void Reset() { flashLight = GetComponentInChildren<Light>(true); }
    void OnEnable() { if (flashLight) flashLight.enabled = false; }

    public void Play()
    {
        if (!flashLight) return;
        t = duration;
        flashLight.enabled = true;
    }

    void Update()
    {
        if (!flashLight || t <= 0f) return;
        t -= Time.deltaTime;
        if (t <= 0f) flashLight.enabled = false;
    }
}
