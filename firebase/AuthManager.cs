using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class AuthManager : MonoBehaviour
{
    public TMP_InputField inputEmail;
    public TMP_InputField inputPassword;
    public TMP_InputField inputNombre;

    public TextMeshProUGUI txtMensaje;

    private FirebaseAuth auth;
    private DatabaseReference db;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseDatabase.DefaultInstance.RootReference;

                // 🔥 AUTO LOGIN
                if (auth.CurrentUser != null)
                {
                    Debug.Log("Usuario ya logueado: " + auth.CurrentUser.Email);
                    txtMensaje.text = "Bienvenido de nuevo 👋";
                }
            }
            else
            {
                txtMensaje.text = "Error con Firebase";
            }
        });
    }

    // 🔐 LOGIN
    public void Login()
    {
        string email = inputEmail.text;
        string password = inputPassword.text;

        if (email == "" || password == "")
        {
            txtMensaje.text = "Completa los campos";
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    txtMensaje.text = "Login exitoso ";
                    Debug.Log("Login correcto");
                }
                else
                {
                    txtMensaje.text = "Error al iniciar sesión ";
                }
            });
    }

    // 📝 REGISTRO
    public void Register()
    {
        string email = inputEmail.text;
        string password = inputPassword.text;
        string nombre = inputNombre.text;

        if (email == "" || password == "" || nombre == "")
        {
            txtMensaje.text = "Completa todos los campos";
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    FirebaseUser user = task.Result.User;

                    txtMensaje.text = "Registro exitoso 🎉";
                    Debug.Log("Usuario creado: " + user.Email);

                    // 🔥 GUARDAR EN BASE DE DATOS
                    GuardarUsuario(user.UserId, email, nombre);
                }
                else
                {
                    txtMensaje.text = "Error al registrarse ❌";
                }
            });
    }

    // 💾 GUARDAR DATOS EN FIREBASE
    void GuardarUsuario(string userId, string email, string nombre)
    {
        Usuario nuevoUsuario = new Usuario(nombre, email, 0); // score inicia en 0

        string json = JsonUtility.ToJson(nuevoUsuario);

        db.Child("usuarios").Child(userId).SetRawJsonValueAsync(json);

        Debug.Log("Usuario guardado en DB");
    }

    // 🏆 ACTUALIZAR SCORE (LLÁMALO DESDE TU JUEGO)
    public void ActualizarScore(int nuevoScore)
    {
        string userId = auth.CurrentUser.UserId;

        db.Child("usuarios").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                int scoreActual = int.Parse(snapshot.Child("scoreMax").Value.ToString());

                if (nuevoScore > scoreActual)
                {
                    db.Child("usuarios").Child(userId).Child("scoreMax").SetValueAsync(nuevoScore);
                    Debug.Log("Nuevo récord guardado 🔥");
                }
            }
        });
    }

    // 🚪 LOGOUT
    public void Logout()
    {
        auth.SignOut();
        txtMensaje.text = "Sesión cerrada";
    }
}