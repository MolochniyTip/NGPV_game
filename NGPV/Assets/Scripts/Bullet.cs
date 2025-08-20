using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                if (HitMarkerUI.I) HitMarkerUI.I.Ping();
            }
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player")) // чтобы пуля не уничтожалась при столкновении с игроком
        {
            Destroy(gameObject);
        }
    }
}
    