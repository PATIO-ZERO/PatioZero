using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 7f;
    private float moveInput;
    private bool isGrounded;

    [Header("Joystick")]
    public Joystick joystick;

    [Header("Botones Móviles")]
    public bool jumpButtonPressed = false;
    public bool attackButtonPressed = false;

    [Header("Ataque")]
    public bool canAttack = true;
    public AudioClip attackSound;

    [Header("Componentes")]
    private Rigidbody2D rb;
    private Animator animator;
    public AudioSource audioSource;
    public LayerMask groundLayer;

    public float groundCheckDistance = 0.8f;
    public float rayOffset = 0.3f;

    [Header("Sonidos")]
    public AudioClip jumpSound;

    [Header("Vidas")]
    public int maxLives = 3;
    private int currentLives;

    [Header("Daño / Invencibilidad")]
    public float invincibleTime = 1f;
    public float knockbackForce = 8f;
    private bool isInvincible = false;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        currentLives = maxLives;
    }

    void Update()
    {
       
        float joyInput = joystick ? joystick.Horizontal : 0f;
        float keyboardInput = Input.GetAxisRaw("Horizontal");
        moveInput = (joyInput != 0) ? joyInput : keyboardInput;

        
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        
        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);

        
        if ((Input.GetButtonDown("Jump") || jumpButtonPressed) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (audioSource && jumpSound) audioSource.PlayOneShot(jumpSound);
            jumpButtonPressed = false;
        }

        
        if ((Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1") || attackButtonPressed) && canAttack)
        {
            EjecutarGolpe();
            attackButtonPressed = false;
        }

        
        if (animator != null)
        {
            animator.SetFloat("Horizontal", Mathf.Abs(moveInput));
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("vSpeed", rb.velocity.y);
        }
    }

    void FixedUpdate()
    {
        
        Vector2 origin = transform.position;

      
        RaycastHit2D hitCenter = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin + Vector2.left * rayOffset, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(origin + Vector2.right * rayOffset, Vector2.down, groundCheckDistance, groundLayer);

        
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, Color.red);
        Debug.DrawRay(origin + Vector2.left * rayOffset, Vector2.down * groundCheckDistance, Color.red);
        Debug.DrawRay(origin + Vector2.right * rayOffset, Vector2.down * groundCheckDistance, Color.red);

        isGrounded = hitCenter.collider != null || hitLeft.collider != null || hitRight.collider != null;
    }

    void EjecutarGolpe()
    {
        animator.SetTrigger("golpe");
        if (audioSource && attackSound) audioSource.PlayOneShot(attackSound);
    }

    // ==========================================
    // FUNCIONES RECUPERADAS (PARA QUITAR ERRORES)
    // ==========================================

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        float direction = transform.localScale.x > 0 ? -1 : 1;
        rb.velocity = new Vector2(direction * knockbackForce, rb.velocity.y);
        StartCoroutine(DamageEffect());
        if (currentLives <= 0) Die();
    }

    public void Heal(int amount) // Requerida por heartpicup.cs
    {
        currentLives += amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
    }

    public int GetCurrentLives() // Requerida por HEARTpanel.cs
    {
        return currentLives;
    }

    private IEnumerator DamageEffect()
    {
        isInvincible = true;
        float elapsed = 0f;
        while (elapsed < invincibleTime)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }
        sr.enabled = true;
        isInvincible = false;
    }

    void Die() { gameObject.SetActive(false); }

    public void JumpButton() { if (isGrounded) jumpButtonPressed = true; }
    public void AttackButton() { attackButtonPressed = true; }
}