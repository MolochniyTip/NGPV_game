using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.ProbeAdjustmentVolume;

public class Gun : MonoBehaviour
{
    [Header("Refs")]
    public Transform firePoint;          // ���� ������� ����
    public GameObject bulletPrefab;      // ���� ������ ���� (Bullet)
    public MuzzleFlash muzzle;           // ������� �� FirePoint (�������������)
    public AudioSource shotAudio;        // ���� (�������������)

    [Header("Gun Settings")]
    public float bulletForce = 20f;
    public float fireRate = 0.1f;        // �������� ����� ����������
    public float maxCharge = 100f;       // ������������ �����
    public float chargeUsePerShot = 5f;  // ������ �� �������
    public float reloadTime = 3f;        // ����� �����������

    [Header("Recoil (real)")]
    public MouseLook look;            // перетащи сюда компонент MouseLook с Main Camera
    public float recoilUp = 2f;       // подъём камеры за выстрел (°)
    public float recoilSide = 0.5f;   // горизонтальный разброс (°)


    [Header("UI")]
    public Slider gunChargeBar;          // ������� ������
    public GameObject reloadHint;        // ����� UI-������ "Reloading..." (����� �������� ������)

    [Header("Aim")]
    public Camera cam;                         // сюда перетащи Main Camera
    public LayerMask aimMask = ~0;             // маска для наведения (выключим Player)
    public float aimMaxDistance = 1000f;       // дальность луча

    [Header("Camera Shake")]
    public SimpleScreenShake shaker;   // перетащи Main Camera сюда
    public float shakePerShot = 0.25f; // сила тряски за выстрел

    [Header("Visual Recoil (kick)")]
    public Transform weaponVisual;       // ������ � ������� (��������, rifle)
    public float kickBack = 0.05f;       // �������� �� -Z
    public float kickUp = 2f;            // �������� ����� (� ��������)
    public float returnSpeed = 12f;      // �������� ��������

    float currentCharge;
    bool isReloading;
    float nextFireTime;
    Vector3 defaultPos;
    Quaternion defaultRot;
    Image fillImg;

    void Start()
    {
        currentCharge = maxCharge;

        if (gunChargeBar)
        {
            gunChargeBar.maxValue = maxCharge;
            gunChargeBar.value = currentCharge;
            if (gunChargeBar.fillRect) fillImg = gunChargeBar.fillRect.GetComponent<Image>();
        }

        if (weaponVisual)
        {
            defaultPos = weaponVisual.localPosition;
            defaultRot = weaponVisual.localRotation;
        }

        if (!muzzle && firePoint) muzzle = firePoint.GetComponent<MuzzleFlash>(); // ���������
        if (!cam) cam = Camera.main;
        if (!shaker && Camera.main) shaker = Camera.main.GetComponent<SimpleScreenShake>();
        if (!look && Camera.main) look = Camera.main.GetComponent<MouseLook>();
    }

    void Update()
    {
        // ��������� UI
        if (gunChargeBar) gunChargeBar.value = currentCharge;
        if (fillImg)
        {
            if (isReloading) fillImg.color = new Color(0.6f, 0.6f, 0.6f, 1f); // �����
            else if (currentCharge < chargeUsePerShot) fillImg.color = Color.red;               // �������
            else fillImg.color = Color.white;              // �����
        }
        if (reloadHint) reloadHint.SetActive(isReloading);

        // ������� ������ ����� ������
        if (weaponVisual)
        {
            weaponVisual.localPosition = Vector3.Lerp(weaponVisual.localPosition, defaultPos, returnSpeed * Time.deltaTime);
            weaponVisual.localRotation = Quaternion.Slerp(weaponVisual.localRotation, defaultRot, returnSpeed * Time.deltaTime);
        }

        if (isReloading) return;

        bool canShoot = Time.time >= nextFireTime && currentCharge >= chargeUsePerShot;

        if (Input.GetButton("Fire1") && canShoot)
        {
            Shoot();
            currentCharge = Mathf.Max(0f, currentCharge - chargeUsePerShot);
            nextFireTime = Time.time + fireRate;
        }

        // ����-����������� + ������ �� R
        if ((currentCharge < chargeUsePerShot && !isReloading) || (Input.GetKeyDown(KeyCode.R) && currentCharge < maxCharge))
            StartCoroutine(Reload());
    }

    void Shoot()
    {
        // 1) Луч из центра экрана: куда "смотрит" прицел
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 aimPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, aimMaxDistance, aimMask, QueryTriggerInteraction.Ignore))
            aimPoint = hit.point;
        else
            aimPoint = ray.origin + ray.direction * aimMaxDistance;

        // 2) Направление от дула к точке прицеливания
        Vector3 dir = (aimPoint - firePoint.position).normalized;

        // 3) Спавн и разгон пули по этому направлению
        GameObject go = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
        var rb = go.GetComponent<Rigidbody>();
        if (!rb) rb = go.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dir * bulletForce, ForceMode.Impulse);

        // 4) Визуалка
        if (weaponVisual)
        {
            weaponVisual.localPosition -= new Vector3(0f, 0f, kickBack);
            weaponVisual.localRotation *= Quaternion.Euler(-kickUp, 0f, 0f);
        }
        if (muzzle) muzzle.Play();
        if (shotAudio) shotAudio.Play();
        if (shaker) shaker.Punch(shakePerShot);
        // реальная отдача, которую игрок может контролировать
        if (look)
        {
            float side = Random.Range(-recoilSide, recoilSide);
            look.AddRecoil(recoilUp, side);
        }



        // Debug (можно оставить для проверки траектории)
        Debug.DrawLine(firePoint.position, aimPoint, Color.cyan, 0.25f);
    }


    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentCharge = maxCharge;
        isReloading = false;
    }
}