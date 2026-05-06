[System.Serializable]
public class Usuario
{
    public string nombre;
    public string email;
    public float tiempo;

    public Usuario(string nombre, string email, float tiempo)
    {
        this.nombre = nombre;
        this.email = email;
        this.tiempo = tiempo;
    }
}