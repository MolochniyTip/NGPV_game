using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;

    public float maxOxygen = 100f;
    public float currentOxygen;
    public Slider oxygenBar;

    void Start()
    {
        currentHealth = maxHealth;
        currentOxygen = maxOxygen;
        healthBar.maxValue = maxHealth;
        oxygenBar.maxValue = maxOxygen;
    }

    void Update()
    {
        healthBar.value = currentHealth;
        oxygenBar.value = currentOxygen;

        // ������ ���������
        currentOxygen -= Time.deltaTime * 2f;
        if (currentOxygen <= 0)
        {
            TakeDamage(Time.deltaTime * 5f); // ���� �� �������� ���������
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("����� ����");
    }
}
