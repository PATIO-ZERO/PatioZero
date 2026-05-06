using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject pauseMenuPanel;

    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("Juego pausado y panel activado");
        }
        else
        {
            Debug.LogWarning("No se asignó el panel al script.");
        }
    }
}
