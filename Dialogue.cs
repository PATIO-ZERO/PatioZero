using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Mensaje al entrar en rango (PC)")]
    [SerializeField] private GameObject mensajePanel;
    [SerializeField] private TMP_Text mensajeTexto;
    [SerializeField, TextArea(2, 3)] private string mensajeAlEntrar = "Presiona E para hablar.";

    [Header("Botón de interacción (Android)")]
    public GameObject interactButton;

    [Header("Configuración de Diálogo (Auto-Avance)")]
    [SerializeField] private float typingTime = 0.01f;
    [SerializeField, Tooltip("Tiempo de espera para leer antes de pasar a la siguiente línea")]
    private float timeToRead = 2.0f;

    [Header("Diálogos Español")]
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    [SerializeField, TextArea(4, 6)] private string[] finalDialogueLines;

    [Header("Diálogos Inglés")]
    [SerializeField, TextArea(4, 6)] private string[] dialogueLinesEnglish;
    [SerializeField, TextArea(4, 6)] private string[] finalDialogueLinesEnglish;

    private bool isPlayerInRange;
    private bool didDialogueStart;
    private int lineIndex;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hablaClip;

    private PlayerInventory playerInventory;
    private Player playerMovement;
    [Header("Referencia Cronómetro")]
public Cronometro cronometro;

private bool primerDialogoYaEjecutado = false;
private bool dialogoFinalDisponible = false;
private bool dialogoFinalYaMostrado = false;

    void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerMovement = FindObjectOfType<Player>();

        if (mensajePanel) mensajePanel.SetActive(false);
        if (dialoguePanel) dialoguePanel.SetActive(false);
        if (interactButton) interactButton.SetActive(false);
    }

    void Update()
    {
        // Interacción en PC
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            EjecutarInteraccion();
        }
    }

    // Llamado desde el botón en Android
    public void OnInteractButton()
    {
        if (isPlayerInRange)
            EjecutarInteraccion();
    }

    private void EjecutarInteraccion()
    {
        // Si el diálogo no ha empezado, lo iniciamos
        if (!didDialogueStart)
        {
            StartDialogue();
        }
        // Si el diálogo ya empezó y el jugador presiona el botón/tecla MIENTRAS se escribe (PC)
        else if (dialogueText.text != GetCurrentDialogue()[lineIndex])
        {
            // Autocompleta el texto inmediatamente y espera el tiempo de lectura
            StopAllCoroutines();
            dialogueText.text = GetCurrentDialogue()[lineIndex];
            StartCoroutine(WaitAndNextLine());
        }
    }
    private void StartDialogueAutomatico()
    {
        if (!didDialogueStart)
        {
          didDialogueStart = true;
          dialoguePanel.SetActive(true);
          dialogueMark.SetActive(false);

          if (mensajePanel) mensajePanel.SetActive(false);
          if (interactButton) interactButton.SetActive(false);

        // Bloquear movimiento
            if   (playerMovement)
              playerMovement.enabled = false;

            lineIndex = 0;

         ReproducirSonido();
         StartCoroutine(ShowLine());
        }
    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        dialogueMark.SetActive(false);

        if (mensajePanel) mensajePanel.SetActive(false);

        // Ocultamos el botón de interacción porque ahora avanzará solo
        if (interactButton) interactButton.SetActive(false);

        // Bloquea movimiento
        if (playerMovement)
            playerMovement.enabled = false;

        lineIndex = 0;
        ReproducirSonido();
        StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        lineIndex++;

        if (lineIndex < GetCurrentDialogue().Length)
        {
            ReproducirSonido();
            StartCoroutine(ShowLine());
        }
        else
        {
            // Finaliza la secuencia de diálogos
            dialoguePanel.SetActive(false);
            didDialogueStart = false;
            dialogueMark.SetActive(true);

            if (playerMovement)
                playerMovement.enabled = true;

            if (isPlayerInRange && mensajePanel)
                mensajePanel.SetActive(true);

            // Reactivamos el botón de Android al terminar, por si quieren volver a hablar
            if (isPlayerInRange && interactButton)
                interactButton.SetActive(true);

            if (UsarFinalDialogue())
                FindObjectOfType<IntroCinematic>()?.FinalizarDialogo();
        }
    }

    // Corrutina principal de escritura y auto-avance
    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        // Escribimos letra por letra
        foreach (char ch in GetCurrentDialogue()[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }

        // Una vez que termina de escribir, espera 'timeToRead' segundos
        yield return new WaitForSeconds(timeToRead);

        // Avanza a la siguiente línea de forma automática sin pedir clics
        NextDialogueLine();
    }

    // Corrutina secundaria en caso de que el jugador en PC decida saltarse la animación de escritura
    private IEnumerator WaitAndNextLine()
    {
        yield return new WaitForSeconds(timeToRead);
        NextDialogueLine();
    }

    private void ReproducirSonido()
    {
        if (audioSource && hablaClip)
            audioSource.PlayOneShot(hablaClip);
    }

    private string[] GetCurrentDialogue()
    {
        if (LanguageManager.Instance != null &&
            LanguageManager.Instance.currentLanguage == LanguageManager.Language.English)
        {
            return UsarFinalDialogue() ? finalDialogueLinesEnglish : dialogueLinesEnglish;
        }
        else
        {
            return UsarFinalDialogue() ? finalDialogueLines : dialogueLines;
        }
    }

private bool UsarFinalDialogue()
{
    return dialogoFinalDisponible;
}

private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        isPlayerInRange = true;
        dialogueMark.SetActive(true);

        // 🔥 PRIMERA VEZ → diálogo automático normal
        if (!primerDialogoYaEjecutado)
        {
            primerDialogoYaEjecutado = true;
            StartDialogueAutomatico();
            return;
        }

        // 🔥 SI YA TERMINÓ TODO → activar diálogo final
        if (cronometro != null && cronometro.ObjetivoCumplido() && !dialogoFinalYaMostrado)
        {
            dialogoFinalDisponible = true;
            dialogoFinalYaMostrado = true;
            StartDialogueAutomatico();
            return;
        }

        // Mostrar UI normal si no aplica automático
        if (!didDialogueStart && mensajePanel)
            mensajePanel.SetActive(true);

        if (!didDialogueStart && interactButton)
            interactButton.SetActive(true);
    }
}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;

            dialogueMark.SetActive(false);
            if (mensajePanel) mensajePanel.SetActive(false);
            if (interactButton) interactButton.SetActive(false);
        }
    }
}