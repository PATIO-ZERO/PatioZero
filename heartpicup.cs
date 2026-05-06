using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public int healAmount = 1;       // Cuántas vidas recupera
    public AudioClip pickupSound;    // Clip de sonido al recoger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.Heal(healAmount);  // CURA
            }

            // Reproducir sonido al instante
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Destruir el objeto al instante
            Destroy(gameObject);
        }
    }
}

