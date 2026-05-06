using UnityEngine;
using UnityEngine.SceneManagement;
// Elimina la línea duplicada y errónea:
// using UnityEngine.SceneManager;

public class LevelSelector : MonoBehaviour
{
    [Header("Referencia a los puntos de nivel")]
    public RectTransform[] levelPoints;  // botones
    public RectTransform selector;       // tu flecha

    [Header("Configuración")]
    public float moveSpeed = 10f;
    public float offsetY = 50f;  // distancia hacia ARRIBA del botón

    private int currentIndex = 0;

    void Start()
    {
        if (selector != null && levelPoints.Length > 0)
        {
            SetSelectorPositionInstant(0);
        }
    }

    void Update()
    {
        // IZQUIERDA (teclado)
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLeft();

        // DERECHA (teclado)
        if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveRight();

        // Posición del selector
        Vector3 targetPos = levelPoints[currentIndex].position;
        targetPos.y += offsetY;

        selector.position = Vector3.Lerp(
            selector.position,
            targetPos,
            Time.deltaTime * moveSpeed
        );

        // ENTRAR (teclado)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            LoadLevel(currentIndex);
    }

    // ===========================
    // BOTONES PARA MÓVIL
    // ===========================
    public void MoveLeft()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = levelPoints.Length - 1;
    }

    public void MoveRight()
    {
        currentIndex++;
        if (currentIndex >= levelPoints.Length) currentIndex = 0;
    }

    // ENTRAR NIVEL EN MÓVIL
    public void EnterLevel()
    {
        LoadLevel(currentIndex);
    }
    // ===========================

    void SetSelectorPositionInstant(int index)
    {
        Vector3 pos = levelPoints[index].position;
        pos.y += offsetY;
        selector.position = pos;
    }

    void LoadLevel(int index)
    {
        switch (index)
        {
            case 0: SceneManager.LoadScene("SampleScene"); break;
            case 1: SceneManager.LoadScene("cinematica2"); break;
            case 2: SceneManager.LoadScene("cinematica2"); break;
        }
    }
}

