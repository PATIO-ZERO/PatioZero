using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SALTO : MonoBehaviour
{
   public float fuerzaSalto = 1.0f;
   public float longitudRaycast = 1.0f;
   public LayerMask capaSuelo;

   private bool enSuelo;
    private Rigidbody2D rb;

    public Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        enSuelo = hit.collider != null;

        if (enSuelo && Input.GetKeyDown(KeyCode.Space))
        {
             rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D. Impulse);
        }

        animator.SetBool("enSuelo", enSuelo);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}

