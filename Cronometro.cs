using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Database;

public class Cronometro : MonoBehaviour
{
    [Header("⏱ Temporizador")]
    public float elapsedTime = 0f;
    public bool timerIsRunning = false;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public GameObject canvasUI;

    [Header("Recolección por botes")]
    public int metaBote1 = 5;
    public int metaBote2 = 5;
    public int metaBote3 = 5;

    private int bote1 = 0;
    private int bote2 = 0;
    private int bote3 = 0;

    [Header("Animación")]
    public Animator animadorBasura;

    [Header("Jugador")]
    public GameObject jugador;

    [Header("Escena derrota")]
    public bool usarDerrota1 = true;

    private bool nivelFinalizado = false;

    void Start()
    {
        timerIsRunning = false;
        nivelFinalizado = false;
        UpdateTimerDisplay(elapsedTime);

        if (animadorBasura != null)
            animadorBasura.gameObject.SetActive(false);
    }

    void Update()
    {
        if (timerIsRunning && !nivelFinalizado)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay(elapsedTime);
        }

        // ✅ SOLO GANA SI LOS 3 BOTES ESTÁN COMPLETOS
        if (BotesCompletos() && !nivelFinalizado)
        {
            FinDelNivel(true);
        }
    }

    // 🔥 MÉTODOS PARA SUMAR A CADA BOTE
    public void AgregarABote1()
    {
        if (bote1 < metaBote1)
            bote1++;
    }

    public void AgregarABote2()
    {
        if (bote2 < metaBote2)
            bote2++;
    }

    public void AgregarABote3()
    {
        if (bote3 < metaBote3)
            bote3++;
    }

    // 🔥 VERIFICACIÓN REAL
    bool BotesCompletos()
    {
        return (bote1 >= metaBote1 &&
                bote2 >= metaBote2 &&
                bote3 >= metaBote3);
    }

    // 🔥 PARA EL NPC
    public bool ObjetivoCumplido()
    {
        return BotesCompletos();
    }

    public void IniciarTemporizador()
    {
        if (!timerIsRunning)
        {
            timerIsRunning = true;
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void FinDelNivel(bool completado = false)
    {
        if (nivelFinalizado) return;
        nivelFinalizado = true;

        if (completado)
        {
            GuardarDatosFirebase();
            SceneManager.LoadScene("Victoria");
            return;
        }
        else
        {
            if (canvasUI != null)
                canvasUI.SetActive(false);

            if (jugador != null)
                jugador.SetActive(false);

            if (animadorBasura != null)
            {
                animadorBasura.gameObject.SetActive(true);
                animadorBasura.SetTrigger("Activar");

                Invoke("CambiarEscenaDerrota", 2f);
            }
            else
            {
                CambiarEscenaDerrota();
            }
        }
    }

    void CambiarEscenaDerrota()
    {
        if (usarDerrota1)
            SceneManager.LoadScene("Derrota");
        else
            SceneManager.LoadScene("Derrota2");
    }

    void GuardarDatosFirebase()
    {
        string userId = SystemInfo.deviceUniqueIdentifier;

        string nombre = PlayerPrefs.GetString("nombre", "Invitado");
        string correo = PlayerPrefs.GetString("correo", "sincorreo");

        float tiempoFinal = Mathf.Round(elapsedTime);

        DatabaseReference db = FirebaseDatabase.DefaultInstance.RootReference;

        Dictionary<string, object> datos = new Dictionary<string, object>()
        {
            { "nombre", nombre },
            { "correo", correo },
            { "tiempo", tiempoFinal }
        };

        db.Child("usuarios")
          .Child(userId)
          .SetValueAsync(datos);

        Debug.Log("Datos guardados en Firebase");
    }
}