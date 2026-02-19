using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Instancia pública accesible desde cualquier script
    public static GameManager Instance { get; private set; }

    // Variables globales de ejemplo
    public int Puntuacion;

    private void Awake()
    {
        // Si ya existe una instancia y no somos nosotros, nos destruimos
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Asignamos la instancia
        Instance = this;

        // Evita que se destruya al cambiar de escena
        DontDestroyOnLoad(gameObject);
    }

    private void ResetPoints()
    {
        Puntuacion = 0;
    }
}