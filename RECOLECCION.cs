using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    private string basuraActual = "";
    private int cantidadActual = 0;

    public int organicaTotal = 0;
    public int vidrioTotal = 0;
    public int plasticoTotal = 0;
    public int unicelTotal = 0; 

    public int organicaGoal = 5;
    public int vidrioGoal = 5;
    public int plasticoGoal = 5;
    public int unicelGoal = 5; 

    public TextMeshProUGUI organicaText;
    public TextMeshProUGUI vidrioText;
    public TextMeshProUGUI plasticoText;
    public TextMeshProUGUI unicelText; 

    public AudioSource audioSource;
    public AudioClip organicaClip;
    public AudioClip entregaClip;

    public Cronometro cronometro;

    [Header("Opciones de Cinem�tica")]
    public bool saltarCinematica = false; // Decide desde Inspector si saltar cinem�tica
    public string escenaVictoria = "Victoria"; // Nombre de la escena de victoria

    void Start()
    {
        if (cronometro == null)
            cronometro = FindObjectOfType<Cronometro>();

        ActualizarUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // RECOGER BASURA
        if (other.CompareTag("Organica") || other.CompareTag("Vidrio") ||
            other.CompareTag("Plastico") || other.CompareTag("Unicel"))
        {
            string tipo = other.tag;

            if (basuraActual == "" || basuraActual == tipo)
            {
                basuraActual = tipo;
                cantidadActual++;
                Destroy(other.gameObject);

                if (cronometro != null && !cronometro.timerIsRunning)
                    cronometro.IniciarTemporizador();

                if (audioSource != null && organicaClip != null)
                    audioSource.PlayOneShot(organicaClip);
            }
        }

        // ENTREGAR EN LOS BOTES
        else if (other.CompareTag("BoteOrganica") && basuraActual == "Organica")
{
    organicaTotal += cantidadActual;
    EntregarObjetos("Organica");
    PlayEntregaSound();
}
else if (other.CompareTag("BoteVidrio") && basuraActual == "Vidrio")
{
    vidrioTotal += cantidadActual;
    EntregarObjetos("Vidrio");
    PlayEntregaSound();
}
else if (other.CompareTag("BotePlastico") && basuraActual == "Plastico")
{
    plasticoTotal += cantidadActual;
    EntregarObjetos("Plastico");
    PlayEntregaSound();
}
else if (other.CompareTag("BoteUnicel") && basuraActual == "Unicel")
{
    unicelTotal += cantidadActual;
    EntregarObjetos("Unicel");
    PlayEntregaSound();
}
    }

private void EntregarObjetos(string tipoBote)
{
    if (cronometro != null)
    {
        for (int i = 0; i < cantidadActual; i++)
        {
            switch (tipoBote)
            {
                case "Organica":
                    cronometro.AgregarABote1();
                    break;

                case "Vidrio":
                    cronometro.AgregarABote2();
                    break;

                case "Plastico":
                    cronometro.AgregarABote3();
                    break;

                case "Unicel":
                    // Si quieres 4to bote, luego lo agregamos
                    break;
            }
        }
    }

    VaciarInventario();
    ActualizarUI();
}

    private void VaciarInventario()
    {
        basuraActual = "";
        cantidadActual = 0;
    }

    private void ActualizarUI()
    {
        if (organicaText != null)
            organicaText.text = $"{organicaTotal}/{organicaGoal}";
        if (vidrioText != null)
            vidrioText.text = $"{vidrioTotal}/{vidrioGoal}";
        if (plasticoText != null)
            plasticoText.text = $"{plasticoTotal}/{plasticoGoal}";
        if (unicelText != null)
            unicelText.text = $"{unicelTotal}/{unicelGoal}";
    }

    private void PlayEntregaSound()
    {
        if (audioSource != null && entregaClip != null)
            audioSource.PlayOneShot(entregaClip);
    }
}
