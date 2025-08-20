using UnityEngine;

public class HitMarkerUI : MonoBehaviour
{
    public static HitMarkerUI I { get; private set; }
    public CanvasGroup group;          // �������� ���� CanvasGroup � ����� �� �������
    public float showTime = 0.12f;     // ������� ������� ����
    public float fadeSpeed = 8f;       // �������� ���������
    public float popScale = 1.25f;     // �� ������� "�����������" ��� �������
    public float relaxSpeed = 10f;     // �������� �������� �������� � 1

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

    public void Ping()               // ��������, ����� ������ �� �����
    {
        t = showTime;
        group.alpha = 1f;
        rt.localScale = Vector3.one * popScale;
    }

    void Update()
    {
        // ������ ����� ���� t>0
        if (t > 0f) { t -= Time.deltaTime; }
        else
        {
            group.alpha = Mathf.MoveTowards(group.alpha, 0f, fadeSpeed * Time.deltaTime);
        }
        // ����� ���������� �������
        rt.localScale = Vector3.Lerp(rt.localScale, Vector3.one, relaxSpeed * Time.deltaTime);
    }
}
