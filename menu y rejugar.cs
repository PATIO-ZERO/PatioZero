using UnityEngine;
using UnityEngine.SceneManagement;


public class menuyrejugar : MonoBehaviour
{
    // -------------------------
    // REINICIAR / MENU NIVEL 1
    // -------------------------

    public void ReplayNivel1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void MenuDesdeNivel1()
    {
        SceneManager.LoadScene("Menu");
    }

    // -------------------------
    // REINICIAR / MENU NIVEL 2
    // -------------------------

    public void ReplayNivel2()
    {
        SceneManager.LoadScene("level 3");
    }

    
}
