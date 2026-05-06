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
    private bool isJumping;
    private bool isAttacking;

    [Header("Joystick")]
    public Joystick joystick;   // Movimiento Android

    [Header("Botones Móviles")]
    public bool jumpButtonPressed = false;
    public bool attackButtonPressed = false;

    [Header("Ataque")]
    public bool canAttack = false;
    public AudioClip attackSound;

    [Header("Componentes")]
    private Rigidbody2D rb;
    private Animator animator;
    public AudioSource audioSource;
    public LayerMask groundLayer;

    // 🔥 NUEVO (raycast)
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
        // ====================
        // MOVIMIENTO
        // ====================
        float joyInput = joystick ? joystick.Horizontal : 0f;
        float keyboardInput = Input.GetAxisRaw("Horizontal");
        moveInput = (joyInput != 0) ? joyInput : keyboardInput;

        if (!isAttacking)
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // ====================
        // SALTO
        // ====================
        if ((Input.GetButtonDown("Jump") || jumpButtonPressed) && isGrounded && !isAttacking)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            isJumping = true;
            animator.SetBool("isJumping", true);

            if (audioSource && jumpSound)
                audioSource.PlayOneShot(jumpSound);

            jumpButtonPressed = false;
        }

        // ====================
        // ATAQUE
        // ====================
    }

    void FixedUpdate()
    {
        Vector2 origin = transform.position;

        RaycastHit2D hitCenter = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin + Vector2.left * rayOffset, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(origin + Vector2.right * rayOffset, Vector2.down, groundCheckDistance, groundLayer);

        bool groundedNow = hitCenter.collider != null || hitLeft.collider != null || hitRight.collider != null;

        if (groundedNow && !isGrounded)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        isGrounded = groundedNow;
    }

    // ====================
    // ATAQUE
    // ====================
    void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        if (audioSource && attackSound)
            audioSource.PlayOneShot(attackSound);
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    // ====================
    // DAÑO + EMPUJÓN
    // ====================
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        float direction = transform.localScale.x > 0 ? -1 : 1;
        rb.velocity = new Vector2(direction * knockbackForce, rb.velocity.y);

        StartCoroutine(DamageEffect());

        if (currentLives <= 0)
            Die();
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

    void Die()
    {
        Debug.Log("El jugador ha muerto");
        gameObject.SetActive(false);
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    public void Heal(int amount)
    {
        currentLives += amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        Debug.Log("Curado. Vidas actuales: " + currentLives);
    }

    public void JumpButton()
    {
        if (isGrounded)
            jumpButtonPressed = true;
    }

    public void AttackButton()
    {
        attackButtonPressed = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 origin = transform.position;

        Gizmos.DrawLine(origin, origin + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(origin + Vector3.left * rayOffset, origin + Vector3.left * rayOffset + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(origin + Vector3.right * rayOffset, origin + Vector3.right * rayOffset + Vector3.down * groundCheckDistance);
    }
}