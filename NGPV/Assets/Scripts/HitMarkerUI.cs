using UnityEngine;

public class HitMarkerUI : MonoBehaviour
{
    public static HitMarkerUI I { get; private set; }
    public CanvasGroup group;          // перетащи сюда CanvasGroup с этого же объекта
    public float showTime = 0.12f;     // сколько держать €рко
    public float fadeSpeed = 8f;       // скорость затухани€
    public float popScale = 1.25f;     // на сколько "выпрыгивает" при вспышке
    public float relaxSpeed = 10f;     // скорость возврата масштаба к 1

    RectTransform rt;
    float t;

    void Awake()
    {
        I = this;
        rt = GetComponent<RectTransform>();
        if (!group) group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
        rt.localScale = Vector3.one;
    }

    public void Ping()               // вызываем, когда попали во врага
    {
        t = showTime;
        group.alpha = 1f;
        rt.localScale = Vector3.one * popScale;
    }

    void Update()
    {
        // держим €рким пока t>0
        if (t > 0f) { t -= Time.deltaTime; }
        else
        {
            group.alpha = Mathf.MoveTowards(group.alpha, 0f, fadeSpeed * Time.deltaTime);
        }
        // м€гко возвращаем масштаб
        rt.localScale = Vector3.Lerp(rt.localScale, Vector3.one, relaxSpeed * Time.deltaTime);
    }
}
