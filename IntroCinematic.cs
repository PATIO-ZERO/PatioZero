using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class IntroCinematic : MonoBehaviour
{
    public Transform startPoint;
    public Transform npcPoint;
    public float speed = 3f;

    private GameObject player;
    private Player playerController;
    private bool moving = false;

    public Tilemap platformTilemap;
    private TilemapRenderer tilemapRenderer;
    private TilemapCollider2D tilemapCollider;

    private bool primeraCinematicaTerminada = false;
    private bool segundaCinematicaActiva = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (platformTilemap != null)
        {
            tilemapRenderer = platformTilemap.GetComponent<TilemapRenderer>();
            tilemapCollider = platformTilemap.GetComponent<TilemapCollider2D>();

            if (tilemapRenderer != null)
                tilemapRenderer.enabled = false;

            if (tilemapCollider != null)
                tilemapCollider.enabled = false;
        }

        if (player != null && startPoint != null)
        {
            player.transform.position = startPoint.position;

            playerController = player.GetComponent<Player>();
            if (playerController != null)
                playerController.enabled = false;

            moving = true; // Primera cinemática
        }
    }

    void Update()
    {
        if (moving && player != null)
        {
            // Movimiento hacia el NPC
            player.transform.position = Vector2.MoveTowards(
                player.transform.position,
                npcPoint.position,
                speed * Time.deltaTime
            );

            // ?? Forzar siempre mirar hacia la derecha
            Vector3 scale = player.transform.localScale;
            scale.x = 1f;
            player.transform.localScale = scale;

            // Comprobar si llegó al destino
            if (Vector2.Distance(player.transform.position, npcPoint.position) < 0.05f)
            {
                moving = false;

                if (playerController != null)
                    playerController.enabled = true;

                if (!primeraCinematicaTerminada)
                {
                    // Activar plataformas
                    if (tilemapRenderer != null)
                        tilemapRenderer.enabled = true;
                    if (tilemapCollider != null)
                        tilemapCollider.enabled = true;

                    primeraCinematicaTerminada = true;
                }
                else if (segundaCinematicaActiva)
                {
                    segundaCinematicaActiva = false;
                }
            }
        }
    }

    public void ActivarCinematica()
    {
        if (playerController != null)
            playerController.enabled = false;

        moving = true;
        segundaCinematicaActiva = true;

        // Desactivar plataformas durante la segunda cinemática
        if (tilemapRenderer != null)
            tilemapRenderer.enabled = false;
        if (tilemapCollider != null)
            tilemapCollider.enabled = false;

        // Detener el temporizador si existe
        Cronometro cronometro = FindObjectOfType<Cronometro>();
        if (cronometro != null)
        {
            cronometro.timerIsRunning = false;
            Debug.Log("?? Temporizador detenido para la cinemática final");
        }
    }

    public void FinalizarDialogo()
    {
        Debug.Log("?? Diálogo final terminado. Cargando escena de victoria...");
        SceneManager.LoadScene("Victoria");
    }
}
