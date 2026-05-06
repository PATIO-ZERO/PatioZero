using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            Player playerScript = collision.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1);  // Solo 1 daño por cooldown
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
}
