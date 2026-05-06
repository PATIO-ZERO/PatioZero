using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("ATAQUE")]
    public float attackRangeX = 1.2f;   // Largo del ataque
    public float attackRangeY = 0.8f;   // Altura del ataque
    public float attackOffset = 0.8f;   // Distancia frente al jugador
    public int damage = 1;
    public float attackCooldown = 0.5f;

    [Header("REFERENCIAS")]
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip attackSound;

    private bool canAttack = true;
    private bool isAttacking = false;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        canAttack = false;

        // Activar animación
        animator.SetTrigger("Attack");

        // Sonido
        if (audioSource && attackSound)
            audioSource.PlayOneShot(attackSound);

        // Esperar a que coincida con el golpe visual
        yield return new WaitForSeconds(0.15f);

        // Definir la posición del ataque según la dirección
        float direction = playerTransform.localScale.x;
        Vector2 attackPos = new Vector2(
            playerTransform.position.x + attackOffset * direction,
            playerTransform.position.y
        );

        // Detectar enemigos solo EN FRENTE
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
            attackPos,
            new Vector2(attackRangeX, attackRangeY),
            0
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<EnemyFinal>()?.TakeDamage(damage);
                Debug.Log("Golpe frontal!");
            }
        }

        // Cooldown
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerTransform == null)
            playerTransform = transform;

        Gizmos.color = Color.red;

        float direction = playerTransform.localScale.x;

        Vector2 attackPos = new Vector2(
            playerTransform.position.x + attackOffset * direction,
            playerTransform.position.y
        );

        Gizmos.DrawWireCube(attackPos, new Vector3(attackRangeX, attackRangeY, 1));
    }
    public void AttackButton()
{
    if (canAttack)
        StartCoroutine(Attack());
}

}
