using UnityEngine;
using UnityEngine.UI;

public class CrosshairAim : MonoBehaviour
{
    public Camera cam;                       // перетащи Main Camera
    public LayerMask aimMask = ~0;           // маска, исключим Player/UI
    public Image crosshair;                  // можно оставить пустым Ч возьмЄтс€ с этого объекта
    public Color normalColor = Color.white;
    public Color enemyColor = Color.red;
    public float normalSize = 10f;
    public float enemySize = 14f;
    public float lerpSpeed = 12f;

    RectTransform rt;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        if (!crosshair) crosshair = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        bool onEnemy = false;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out var hit, 1000f, aimMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.GetComponentInParent<Enemy>() != null)
                onEnemy = true;
        }

        // плавно мен€ем цвет/размер
        var targetColor = onEnemy ? enemyColor : normalColor;
        var targetSize = onEnemy ? enemySize : normalSize;
        if (crosshair) crosshair.color = Color.Lerp(crosshair.color, targetColor, Time.deltaTime * lerpSpeed);
        if (rt) rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, new Vector2(targetSize, targetSize), Time.deltaTime * lerpSpeed);
    }
}
