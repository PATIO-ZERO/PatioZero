using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;          // Asignar en inspector o buscar automáticamente
    public float detectionRange = 8f;
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Buscar jugador automáticamente si no está asignado
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;

                // Ignorar colisión física entre enemigo y jugador
                Collider2D enemyCol = GetComponent<Collider2D>();
                Collider2D playerCol = playerObj.GetComponent<Collider2D>();
                if (playerCol != null)
                    Physics2D.IgnoreCollision(enemyCol, playerCol);
            }
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Mathf.Abs(player.position.x - transform.position.x);

        if (distance <= detectionRange)
        {
            // Movimiento solo horizontal
            float directionX = Mathf.Sign(player.position.x - transform.position.x);
            Vector2 targetPos = rb.position + new Vector2(directionX * moveSpeed * Time.fixedDeltaTime, 0);
            rb.MovePosition(targetPos);

            // Voltear sprite
            if (directionX > 0 && !facingRight) Flip();
            else if (directionX < 0 && facingRight) Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
