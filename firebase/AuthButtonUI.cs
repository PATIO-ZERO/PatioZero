using UnityEngine;
using Firebase.Auth;
using TMPro;

public class AuthButtonUI : MonoBehaviour
{
    public GameObject panelAuth;        // Panel de login/registro
    public TextMeshProUGUI txtMensaje; // 🔥 TextMeshPro

    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // Asegurarse que el panel esté oculto al inicio
        panelAuth.SetActive(false);
    }

    public void OnAuthButtonPressed()
    {
        if (auth.CurrentUser != null)
        {
            // 🔐 YA está logueado
            string email = auth.CurrentUser.Email;

            txtMensaje.text = "Ya estás registrado con: " + email;

            panelAuth.SetActive(false);

            Debug.Log("Usuario activo: " + email);
        }
        else
        {
            // 🚀 NO está logueado → abrir menú de registro/login
            panelAuth.SetActive(true);
            txtMensaje.text = "";
        }
    }
    public void CerrarPanelAuth()
    {
        if (panelAuth != null)
            panelAuth.SetActive(false);

        txtMensaje.text = "";

    }
}