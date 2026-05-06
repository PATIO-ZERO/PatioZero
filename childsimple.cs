using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDamageTriggerSimple : MonoBehaviour
{
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;

    [Header("Escena de derrota")]
    public string defeatSceneName = "Derrota";   // Puedes poner Derrota o Derrota2

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            Player playerScript = collision.GetComponent<Player>();

            if (playerScript != null)
            {
                playerScript.TakeDamage(1);

                if (playerScript.GetCurrentLives() <= 0)
                {
                    // Cargar la escena seleccionada desde el inspector
                    SceneManager.LoadScene(defeatSceneName);
                }

                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
}
