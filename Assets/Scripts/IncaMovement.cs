using UnityEngine;

public class IncaMovement : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float Speed = 5f;
    public float JumpForce = 10f;
    public LayerMask groundLayer;
    public float ShootInterval = 0.1f; // Intervalo entre disparos, ajustado para más rapidez

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    private float LastShootTime; // Control de tiempo para disparar
    private int Health = 5;

    // Radio para la detección del suelo
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento horizontal
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Guarda el tamaño original del sprite
        Vector3 originalScale = transform.localScale;

        // Si camina hacia la izquierda, invierte el signo del eje X
        if (Horizontal > 0.0f)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        // Si camina hacia la derecha, mantén el signo positivo en el eje X
        else if (Horizontal < 0.0f)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // Actualiza el parámetro del Animator
        Animator.SetBool("Running", Horizontal != 0.0f);

        // Detectar Suelo con un OverlapCircle
        Grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }

        // Disparar con mayor frecuencia
        if (Input.GetKey(KeyCode.Space) && Time.time > LastShootTime + ShootInterval)
        {
            Shoot();
            LastShootTime = Time.time; // Actualizar tiempo del último disparo
        }
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal
        Rigidbody2D.linearVelocity = new Vector2(Horizontal * Speed, Rigidbody2D.linearVelocity.y);
    }

    private void Jump()
    {
        // Aplica la fuerza de salto y resetea la velocidad vertical
        Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, 0);
        Rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x > 0.0f)
            direction = Vector2.left;
        else
            direction = Vector2.right;

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        bulletScript.SetDirection(direction);

        // Ignorar colisión entre la bala y el personaje
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        // Destruir la bala automáticamente después de 1 segundo
        Destroy(bullet, 1f);
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public void Hit()
    {
        Health -= 1;
        if (Health == 0) Destroy(gameObject);
    }
}

