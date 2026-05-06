using UnityEngine;

public class HeartDisplay : MonoBehaviour
{
    public GameObject[] hearts;   // íconos en el canvas
    public Player playerHealth;   // referencia al Player

    void Update()
    {
        int currentLives = playerHealth.GetCurrentLives();

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentLives);
        }
    }
}
