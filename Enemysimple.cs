using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class EnemyFinal : MonoBehaviour
{
    [Header("Movimiento / Patrulla")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 3f;
    private Transform currentTarget;

    [Header("Detección (solo a los lados)")]
    public Transform player;
    public float detectionRange = 8f;
    public float verticalDetectionLimit = 1.5f;

    [Header("Vida")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Flash de Daño (Hollow Knight BRILLO FUERTE)")]
    public float flashDuration = 0.1f;
    private SpriteRenderer sr;
    private MaterialPropertyBlock mpb;
    private Color originalColor;

    private Rigidbody2D rb;
    private bool facingRight = false;
    private bool chasingPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
        originalColor = sr.color;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        currentHealth = maxHealth;

        // Inicia mirando a la izquierda
        facingRight = false;
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;

        currentTarget = pointA;

        // Buscar player
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // Ignorar colisión
        if (player != null)
        {
            Collider2D enemyCol = GetComponent<Collider2D>();
            Collider2D playerCol = player.GetComponent<Collider2D>();
            if (playerCol != null)
                Physics2D.IgnoreCollision(enemyCol, playerCol);
        }
    }

    void FixedUpdate()
    {
        if (player == null || currentHealth <= 0)
            return;

        DetectPlayer();

        if (chasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    // ---------------------
    // DETECTAR SOLO LATERAL
    // ---------------------
    void DetectPlayer()
    {
        float distX = Mathf.Abs(player.position.x - transform.position.x);
        float distY = Mathf.Abs(player.position.y - transform.position.y);

        if (distX <= detectionRange && distY <= verticalDetectionLimit)
            chasingPlayer = true;
        else
            chasingPlayer = false;
    }

    // ---------------------
    // PERSEGUIR AL PLAYER
    // ---------------------
    void ChasePlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        if (direction > 0 && !facingRight) Flip();
        else if (direction < 0 && facingRight) Flip();
    }

    // ---------------------
    // PATRULLA
    // ---------------------
    void Patrol()
    {
        if (pointA == null || pointB == null)
            return;

        float distance = Vector2.Distance(transform.position, currentTarget.position);

        if (distance < 0.5f)
            currentTarget = (currentTarget == pointA) ? pointB : pointA;

        float direction = Mathf.Sign(currentTarget.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        if (direction > 0 && !facingRight) Flip();
        else if (direction < 0 && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ---------------------
    // DAÑO + FLASH
    // ---------------------
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashHK());

        if (currentHealth <= 0)
            Die();
    }

    // FLASH TIPO HOLLOW KNIGHT BRILLANTE
    IEnumerator FlashHK()
    {
        sr.GetPropertyBlock(mpb);

        // brillo fuerte (si quieres más, sube el 3f a 5f o 8f)
        mpb.SetColor("_Color", new Color(3f, 3f, 3f, 1f));
        sr.SetPropertyBlock(mpb);

        yield return new WaitForSeconds(flashDuration);

        mpb.SetColor("_Color", originalColor);
        sr.SetPropertyBlock(mpb);
    }

    private void Die()
    {
        Destroy(gameObject);
        Debug.Log("¡Enemigo eliminado!");
    }
}
