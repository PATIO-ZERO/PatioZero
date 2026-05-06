using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menusistem : MonoBehaviour
{
   
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
     public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }


    public void Niveles()
    {
        Debug.Log("?? Diálogo final terminado. Cargando escena de victoria...");
        SceneManager.LoadScene("NivelSelector");
    }

    public void Nivelesvolver()
    {
        Debug.Log("?? Diálogo final terminado. Cargando escena de victoria...");
        SceneManager.LoadScene("Menu");
    }
}
