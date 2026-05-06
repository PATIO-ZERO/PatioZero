using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{
    public enum EscenaDerrota
    {
        Derrota,
        Derrota2
    }

    [Header("?? Selección de pantalla de derrota")]
    public EscenaDerrota escenaDerrota = EscenaDerrota.Derrota;

    [Header("?? Configuración de vidas")]
    public int maxLives = 3;
    private int currentLives;

    [Header("?? UI - Corazones")]
    public Image[] hearts;

    [Header("?? Daño continuo")]
    public float damageInterval = 1f;

    [Header("?? Sonido de daño")]
    public AudioClip damageClip;
    private AudioSource audioSource;

    private bool isTouchingDamage = false;
    private bool isInvulnerable = false;

    void Start()
    {
        currentLives = maxLives;
        UpdateHeartsUI();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HazDano>() != null)
        {
            isTouchingDamage = true;
            StartCoroutine(DamageOverTime());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<HazDano>() != null)
        {
            isTouchingDamage = false;
        }
    }

    System.Collections.IEnumerator DamageOverTime()
    {
        while (isTouchingDamage && currentLives > 0)
        {
            TakeDamage(1);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        UpdateHeartsUI();

        if (damageClip != null)
            audioSource.PlayOneShot(damageClip);

        if (currentLives <= 0)
        {
            IrAEscenaDeDerrota();
        }
        else
        {
            StartCoroutine(InvulnerabilityCooldown());
        }
    }

    void IrAEscenaDeDerrota()
    {
        switch (escenaDerrota)
        {
            case EscenaDerrota.Derrota:
                SceneManager.LoadScene("Derrota");
                break;

            case EscenaDerrota.Derrota2:
                SceneManager.LoadScene("Derrota2");
                break;
        }
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = (i < currentLives);
        }
    }

    System.Collections.IEnumerator InvulnerabilityCooldown()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(0.3f);
        isInvulnerable = false;
    }
}
