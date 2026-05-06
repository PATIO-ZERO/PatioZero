using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    public float velocidad = 5f;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        // Mover al personaje
        rb.velocity = new Vector2(horizontal * velocidad, rb.velocity.y);

        // Actualizar parámetro del Animator
        animator.SetFloat("Horizontal", horizontal);

        // Voltear el sprite según la dirección
        if (horizontal > 0)
            transform.localScale = new Vector3(1, 1, 1); // Derecha
        else if (horizontal < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Izquierda
    }
}