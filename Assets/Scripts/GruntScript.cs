using UnityEngine;

public class GruntScript : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject Inca;

    private float LastShoot;

    private int Health = 3;
    private void Update()
    {
        if (Inca == null) return;

        Vector3 originalScale = transform.localScale;


        Vector3 direction = Inca.transform.position - transform.position;
        if (direction.x >=  0.0f) transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);


        float distance = Mathf.Abs(Inca.transform.position.x - transform.position.x);

        if (distance < 2.0f && Time.time > LastShoot + 0.35f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x > 0.0f)
            direction = Vector3.right;
        else
            direction = Vector3.left;

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        bulletScript.SetDirection(direction);

        // Ignorar colisión entre la bala y el enemigo
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        // Destruir la bala automáticamente después de 1 segundo
        Destroy(bullet, 1f);
    }


    public void Hit()
    {
        Health -= 1;
        if (Health == 0) Destroy(gameObject);
    }
}
